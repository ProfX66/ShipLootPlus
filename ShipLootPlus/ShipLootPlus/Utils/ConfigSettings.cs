using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Utils
{
    public class ConfigSettings
    {
        public static ConfigEntry<bool> AlwaysShow;
        public static ConfigEntry<bool> AllowOutside;
        public static ConfigEntry<bool> AllowInside;
        public static ConfigEntry<bool> AllCaps;
        public static ConfigEntry<int> ShortCharLength;
        public static ConfigEntry<bool> ShowLine;
        public static ConfigEntry<string> LineColor;

        public static ConfigEntry<float> DisplayDuration;
        public static ConfigEntry<bool> DisplayDurationReset;
        public static ConfigEntry<bool> RefreshOnScan;

        public static ConfigEntry<bool> ShowLineOne;
        public static ConfigEntry<string> LineOneColor;
        public static ConfigEntry<string> LineOneFormat;

        public static ConfigEntry<bool> ShowLineTwo;
        public static ConfigEntry<string> LineTwoColor;
        public static ConfigEntry<string> LineTwoFormat;

        public static ConfigEntry<bool> ShowLineThree;
        public static ConfigEntry<string> LineThreeColor;
        public static ConfigEntry<string> LineThreeFormat;

        /// <summary>
        /// Create BepInEx config items
        /// </summary>
        /// <param name="config"></param>
        /// <param name="description"></param>
        public static void Initialize(ConfigFile config, string description)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Data Points:");

            foreach (ReplacementData dataPoint in UiHelper.DataPoints)
            {
                sb.AppendLine(dataPoint.Pattern);
                sb.AppendLine(dataPoint.Description);
                sb.AppendLine();
            }

            StringBuilder sbShort = new StringBuilder();
            foreach (ReplacementData dataPoint in UiHelper.DataPoints.Where(e => Regex.IsMatch(e.Name, "Short", RegexOptions.IgnoreCase)))
            {
                sbShort.AppendLine(dataPoint.Pattern);
                sbShort.AppendLine(dataPoint.Description);
                sbShort.AppendLine();
            }

            //TODO: Add show in spectator option

            string category = "General";
            AlwaysShow = config.Bind<bool>(category, "Always Show", false, "Should the hud elements be decoupled from the scanner? (Meaning it will always be shown on screen)");
            AllowOutside = config.Bind<bool>(category, "Allow Outside", false, "Should the scanner hud be shown when scanning outside the ship?");
            AllowInside = config.Bind<bool>(category, "Allow Inside Dungeon", false, "Should the scanner hud be shown when scanning inside the dungeon?");
            AllCaps = config.Bind<bool>(category, "All Caps", false, "Should text be in all caps?");
            ShortCharLength = config.Bind<int>(category, "Short Character Count", 3, $"How many characters to show for the following data points:\n\n{sbShort}");
            ShowLine = config.Bind<bool>(category, "Show Line", true, "Shows the line element");
            LineColor = config.Bind<string>(category, "Line Color", "2D5122", "Line color (hex code)");

            category = "On Scan";
            DisplayDuration = config.Bind<float>(category, "Display Duration", 5f, "How long in seconds should the items stay on screen. (This is ignored if Always Show is true)");
            DisplayDurationReset = config.Bind<bool>(category, "Reset Duration Timer On Scan", false, "Should the duration timer get reset if you scan?");
            RefreshOnScan = config.Bind<bool>(category, "Refresh Data On Scan", false, "Should a data refresh be forced when scanning?\n\nAll data is kept updated when events are triggered (player grabs an item, items get moved into the ship, etc.) so this isn't required.\n\n<b>IMPORTANT</B>: This could cause issues with any mod that makes the scanner always on");

            category = "Line #1";
            ShowLineOne = config.Bind<bool>(category, "Show", true, $"Shows {category} on the hud.");
            LineOneColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LineOneFormat = config.Bind<string>(category, "Format", "Ship: $%ShipLootValue%(%ShipLootCount%)/$%MoonLootValue%(%MoonLootCount%) <i>[%MoonShortName%:%Weather%]</i>", $"{category} text format.\n\n{sb}");

            category = "Line #2";
            ShowLineTwo = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            LineTwoColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LineTwoFormat = config.Bind<string>(category, "Format", "Quota: $%FulfilledValue%/$%QuotaValue% - Prof: $%ExpectedProfit%(%CompanyRate%%)", $"{category} text format.\n\n{sb}");

            category = "Line #3";
            ShowLineThree = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            LineThreeColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LineThreeFormat = config.Bind<string>(category, "Format", "Deadline: %Deadline% - %DayNumberHuman% day", $"{category} text format.\n\n{sb}");

            try
            {
                if (AssemblyExists("LethalConfig")) SetupLethalConfig(description);
            }
            catch { Log.LogWarning("LethalSettings was not found - Skipping its initialization..."); }
        }

        /// <summary>
        /// Create LethalConfig items
        /// </summary>
        /// <param name="description"></param>
        private static void SetupLethalConfig(string description)
        {
            //TODO: Make a reset all function

            //General
            LethalConfigManager.SetModDescription(description);
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AlwaysShow, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllowOutside, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllowInside, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllCaps, false));
            LethalConfigManager.AddConfigItem(new IntSliderConfigItem(ShortCharLength, new IntSliderOptions { Min = 1, Max = 30, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowLine, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineColor, false));

            //On Scan
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(DisplayDuration, new FloatSliderOptions { Min = 1f, Max = 60f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(DisplayDurationReset, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(RefreshOnScan, false));

            //ShipLoot
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowLineOne, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineOneColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineOneFormat, false));

            //Quota
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowLineTwo, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineTwoColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineTwoFormat, false));

            //Days Left
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowLineThree, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineThreeColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineThreeFormat, false));
        }

        #region Future

        /*

        /// <summary>
        /// Element positions
        /// </summary>
        public enum ElementLocation
        {
            Top,
            Middle,
            Bottom
        }

        public static ConfigEntry<ElementLocation> DaysPosition;

        DaysPosition = config.Bind<ElementLocation>(category, "Position", ElementLocation.Bottom, $"This is where the {category} information is positioned on the HUD. (IMPORTANT! Must not be the same as the other two)");

        LethalConfigManager.AddConfigItem(new GenericButtonConfigItem(category, "Reset all defaults", "This button will reset all config defaults.", "Reset Defaults", () =>
        {
            // Code you want to run when the button is clicked goes here.

        }));

        LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<ElementLocation>(DaysPosition, new EnumDropDownOptions { RequiresRestart = true }));
        */

        #endregion
    }
}
