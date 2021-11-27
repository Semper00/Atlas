using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

using Atlas.Entities;
using Atlas.Commands.Builders;
using Atlas.Commands.Converters;
using Atlas.Commands.Converters.Results;
using Atlas.Commands.Entities;
using Atlas.Commands.Enums;
using Atlas.Commands.Interfaces;

namespace Atlas.Commands
{
    public class CommandManager : IDisposable
    {
        public static readonly List<CommandManager> All = new List<CommandManager>();

        public event Action<CommandContext, CommandInfo, IResult> OnCommandExecuted;
        public event Action<CommandContext, CommandInfo, Exception> OnCommandErrored;

        private readonly SemaphoreSlim _moduleLock;
        private readonly ConcurrentDictionary<Type, CommandModuleInfo> _typedModuleDefs;
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, Converter>> _converters;
        private readonly ConcurrentDictionary<Type, Converter> _defaultConverters;
        private readonly ImmutableList<(Type, Type)> _entityConverters;
        private readonly HashSet<CommandModuleInfo> _moduleDefs;
        private readonly CommandMap _map;

        internal bool _isDisposed;

        public IEnumerable<CommandModuleInfo> Modules => _moduleDefs.Select(x => x);

        public IEnumerable<CommandInfo> Commands => _moduleDefs.SelectMany(x => x.Commands);

        public ILookup<Type, Converter> Converters => _converters.SelectMany(x => x.Value.Select(y => new { y.Key, y.Value })).ToLookup(x => x.Key, x => x.Value);

        public CommandManagerConfig Config { get; }

        public CommandManager() : this(new CommandManagerConfig()) { }

        public CommandManager(CommandManagerConfig config)
        {
            All.Add(this);

            Config = config;

            _moduleLock = new SemaphoreSlim(1, 1);
            _typedModuleDefs = new ConcurrentDictionary<Type, CommandModuleInfo>();
            _moduleDefs = new HashSet<CommandModuleInfo>();
            _map = new CommandMap(this);
            _converters = new ConcurrentDictionary<Type, ConcurrentDictionary<Type, Converter>>();
            _defaultConverters = new ConcurrentDictionary<Type, Converter>();

            foreach (var type in PrimitiveConverters.SupportedTypes)
            {
                _defaultConverters[type] = PrimitiveTypeConverter.Create(type);
                _defaultConverters[typeof(Nullable<>).MakeGenericType(type)] = NullableConverter.Create(type, _defaultConverters[type]);
            }

            var tsreader = new TimeSpanConverter();

            _defaultConverters[typeof(TimeSpan)] = tsreader;
            _defaultConverters[typeof(TimeSpan?)] = NullableConverter.Create(typeof(TimeSpan), tsreader);

            _defaultConverters[typeof(string)] =
                new PrimitiveTypeConverter<string>((string x, out string y) => { y = x; return true; }, 0);

            var entityConverters = ImmutableList.CreateBuilder<(Type, Type)>();

            entityConverters.Add((typeof(Camera), typeof(CameraConverter)));
            entityConverters.Add((typeof(Door), typeof(DoorConverter)));
            entityConverters.Add((typeof(Elevator), typeof(ElevatorConvertor)));
            entityConverters.Add((typeof(Player), typeof(PlayerConverter)));
            entityConverters.Add((typeof(Prefab), typeof(PrefabConverter)));
            entityConverters.Add((typeof(Room), typeof(RoomConvertor)));

            _entityConverters = entityConverters.ToImmutable();
        }

        ~CommandManager()
        {
            All.Remove(this);
        }

        public CommandModuleInfo CreateModule(string primaryAlias, Action<CommandModuleInfoBuilder> buildFunc)
        {
            _moduleLock.Wait();

            try
            {
                var builder = new CommandModuleInfoBuilder(this, null, primaryAlias);

                buildFunc(builder);

                var module = builder.Build(this, null);

                return LoadModuleInternal(module);
            }
            finally
            {
                _moduleLock.Release();
            }
        }

        public CommandModuleInfo AddModule<T>() => AddModule(typeof(T));

        public CommandModuleInfo AddModule(Type type)
        {
            _moduleLock.Wait();

            try
            {
                var typeInfo = type.GetTypeInfo();

                if (_typedModuleDefs.ContainsKey(type))
                    throw new ArgumentException("This module has already been added.");

                var module = CommandModuleClassBuilder.Build(this, typeInfo).FirstOrDefault();

                if (module.Value == default(CommandModuleInfo))
                    throw new InvalidOperationException($"Could not build the module {type.FullName}, did you pass an invalid type?");

                _typedModuleDefs[module.Key] = module.Value;

                return LoadModuleInternal(module.Value);
            }
            finally
            {
                _moduleLock.Release();
            }
        }

        public IEnumerable<CommandModuleInfo> AddModules(Assembly assembly)
        {
            _moduleLock.Wait();

            try
            {
                var types = CommandModuleClassBuilder.Search(assembly, this);
                var moduleDefs = CommandModuleClassBuilder.Build(types, this);

                foreach (var info in moduleDefs)
                {
                    _typedModuleDefs[info.Key] = info.Value;
                    LoadModuleInternal(info.Value);
                }

                return moduleDefs.Select(x => x.Value).ToImmutableArray();
            }
            finally
            {
                _moduleLock.Release();
            }
        }

        private CommandModuleInfo LoadModuleInternal(CommandModuleInfo module)
        {
            _moduleDefs.Add(module);

            foreach (var command in module.Commands)
                _map.AddCommand(command);

            foreach (var submodule in module.Submodules)
                LoadModuleInternal(submodule);

            return module;
        }

        public bool RemoveModule(CommandModuleInfo module)
        {
            _moduleLock.Wait();

            try
            {
                return RemoveModuleInternal(module);
            }
            finally
            {
                _moduleLock.Release();
            }
        }

        public bool RemoveModule<T>() => RemoveModule(typeof(T));

        public bool RemoveModule(Type type)
        {
            _moduleLock.Wait();

            try
            {
                if (!_typedModuleDefs.TryRemove(type, out var module))
                    return false;

                return RemoveModuleInternal(module);
            }
            finally
            {
                _moduleLock.Release();
            }
        }

        private bool RemoveModuleInternal(CommandModuleInfo module)
        {
            if (!_moduleDefs.Remove(module))
                return false;

            foreach (var cmd in module.Commands)
                _map.RemoveCommand(cmd);

            foreach (var submodule in module.Submodules)
            {
                RemoveModuleInternal(submodule);
            }

            return true;
        }

        public void AddConverter<T>(Converter reader)
            => AddConverter(typeof(T), reader);

        public void AddConverter(Type type, Converter reader)
        {
            AddConverter(type, reader, true);
        }

        public void AddConverter<T>(Converter reader, bool replaceDefault)
            => AddConverter(typeof(T), reader, replaceDefault);

        public void AddConverter(Type type, Converter reader, bool replaceDefault)
        {
            if (replaceDefault && HasDefaultConverter(type))
            {
                _defaultConverters.AddOrUpdate(type, reader, (k, v) => reader);

                if (type.GetTypeInfo().IsValueType)
                {
                    var nullableType = typeof(Nullable<>).MakeGenericType(type);
                    var nullableReader = NullableConverter.Create(type, reader);
                    _defaultConverters.AddOrUpdate(nullableType, nullableReader, (k, v) => nullableReader);
                }
            }
            else
            {
                var readers = _converters.GetOrAdd(type, x => new ConcurrentDictionary<Type, Converter>());

                readers[reader.GetType()] = reader;

                if (type.GetTypeInfo().IsValueType)
                    AddNullableConverter(type, reader);
            }
        }

        internal bool HasDefaultConverter(Type type)
        {
            if (_defaultConverters.ContainsKey(type))
                return true;

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
                return true;
            return _entityConverters.Any(x => type == x.Item1 || typeInfo.ImplementedInterfaces.Contains(x.Item1));
        }

        internal void AddNullableConverter(Type valueType, Converter valueConverter)
        {
            var readers = _converters.GetOrAdd(typeof(Nullable<>).MakeGenericType(valueType), x => new ConcurrentDictionary<Type, Converter>());
            var nullableReader = NullableConverter.Create(valueType, valueConverter);
            readers[nullableReader.GetType()] = nullableReader;
        }

        internal IDictionary<Type, Converter> GetConverters(Type type)
        {
            if (_converters.TryGetValue(type, out var definedConverters))
                return definedConverters;

            return null;
        }

        internal Converter GetDefaultConverter(Type type)
        {
            if (_defaultConverters.TryGetValue(type, out var reader))
                return reader;
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsEnum)
            {
                reader = EnumConverter.GetReader(type);
                _defaultConverters[type] = reader;
                return reader;
            }

            for (int i = 0; i < _entityConverters.Count; i++)
            {
                if (type == _entityConverters[i].Item1 || typeInfo.ImplementedInterfaces.Contains(_entityConverters[i].Item1))
                {
                    reader = Activator.CreateInstance(_entityConverters[i].Item2.MakeGenericType(type)) as Converter;
                    _defaultConverters[type] = reader;
                    return reader;
                }
            }

            return null;
        }

        public SearchResult Search(CommandContext context, int argPos)
            => Search(context.Text.Substring(argPos));

        public SearchResult Search(CommandContext context, string input)
            => Search(input);

        public SearchResult Search(string input)
        {
            string searchInput = Config.CaseSensitiveCommands ? input : input.ToLowerInvariant();
            var matches = _map.GetCommands(searchInput).OrderByDescending(x => x.Command.Priority).ToImmutableArray();

            if (matches.Length > 0)
                return SearchResult.FromSuccess(input, matches);
            else
                return SearchResult.FromError(CommandError.UnknownCommand, "Unknown command.");
        }

        public IResult Execute(CommandContext context, int argPos)
            => Execute(context, context.Text.Substring(argPos));

        public IResult Execute(CommandContext context, string input)
        {
            try
            {
                var searchResult = Search(input);

                if (!searchResult.IsSuccess)
                {
                    OnCommandExecuted.Invoke(context, null, searchResult);

                    return searchResult;
                }

                var commands = searchResult.Commands;
                var parseResultsDict = new Dictionary<CommandMatch, ParseResult>();

                foreach (var cmd in commands)
                {
                    var parseResult = cmd.Parse(context, searchResult);

                    if (parseResult.Error == CommandError.MultipleMatches)
                    {
                        IReadOnlyList<ConverterValue> argList, paramList;

                        argList = parseResult.ArgValues.Select(x => x.Values.OrderByDescending(y => y.Score).First()).ToImmutableArray();
                        paramList = parseResult.ParamValues.Select(x => x.Values.OrderByDescending(y => y.Score).First()).ToImmutableArray();
                        parseResult = ParseResult.FromSuccess(argList, paramList);
                    }

                    parseResultsDict[cmd] = parseResult;
                }

                float CalculateScore(CommandMatch match, ParseResult parseResult)
                {
                    float argValuesScore = 0, paramValuesScore = 0;

                    if (match.Command.Parameters.Count > 0)
                    {
                        var argValuesSum = parseResult.ArgValues?.Sum(x => x.Values.OrderByDescending(y => y.Score).FirstOrDefault().Score) ?? 0;
                        var paramValuesSum = parseResult.ParamValues?.Sum(x => x.Values.OrderByDescending(y => y.Score).FirstOrDefault().Score) ?? 0;

                        argValuesScore = argValuesSum / match.Command.Parameters.Count;
                        paramValuesScore = paramValuesSum / match.Command.Parameters.Count;
                    }

                    var totalArgsScore = (argValuesScore + paramValuesScore) / 2;
                    return match.Command.Priority + totalArgsScore * 0.99f;
                }

                var parseResults = parseResultsDict
                    .OrderByDescending(x => CalculateScore(x.Key, x.Value));

                var successfulParses = parseResults
                    .Where(x => x.Value.IsSuccess)
                    .ToArray();

                if (successfulParses.Length == 0)
                {
                    var bestMatch = parseResults
                        .FirstOrDefault(x => !x.Value.IsSuccess);

                    OnCommandExecuted.Invoke(context, bestMatch.Key.Command, bestMatch.Value);

                    return bestMatch.Value;
                }

                var chosenOverload = successfulParses[0];
                var result = chosenOverload.Key.Execute(context, chosenOverload.Value);

                if (!result.IsSuccess && !(result is ExecuteResult))
                    OnCommandExecuted.Invoke(context, chosenOverload.Key.Command, result);

                return result;
            }
            catch (Exception e)
            {
                OnCommandErrored.Invoke(context, null, e);

                return ExecuteResult.FromError(e);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _moduleLock?.Dispose();
                }

                _isDisposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        public static IResult Execute(CommandContext ctx)
        {
            foreach (CommandManager cmdManager in All)
            {
                SearchResult result = cmdManager.Search(ctx.Text);

                if (result.IsSuccess)
                {
                    return cmdManager.Execute(ctx, ctx.Text);             
                }
                else
                {
                    continue;
                }
            }

            return ExecuteResult.FromError(CommandError.UnknownCommand, "Command not found.");
        }
    }
}