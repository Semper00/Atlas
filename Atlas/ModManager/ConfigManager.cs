using System;
using System.IO;
using System.Reflection;

using Atlas.ModManager.Configs.Extensions;

using Atlas.Interfaces;

namespace Atlas.ModManager
{
    public static class ConfigManager
    {
        public static string Path(IModBase<IConfig> mod)
            => $"{Loader.Loader.ModConfigs}/{mod.Name}.yml";

        public static string Path(IConfig cfg)
        {
            Type type = cfg.GetType();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public);

            string path = $"{Loader.Loader.Configs}/{type.Name}.yml";

            if (fields != null && fields.Length > 0)
            {
                foreach (FieldInfo field in fields)
                {
                    if (field.Name == "Path" && field.FieldType == typeof(string))
                        path = field.GetValue(cfg)?.ToString() ?? path;
                }
            }

            return path;
        }

        public static void Reload(IModBase<IConfig> mod)
        {
            string path = Path(mod);

            if (!File.Exists(path))
            {
                try
                {
                    Functions.Info($"{mod.Name} does not have default configs, generating ...");

                    File.Create(path).Close();

                    string serialized = ModLoader.Serializer.Serialize(mod.Config);

                    File.WriteAllText(path, serialized);
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
            else
            {
                try
                {
                    string nonDeserialzed = File.ReadAllText(path);

                    mod.Config.CopyProperties(ModLoader.Deserializer.Deserialize(nonDeserialzed, mod.Config.GetType()));
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
        }

        public static void Save(IModBase<IConfig> mod)
        {
            string path = Path(mod);

            if (!File.Exists(path))
            {
                try
                {
                    File.Create(path).Close();

                    string serialized = ModLoader.Serializer.Serialize(mod.Config);

                    File.WriteAllText(path, serialized);
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
            else
            {
                File.WriteAllText(path, ModLoader.Serializer.Serialize(mod.Config));
            }
        }

        public static void Read(IModBase<IConfig> mod)
        {
            string path = Path(mod);

            if (!File.Exists(path))
            {
                return;
            }
            else
            {
                mod.Config.CopyProperties(ModLoader.Deserializer.Deserialize(File.ReadAllText(path), mod.Config.GetType()));
            }
        }

        public static void Reload(IConfig cfg)
        {
            string path = Path(cfg);

            if (!File.Exists(path))
            {
                try
                {
                    Functions.Info($"{cfg.GetType().Assembly.GetName().Name} does not have default configs, generating ...");

                    File.Create(path).Close();

                    string serialized = ModLoader.Serializer.Serialize(cfg);

                    File.WriteAllText(path, serialized);
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
            else
            {
                try
                {
                    string nonDeserialzed = File.ReadAllText(path);

                    cfg.CopyProperties(ModLoader.Deserializer.Deserialize(nonDeserialzed, cfg.GetType()));
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
        }

        public static void Save(IConfig cfg)
        {
            string path = Path(cfg);

            if (!File.Exists(path))
            {
                try
                {
                    File.Create(path).Close();
                    File.WriteAllText(path, ModLoader.Serializer.Serialize(cfg));
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
            else
            {
                File.WriteAllText(path, ModLoader.Serializer.Serialize(cfg));
            }
        }

        public static void Read(IConfig cfg, bool throwifNotExits = true)
        {
            string path = Path(cfg);

            if (!File.Exists(path))
            {
                if (throwifNotExits)
                    throw new InvalidOperationException($"The config file of {cfg.GetType().Name}@{cfg.GetType().Assembly.GetName().Name}, therefore it cannot be read!");
            }
            else
            {
                cfg.CopyProperties(ModLoader.Deserializer.Deserialize(File.ReadAllText(path), cfg.GetType()));
            }
        }
    }
}