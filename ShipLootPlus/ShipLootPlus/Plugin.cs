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
using System.Linq;
using BepInEx.Bootstrap;

namespace ShipLootPlus
{
    public class PluginMetadata
    {
        public const string Author = "PXC";
        public const string Name = "ShipLootPlus";
        public const string Id = "PXC.ShipLootPlus";
        public const string Version = "1.0.6";
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
            Log.LogInfo($"Initializing plugin: {pluginMetadata.FullName} by {PluginMetadata.Author}");

            Harmony.CreateAndPatchAll(typeof(DepositItemsDeskPatcher));
            Log.LogInfo("[Patched] DepositItemsDesk");

            Harmony.CreateAndPatchAll(typeof(GrabbableObjectPatcher));
            Log.LogInfo("[Patched] GrabbableObjects");

            Harmony.CreateAndPatchAll(typeof(HudManagerPatcher));
            Log.LogInfo("[Patched] HUDManager");

            Harmony.CreateAndPatchAll(typeof(PlayerControllerBPatcher));
            Log.LogInfo("[Patched] PlayerControllerB");

            Harmony.CreateAndPatchAll(typeof(RoundManagerPatcher));
            Log.LogInfo("[Patched] RoundManager");

            Harmony.CreateAndPatchAll(typeof(StartOfRoundPatcher));
            Log.LogInfo("[Patched] StartOfRound");

            Harmony.CreateAndPatchAll(typeof(TimeOfDayPatcher));
            Log.LogInfo("[Patched] TimeOfDay");

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

#if DEBUG
            Log.LogWarning($"Loaded! (IN DEBUG)\n{FiggleFonts.Doom.Render(pluginMetadata.FullName)}");
#endif
#if !DEBUG
            Log.LogInfo($"Loaded!\n{FiggleFonts.Doom.Render(pluginMetadata.FullName)}");
#endif
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
        /// Tests if a plugin is available or not
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>bool</returns>
        public static bool PluginExists(string Name, bool ShowWarning = true)
        {
            if (Chainloader.PluginInfos.ContainsKey(Name))
            {
                KeyValuePair<string, PluginInfo> plugin = Chainloader.PluginInfos.FirstOrDefault(n => n.Key == Name);
                
                if (ShowWarning) Log.LogInfo($"[SoftDependency] Found plugin: {plugin.Value.Metadata.Name} ({plugin.Value.Metadata.GUID}) v{plugin.Value.Metadata.Version} - Initializing methods...");
                return true;
            }

            if (ShowWarning) Log.LogWarning($"[SoftDependency] Unable to find plugin '{Name}' - Skipping its initialization!");
            return false;
        }

        #endregion
    }
}
