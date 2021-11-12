using System;
using System.IO;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Atlas.Injector
{
    public static class Injector
    {
        public static void Main(string[] args)
        {
            try
            {
                Log("Welcome to the Atlas Injector!");

                string path = "";
                string loaderPath = "";

                if (File.Exists(Directory.GetCurrentDirectory() + "/assemblypath.txt"))
                    path = File.ReadAllText(Directory.GetCurrentDirectory() + "/assemblypath.txt");

                if (File.Exists(Directory.GetCurrentDirectory() + "/loaderpath.txt"))
                    loaderPath = File.ReadAllText(Directory.GetCurrentDirectory() + "/loaderpath.txt");

                if (string.IsNullOrWhiteSpace(path))
                {
                    if (args.Length != 1)
                    {
                        Console.WriteLine("Provide the location of Assembly-CSharp.dll");

                        path = Console.ReadLine();
                    }
                    else
                    {
                        path = args[0];
                    }
                }

                Log($"Assembly-CSharp path: {path}");

                ModuleDefMD module = ModuleDefMD.Load(path);

                if (module == null)
                {
                    Log($"Failed to locate Assembly-CSharp at the specified path!");

                    Console.Read();

                    return;
                }

                Log($"Succesfully loaded {module.Name}!");

                module.Context = ModuleDef.CreateModuleContext();

                ((AssemblyResolver)module.Context.AssemblyResolver).AddToCache(module);

                Log("Starting the injection!");

                if (string.IsNullOrWhiteSpace(loaderPath))
                    loaderPath = Path.Combine(Directory.GetCurrentDirectory(), "Atlas.Loader.dll");

                ModuleDefMD loader = ModuleDefMD.Load(loaderPath);

                Log($"Found the loader file! ({loader.Location})");

                TypeDef modClass = loader.Types[0];

                foreach (var type in loader.Types)
                {
                    if (type.Name == "Loader")
                    {
                        modClass = type;

                        Log($"Using \"{type.FullName}\" for injection!");
                    }
                }

                var modRefType = modClass;

                loader.Types.Remove(modClass);

                modRefType.DeclaringType = null;

                module.Types.Add(modRefType);

                Log("Searching for the loading method ..");

                MethodDef call = FindMethod(modRefType, "Load");

                if (call == null)
                {
                    Log("Failed to find the loading method!", true);

                    Console.Read();

                    return;
                }

                Log("Injecting the loader into the server assembly ..");

                TypeDef typeDef = FindType(module.Assembly, "ServerConsole");

                MethodDef start = FindMethod(typeDef, "Start");

                if (start == null)
                {
                    start = new MethodDefUser("Start", MethodSig.CreateInstance(module.CorLibTypes.Void), MethodImplAttributes.IL 
                        | MethodImplAttributes.Managed, MethodAttributes.Private 
                        | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

                    typeDef.Methods.Add(start);
                }

                start.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(call));

                Log("Loader injected succesfully!");

                Log("Replacing the original Assembly-CSharp ...");

                File.Delete(path);

                module.Write(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path)), "Assembly-CSharp.dll"));

                Log("The Injector has succesfully finished!");
            }
            catch (Exception exception)
            {
                Log($"An error has occurred while injecting!", true);
                Log(exception, true);
            }

            Console.Read();
        }

        public static void Log(object msg, bool over = false)
        {
            SetColor(over ? ConsoleColor.Red : ConsoleColor.Green);

            Console.WriteLine($"<{DateTime.Now.ToLocalTime().ToString("s")}> [STDOUT] {msg}");

            ResetColor();
        }

        public static void SetColor(ConsoleColor color)
            => Console.ForegroundColor = color;

        public static void ResetColor()
            => Console.ResetColor();

        private static MethodDef FindMethod(TypeDef type, string methodName)
        {
            Log($"Searching for method: {type.FullName}.{methodName}");

            if (type != null)
            {
                foreach (var method in type.Methods)
                {
                    if (method.Name == methodName)
                        return method;
                }
            }

            return null;
        }

        private static TypeDef FindType(AssemblyDef assembly, string path)
        {
            Log($"Searching for type: {path}@{assembly.Name}");

            foreach (var module in assembly.Modules)
            {
                foreach (var type in module.Types)
                {
                    if (type.FullName == path)
                        return type;
                }
            }

            return null;
        }
    }
}