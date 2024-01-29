using BepInEx;
using BepInEx.Logging;
using Figgle;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using ShipLootPlus.Utils;
using System;
using System.IO;
using ShipLootPlus.Patches;
using System.Text.RegularExpressions;

namespace ShipLootPlus
{
    public class PluginMetadata
    {
        public const string Author = "PXC";
        public const string Name = "ShipLootPlus";
        public const string Id = "PXC.ShipLootPlus";
        public const string Version = "1.0.3";
        public string FullName => string.Format("{0} v{1}", Name, Version);
    }

    [BepInPlugin(PluginMetadata.Id, PluginMetadata.Name, PluginMetadata.Version)]
    [BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
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

            if (AssemblyExists("ShipLoot"))
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

            UiHelper.DataSubSet = new List<string>();
            UiHelper.DataPoints = new List<ReplacementData>
            {
                new ReplacementData { Pattern = "%ShipLootValue%", Description = "Value of all scrap on ship"},
                new ReplacementData { Pattern = "%MoonLootValue%", Description = "Value of all scrap on moon"},
                new ReplacementData { Pattern = "%AllLootValue%", Description = "Value of all scrap total"},
                //new ReplacementData { Pattern = "%InventoryLootValue%", Description = "Value of all scrap on the player"},
                new ReplacementData { Pattern = "%ShipLootCount%", Description = "Count of all scrap on ship"},
                new ReplacementData { Pattern = "%MoonLootCount%", Description = "Count of all scrap on moon"},
                new ReplacementData { Pattern = "%AllLootCount%", Description = "Count of all scrap total"},
                //new ReplacementData { Pattern = "%InventoryLootCount%", Description = "Count of all scrap on the player"},
                new ReplacementData { Pattern = "%FulfilledValue%", Description = "Value of turned in scrap for quota"},
                new ReplacementData { Pattern = "%QuotaValue%", Description = "Value of current quota"},
                new ReplacementData { Pattern = "%CompanyRate%", Description = "Current company buy rate"},
                new ReplacementData { Pattern = "%ExpectedProfit%", Description = "Expected profit from scap on ship at current company buy rate"},
                new ReplacementData { Pattern = "%Deadline%", Description = "Quota deadline in days"},
                new ReplacementData { Pattern = "%DeadlineWithColors%", Description = "Quota deadline in days but changes colors based on value"},
                new ReplacementData { Pattern = "%DayNumber%", Description = "Number of days in the ship/save (E.g. 1, 10)"},
                new ReplacementData { Pattern = "%DayNumberHuman%", Description = "Human friendly days in the ship/save (E.g. 1st, 10th)"},
                new ReplacementData { Pattern = "%Weather%", Description = "Current moons weather (full name)"},
                new ReplacementData { Pattern = "%WeatherShort%", Description = "Current moons weather (short name)"},
                new ReplacementData { Pattern = "%MoonLongName%", Description = "Current moons full name"},
                new ReplacementData { Pattern = "%MoonShortName%", Description = "Current moons short name"}
            };
            
            int count = 1;
            foreach (ReplacementData item in UiHelper.DataPoints)
            {
                item.Name = Regex.Replace(item.Pattern, "%", "", RegexOptions.IgnoreCase);
                Log.LogInfo($"[DataPoint #{count:D2}] {item.Pattern} => {item.Description}");
                count++;
            }

            ConfigSettings.Initialize(Config, $"Allows showing up to {UiHelper.DataPoints.Count} customizable data points on your HUD.");
            ConfigSettings.AlwaysShow.SettingChanged += ToggleUi_SettingChanged;
            ConfigSettings.AllCaps.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.ShortCharLength.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.ShowLine.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineColor.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.ShowLineOne.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineOneColor.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineOneFormat.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.ShowLineTwo.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineTwoColor.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineTwoFormat.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.ShowLineThree.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineThreeColor.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineThreeFormat.SettingChanged += RedrawRequired_SettingChanged;
            
            Log.LogInfo(string.Format("Loaded!\n{0}", FiggleFonts.Doom.Render(pluginMetadata.FullName)));
        }

        #endregion

        #region Events

        /// <summary>
        /// When a setting is changed that requires a UI refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedrawRequired_SettingChanged(object sender, System.EventArgs e)
        {
            if (UiHelper.ContainerObject == null) return;
            UiHelper.ResetUiElements();
        }

        /// <summary>
        /// Enable/Disable the UI elements based on AlwaysShow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleUi_SettingChanged(object sender, System.EventArgs e)
        {
            if (UiHelper.ContainerObject == null) return;
            UiHelper.ContainerObject.SetActive(ConfigSettings.AlwaysShow.Value);
        }

        /// <summary>
        /// Refresh the UI data when AllCaps is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshUi_SettingChanged(object sender, System.EventArgs e)
        {
            if (UiHelper.ContainerObject == null) return;
            UiHelper.RefreshElementValues();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Tests if an assembly is available or not
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>bool</returns>
        public static bool AssemblyExists(string Name)
        {
            try
            {
                Assembly assembly = AppDomain.CurrentDomain.Load(Name);
                Log.LogInfo($"Found {Name}: {assembly}");
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        #endregion
    }
}
