using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Utils
{
    public class ConfigSettings
    {
        public static ConfigEntry<bool> AlwaysShow;
        public static ConfigEntry<bool> AllowOutside;
        public static ConfigEntry<bool> AllCaps;
        public static ConfigEntry<float> DisplayDuration;
        public static ConfigEntry<bool> ShowLine;
        public static ConfigEntry<string> LineColor;

        public static ConfigEntry<bool> ShowShipLoot;
        public static ConfigEntry<ElementLocation> ShipLootPosition;
        public static ConfigEntry<string> ShipLootColor;
        public static ConfigEntry<string> ShipLootFormat;

        public static ConfigEntry<bool> ShowQuota;
        public static ConfigEntry<ElementLocation> QuotaPosition;
        public static ConfigEntry<string> QuotaColor;
        public static ConfigEntry<string> QuotaFormat;

        public static ConfigEntry<bool> ShowDays;
        public static ConfigEntry<ElementLocation> DaysPosition;
        public static ConfigEntry<string> DaysColor;
        public static ConfigEntry<string> DaysFormat;

        public static void Initialize(ConfigFile config, string description)
        {
            LethalConfigManager.SetModDescription(description);

            #region Config Items

            #endregion

            #region Lethal Config Elements

            string category = "General";
            //TODO: Make a reset all function
            /*LethalConfigManager.AddConfigItem(new GenericButtonConfigItem(category, "Reset all defaults", "This button will reset all config defaults.", "Reset Defaults", () =>
            {
                // Code you want to run when the button is clicked goes here.

            }));*/

            AlwaysShow = config.Bind<bool>(category, "Always Show", false, "Should the hud elements be decoupled from the scanner? (Meaning it will always be shown on screen)");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AlwaysShow, false));

            AllowOutside = config.Bind<bool>(category, "Allow Outside", false, "Should the scanner hud be shown when scanning outside the ship?");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllowOutside, false));

            AllCaps = config.Bind<bool>(category, "All Caps", false, "Should text be in all caps?");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllCaps, false));

            DisplayDuration = config.Bind<float>(category, "Display Duration", 5f, "How long in seconds should the items stay on screen. (This is ignored if Always Show is true)");
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(DisplayDuration, new FloatSliderOptions { Min = 1f, Max = 60f, RequiresRestart = false }));

            ShowLine = config.Bind<bool>(category, "Show Line", true, "Shows the line element");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowLine, false));

            LineColor = config.Bind<string>(category, "Line Color", "2D5122", "Line color (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineColor, false));

            #endregion

            #region ShipLoot

            category = "Ship Loot";
            ShowShipLoot = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowShipLoot, false));

            ShipLootPosition = config.Bind<ElementLocation>(category, "Position", ElementLocation.Top, $"This is where the {category} information is positioned on the HUD. (IMPORTANT! Must not be the same as the other two)");
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<ElementLocation>(ShipLootPosition, new EnumDropDownOptions { RequiresRestart = true }));

            ShipLootColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(ShipLootColor, false));

            ShipLootFormat = config.Bind<string>(category, "Format", "Ship: $%ShipLootValue% (%ShipLootCount%)", $"{category} text format.\n\n%ShipLootValue% = Loot value on the ship.\n%ShipLootCount% = Scrap count.");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(ShipLootFormat, false));

            #endregion

            #region Quota

            category = "Quota";
            ShowQuota = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowQuota, false));

            QuotaPosition = config.Bind<ElementLocation>(category, "Position", ElementLocation.Middle, $"This is where the {category} information is positioned on the HUD. (IMPORTANT! Must not be the same as the other two)");
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<ElementLocation>(QuotaPosition, new EnumDropDownOptions { RequiresRestart = true }));

            QuotaColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(QuotaColor, false));

            QuotaFormat = config.Bind<string>(category, "Format", "Quota: $%FulfilledValue%/$%QuotaValue%", $"{category} text format.\n\n%FulfilledValue% = Value turned in for the quota.\n%QuotaValue% = Full value of the current quota.");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(QuotaFormat, false));

            #endregion

            #region Days Left

            category = "Days Left";
            ShowDays = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowDays, false));

            DaysPosition = config.Bind<ElementLocation>(category, "Position", ElementLocation.Bottom, $"This is where the {category} information is positioned on the HUD. (IMPORTANT! Must not be the same as the other two)");
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<ElementLocation>(DaysPosition, new EnumDropDownOptions { RequiresRestart = true }));

            DaysColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DaysColor, false));

            DaysFormat = config.Bind<string>(category, "Format", "Deadline: %DaysLeft% days", $"{category} text format.\n\n%DaysLeft% = Days left to turn in quota.");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DaysFormat, false));

            #endregion

        }
    }

    
}
