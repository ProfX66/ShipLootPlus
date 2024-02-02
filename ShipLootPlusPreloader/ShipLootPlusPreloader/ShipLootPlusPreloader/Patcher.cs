using HarmonyLib;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BepInEx.ShipLootPlusPreloader
{
    public class NeuteredAssembly
    {
        public string Name;
        public string FullName;
    }

    public static class Patcher
    {
        public static string Id => "ShipLootPlus.Preloader";
        public static IEnumerable<string> TargetDLLs { get; } = new string[] { };
        internal static Logging.ManualLogSource Logger = Logging.Logger.CreateLogSource(Id);
        public static List<NeuteredAssembly> neuteredAssemblies = new List<NeuteredAssembly>
        {
            new NeuteredAssembly { Name = "ShipLoot", FullName = "ShipLoot.ShipLoot" }
        };

        public static void Initialize()
        {
            Logger.LogInfo("Initializing preloader event hooks");
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs e)
        {
            Harmony harmony = new Harmony(Id);
            Assembly loadedAssembly = e.LoadedAssembly;
            NeuteredAssembly assemblyToNeuter = neuteredAssemblies.FirstOrDefault(a => Regex.IsMatch(loadedAssembly.FullName, $"^{Regex.Escape(a.Name)},", RegexOptions.IgnoreCase));
            if (assemblyToNeuter == null) return;
            Logger.LogMessage($"Found plugin to neuter: {assemblyToNeuter.Name} ({loadedAssembly.FullName})");

            HarmonyMethod harmonyMethod = new HarmonyMethod(typeof(Patcher).GetMethod("Neuter", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
            foreach (Type type in loadedAssembly.GetTypes())
            {
                if (!(type.FullName == assemblyToNeuter.FullName)) continue;
                MethodInfo[] typeMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                foreach (MethodInfo methodInfo in typeMethods)
                {
                    if (methodInfo.Name == "Awake")
                    {
                        Logger.LogMessage($"Attempting to remove 'Awake' from: {assemblyToNeuter.Name} ({loadedAssembly.FullName})");
                        try
                        {
                            harmony.Patch((MethodBase)methodInfo, harmonyMethod, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
                            Logger.LogMessage($"Successfully neutered: {assemblyToNeuter.Name} ({loadedAssembly.FullName})");
                        }
                        catch (Exception Ex)
                        {
                            Logger.LogError($"Failed to neuter: {assemblyToNeuter.Name} ({loadedAssembly.FullName})");
                            Logger.LogError($"Exception: {Ex.Message}");
                        }
                    }
                }
            }
        }

        private static bool Neuter()
        {
            return false;
        }

        public static void Patch(AssemblyDefinition _)
        {
        }
    }
}
