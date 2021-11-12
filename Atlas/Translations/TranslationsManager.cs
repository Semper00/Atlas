using System;
using System.IO;
using System.Reflection;

using Atlas.ModManager;
using Atlas.ModManager.Configs.Extensions;
using Atlas.Interfaces;

namespace Atlas.Translations
{
    public static class TranslationManager
    {
        public static string Path(IModBase<IConfig> mod)
            => $"{Loader.Loader.ModTranslations}/{mod.Name}.yml";

        public static void Info(object msg)
            => Log.Info("Atlas", msg);

        public static string Path(Translation translation)
        {
            Type type = translation.GetType();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public);

            string path = $"{Loader.Loader.Translations}/{type.Name}.yml";

            if (fields != null && fields.Length > 0)
            {
                foreach (FieldInfo field in fields)
                {
                    if (field.Name == "Path" && field.FieldType == typeof(string))
                        path = field.GetValue(translation)?.ToString() ?? path;
                }
            }

            return path;
        }

        public static void FindTranslations(IModBase<IConfig> mod)
        {
            try
            {
                foreach (Type type in mod.Assembly.GetTypes())
                {
                    if (!type.IsPublic)
                        continue;

                    if (!type.IsSubclassOf(typeof(Translation)))
                        continue;

                    Info($"Found a Translation class in {mod.Name} ...");

                    Translation translation = (Translation)Activator.CreateInstance(type);

                    if (translation != null)
                    {
                        mod.Translations.Add(translation.Language, translation);

                        Info($"Added a translation ({translation.Language}) to {mod.Name}");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Atlas", e);
            }
        }

        public static void Reload(IModBase<IConfig> mod)
        {
            if (mod.Translations.Count < 1)
                return;

            string path = Path(mod);

            if (!File.Exists(path))
            {
                try
                {
                    Info($"{mod.Name} does not have default translations, generating ...");

                    File.Create(path).Close();

                    File.WriteAllText(path, ModLoader.Serializer.Serialize(mod.Translations));
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
                    string non = File.ReadAllText(path);

                    mod.Translations.CopyProperties(ModLoader.Deserializer.Deserialize(non, mod.Translations.GetType()));
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
        }

        public static void Save(IModBase<IConfig> mod)
        {
            if (mod.Translations.Count < 1)
                return;

            string path = Path(mod);

            if (!File.Exists(path))
            {
                try
                {
                    File.Create(path).Close();

                    string serialized = ModLoader.Serializer.Serialize(mod.Translations);

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
                mod.Translations.CopyProperties(ModLoader.Deserializer.Deserialize(File.ReadAllText(path), mod.Translations.GetType()));
            }
        }

        public static void Reload(Translation translation)
        {
            string path = Path(translation);

            if (!File.Exists(path))
            {
                try
                {
                    File.Create(path).Close();

                    string serialized = ModLoader.Serializer.Serialize(translation);

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

                    translation.CopyProperties(ModLoader.Deserializer.Deserialize(nonDeserialzed, translation.GetType()));
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
        }

        public static void Save(Translation translation)
        {
            string path = Path(translation);

            if (!File.Exists(path))
            {
                try
                {
                    File.Create(path).Close();

                    string serialized = ModLoader.Serializer.Serialize(translation);

                    File.WriteAllText(path, serialized);
                }
                catch (Exception e)
                {
                    Log.Error("Atlas", e);
                }
            }
            else
            {
                File.WriteAllText(path, ModLoader.Serializer.Serialize(translation));
            }
        }

        public static void Read(Translation translation)
        {
            string path = Path(translation);

            if (!File.Exists(path))
            {
                return;
            }
            else
            {
                translation.CopyProperties(ModLoader.Deserializer.Deserialize(File.ReadAllText(path), translation.GetType()));
            }
        }
    }
}
