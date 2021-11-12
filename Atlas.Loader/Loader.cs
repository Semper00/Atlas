using System;
using System.Net;
using System.IO;
using System.Reflection;

using UnityEngine;

namespace Atlas.Loader
{
    public sealed class Loader
    {
        public const string LatestRelease = "https://github.com/Semper00/Atlas/releases/latest/download/";

        public static string AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string ScpSlAppData => $"{AppData}/SCP Secret Laboratory";
        public static string SCPSL_Data => Application.dataPath;
        public static string Home => Directory.GetParent(SCPSL_Data).FullName;

        public static string Folder => $"{Home}/Atlas";
        public static string ThisFolder => $"{Folder}/{(ServerStatic.ServerPortSet ? ServerStatic.ServerPort : 7777)}";
        public static string Dependencies => $"{ThisFolder}/Dependencies";
        public static string Mods => $"{ThisFolder}/Mods";
        public static string Translations => $"{ThisFolder}/Translations";
        public static string Configs => $"{ThisFolder}/Configuration";
        public static string ModConfigs => $"{Configs}/Mods";
        public static string ModTranslations => $"{Translations}/Mods";
        public static string Atlas => ThisFolder;

        public static string AtlasFile => $"{Atlas}/Atlas.dll";

        public static string YamlDotNet => $"{Dependencies}/YamlDotNet.dll";
        public static string ComponentModel => $"{Dependencies}/System.ComponentModel.Annotations.dll";
        public static string Harmony => $"{Dependencies}/0Harmony.dll";

        public static void Check()
        {
            try
            {
                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);

                if (!Directory.Exists(ThisFolder))
                    Directory.CreateDirectory(ThisFolder);

                if (!Directory.Exists(Dependencies))
                    Directory.CreateDirectory(Dependencies);

                if (!Directory.Exists(Mods))
                    Directory.CreateDirectory(Mods);

                if (!Directory.Exists(Translations))
                    Directory.CreateDirectory(Translations);

                if (!Directory.Exists(Configs))
                    Directory.CreateDirectory(Configs);

                if (!Directory.Exists(Atlas))
                    Directory.CreateDirectory(Atlas);

                if (!Directory.Exists(ModConfigs))
                    Directory.CreateDirectory(ModConfigs);

                if (!Directory.Exists(ModTranslations))
                    Directory.CreateDirectory(ModTranslations);
            }
            catch (Exception ex)
            {
                ServerConsole.AddLog(ex.ToString(), ConsoleColor.DarkRed);
            }
        }

        public static void Log(object message)
                => ServerConsole.AddLog($"[INFO] [Atlas] {message}", ConsoleColor.Red);

        public static void Download(string link, string destination)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(link, destination);
            }
        }

        public static void VerifyFile(string path, string link)
        {
            if (!File.Exists(path))
                Download(link, path);
        }

        public static void Load()
        {
            try
            {
                string AtlasLink = $"{LatestRelease}Atlas.dll";
                string yamlLink = $"{LatestRelease}YamlDotNet.dll";
                string componentModelLink = $"{LatestRelease}System.ComponentModel.Annotations.dll";
                string harmonyLink = $"{LatestRelease}0Harmony.dll";

                Check();

                VerifyFile(AtlasFile, AtlasLink);
                VerifyFile(YamlDotNet, yamlLink);
                VerifyFile(ComponentModel, componentModelLink);
                VerifyFile(Harmony, harmonyLink);

                Assembly atlasA = Assembly.Load(File.ReadAllBytes(AtlasFile));
                Assembly harmonyA = Assembly.Load(File.ReadAllBytes(Harmony));
                Assembly componentModelA = Assembly.Load(File.ReadAllBytes(ComponentModel));
                Assembly yamlDotNetA = Assembly.Load(File.ReadAllBytes(YamlDotNet));

                atlasA?.GetType("Atlas.ModManager.ModLoader")?.GetMethod("Reload")?.Invoke(null,
                    new object[]
                    {
                        new Assembly[]
                        {
                            harmonyA,
                            componentModelA,
                            yamlDotNetA,
                        }
                    });
            }
            catch (Exception e)
            {
                Log(e);

                return;
            }
        }
    }
}