using BepInEx;
using BepInEx.Logging;
using Figgle;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using ShipLootPlus.Utils;
using System;
using System.IO;

namespace ShipLootPlus
{
    public class PluginMetadata
    {
        public const string Author = "PXC";
        public const string Name = "ShipLootPlus";
        public const string Id = "PXC.ShipLootPlus";
        public const string Version = "1.0.0";
        public string FullName => string.Format("{0} v{1}", Name, Version);
    }

    [BepInPlugin(PluginMetadata.Id, PluginMetadata.Name, PluginMetadata.Version)]
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.HardDependency)]
    public class ShipLootPlus : BaseUnityPlugin
    {
        #region Plugin Entry

        public static PluginMetadata pluginMetadata = new PluginMetadata();
        public static ManualLogSource Log = new ManualLogSource(PluginMetadata.Id);
        public static ShipLootPlus Instance;
        public static Dictionary<string, int> UiElements = new Dictionary<string, int>();

        /// <summary>
        /// Plugin entry
        /// </summary>
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            Log = Logger;
            Log.LogInfo(string.Format("Initializing plugin: {0} by {1}", pluginMetadata.FullName, PluginMetadata.Author));

            Assembly originalAssembly = LoadOriginal();
            if (originalAssembly != null)
            {
                Log.LogInfo("");
                Log.LogInfo("");
                Log.LogError(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Log.LogError($"Original ShipLoot has been detected - Disabing {pluginMetadata.FullName}...");
                Log.LogWarning($"If you want to use {pluginMetadata.FullName} please disable the original ShipLoot!");
                Log.LogError("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                Log.LogInfo("");
                Log.LogInfo("");
                Log.LogInfo(string.Format("Unloaded plugin: {0} by {1}", pluginMetadata.FullName, PluginMetadata.Author));
                return;
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginMetadata.Id);
            ConfigSettings.Initialize(Config, "Shows ShipLoot, Quota, and Days left information on the scan HUD");
            Log.LogInfo(string.Format("Loaded!\n{0}", FiggleFonts.Doom.Render(pluginMetadata.FullName)));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Element positions
        /// </summary>
        public enum ElementLocation
        {
            Top,
            Middle,
            Bottom
        }

        /// <summary>
        /// Loads the original ShipLoot assembly if it exists
        /// </summary>
        /// <returns></returns>
        private static Assembly LoadOriginal()
        {
            try
            {
                Assembly assembly = AppDomain.CurrentDomain.Load("ShipLoot");
                Log.LogInfo($"Found Original ShipLoot: {assembly}");
                return assembly;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
        #endregion
    }
}
