using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using System.Text;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Utils
{
    public class ConfigSettings
    {
        public static ConfigEntry<bool> AlwaysShow;
        public static ConfigEntry<bool> AllowOutside;
        public static ConfigEntry<bool> AllowInside;
        public static ConfigEntry<bool> AllCaps;
        public static ConfigEntry<float> DisplayDuration;
        public static ConfigEntry<bool> ShowLine;
        public static ConfigEntry<string> LineColor;

        public static ConfigEntry<bool> ShowShipLoot;
        public static ConfigEntry<string> ShipLootColor;
        public static ConfigEntry<string> ShipLootFormat;

        public static ConfigEntry<bool> ShowQuota;
        public static ConfigEntry<string> QuotaColor;
        public static ConfigEntry<string> QuotaFormat;

        public static ConfigEntry<bool> ShowDays;
        public static ConfigEntry<string> DaysColor;
        public static ConfigEntry<string> DaysFormat;

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

            string category = "General";
            AlwaysShow = config.Bind<bool>(category, "Always Show", false, "Should the hud elements be decoupled from the scanner? (Meaning it will always be shown on screen)");
            AllowOutside = config.Bind<bool>(category, "Allow Outside", false, "Should the scanner hud be shown when scanning outside the ship?");
            AllowInside = config.Bind<bool>(category, "Allow Inside Dungeon", false, "Should the scanner hud be shown when scanning inside the dungeon?");
            AllCaps = config.Bind<bool>(category, "All Caps", false, "Should text be in all caps?");
            DisplayDuration = config.Bind<float>(category, "Display Duration", 5f, "How long in seconds should the items stay on screen. (This is ignored if Always Show is true)");
            ShowLine = config.Bind<bool>(category, "Show Line", true, "Shows the line element");
            LineColor = config.Bind<string>(category, "Line Color", "2D5122", "Line color (hex code)");

            category = "Line #1";
            ShowShipLoot = config.Bind<bool>(category, "Show", true, $"Shows {category} on the hud.");
            ShipLootColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            ShipLootFormat = config.Bind<string>(category, "Format", "Ship: $%ShipLootValue%(%ShipLootCount%)/$%MoonLootValue%(%MoonLootCount%)", $"{category} text format.\n\n{sb}");

            category = "Line #2";
            ShowQuota = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            QuotaColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            QuotaFormat = config.Bind<string>(category, "Format", "Quota: $%FulfilledValue%/$%QuotaValue% - $%ExpectedProfit%(%CompanyRate%%)", $"{category} text format.\n\n{sb}");

            category = "Line #3";
            ShowDays = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            DaysColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            DaysFormat = config.Bind<string>(category, "Format", "Deadline: %Deadline% - %DayNumberHuman% day", $"{category} text format.\n\n{sb}");

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
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(DisplayDuration, new FloatSliderOptions { Min = 1f, Max = 60f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowLine, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineColor, false));

            //ShipLoot
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowShipLoot, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(ShipLootColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(ShipLootFormat, false));

            //Quota
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowQuota, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(QuotaColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(QuotaFormat, false));

            //Days Left
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowDays, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DaysColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DaysFormat, false));
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
