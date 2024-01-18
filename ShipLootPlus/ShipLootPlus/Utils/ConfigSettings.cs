using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Utils
{
    public class ConfigSettings
    {
        public static ConfigEntry<bool> AllowOutside;
        public static ConfigEntry<bool> AllCaps;
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

            #region General

            string category = "General";
            AllowOutside = config.Bind<bool>(category, "Allow Outside", false, "Should the scanner hud be shown when scanning outside the ship?");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllowOutside, false));

            AllCaps = config.Bind<bool>(category, "All Caps", false, "Should text be in all caps?");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllCaps, false));

            LineColor = config.Bind<string>(category, "Line Color", "2D5122", "Line color (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineColor, true));

            #endregion

            #region ShipLoot

            category = "Ship Loot";
            ShowShipLoot = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowShipLoot, true));

            ShipLootPosition = config.Bind<ElementLocation>(category, "Position", ElementLocation.Top, $"This is where the {category} information is positioned on the HUD (IMPORTANT! Must not be the same as the other two)");
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<ElementLocation>(ShipLootPosition, new EnumDropDownOptions { RequiresRestart = true }));

            ShipLootColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(ShipLootColor, true));

            ShipLootFormat = config.Bind<string>(category, "Format", "Ship: %LootValue%", $"{category} text format (%LootValue% = Loot value on the ship)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(ShipLootFormat, true));

            #endregion

            #region Quota

            category = "Quota";
            ShowQuota = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowQuota, true));

            QuotaPosition = config.Bind<ElementLocation>(category, "Position", ElementLocation.Middle, $"This is where the {category} information is positioned on the HUD (IMPORTANT! Must not be the same as the other two)");
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<ElementLocation>(QuotaPosition, new EnumDropDownOptions { RequiresRestart = true }));

            QuotaColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(QuotaColor, true));

            QuotaFormat = config.Bind<string>(category, "Format", "Quota: %FulfilledValue%/%QuotaValue%", $"{category} text format (%FulfilledValue% = Value turned in for the quota. %QuotaValue% = Full value of the current quota)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(QuotaFormat, true));

            #endregion

            #region Days Left

            category = "Days Left";
            ShowDays = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowDays, true));

            DaysPosition = config.Bind<ElementLocation>(category, "Position", ElementLocation.Bottom, $"This is where the {category} information is positioned on the HUD (IMPORTANT! Must not be the same as the other two)");
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<ElementLocation>(DaysPosition, new EnumDropDownOptions { RequiresRestart = true }));

            DaysColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color (hex code)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DaysColor, true));

            DaysFormat = config.Bind<string>(category, "Format", "Days Left: %DaysLeft%", $"{category} text format (%DaysLeft% = Days left to turn in quota)");
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DaysFormat, true));

            #endregion

        }
    }

    
}
