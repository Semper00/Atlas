using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atlas.Commands.Utilities
{
    internal static class ReflectionUtils
    {
        private static readonly TypeInfo ObjectTypeInfo = typeof(object).GetTypeInfo();

        internal static T CreateObject<T>(TypeInfo typeInfo, CommandManager commands) where T : class
            => CreateBuilder<T>(typeInfo, commands)();

        internal static Func<T> CreateBuilder<T>(TypeInfo typeInfo, CommandManager commands)
        {
            var constructor = GetConstructor(typeInfo);
            var parameters = constructor.GetParameters();
            var properties = GetProperties(typeInfo);

            return () =>
            {
                var args = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                    args[i] = GetMember(commands, parameters[i].ParameterType, typeInfo);

                var obj = InvokeConstructor<T>(constructor, args, typeInfo);

                foreach (var property in properties)
                    property.SetValue(obj, GetMember(commands, property.PropertyType, typeInfo));

                return obj;
            };
        }

        private static T InvokeConstructor<T>(ConstructorInfo constructor, object[] args, TypeInfo ownerType)
        {
            try
            {
                return (T)constructor.Invoke(args);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create \"{ownerType.FullName}\".", ex);
            }
        }

        private static ConstructorInfo GetConstructor(TypeInfo ownerType)
        {
            var constructors = ownerType.DeclaredConstructors.Where(x => !x.IsStatic).ToArray();

            if (constructors.Length == 0)
                throw new InvalidOperationException($"No constructor found for \"{ownerType.FullName}\".");
            else if (constructors.Length > 1)
                throw new InvalidOperationException($"Multiple constructors found for \"{ownerType.FullName}\".");

            return constructors[0];
        }

        private static PropertyInfo[] GetProperties(TypeInfo ownerType)
        {
            var result = new List<PropertyInfo>();

            while (ownerType != ObjectTypeInfo)
            {
                foreach (var prop in ownerType.DeclaredProperties)
                {
                    if (prop.SetMethod?.IsStatic == false && prop.SetMethod?.IsPublic == true)
                        result.Add(prop);
                }

                ownerType = ownerType.BaseType.GetTypeInfo();
            }

            return result.ToArray();
        }

        private static object GetMember(CommandManager commands, Type memberType, TypeInfo ownerType)
        {
            if (memberType == typeof(CommandManager))
                return commands;

            return null;
        }
    }
}