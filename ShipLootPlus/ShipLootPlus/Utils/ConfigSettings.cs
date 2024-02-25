using BepInEx.Configuration;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Utils
{
    public class ConfigSettings
    {
        public static ConfigEntry<bool> AlwaysShow;
        public static ConfigEntry<bool> AllowOutside;
        public static ConfigEntry<bool> AllowInside;

        public static ConfigEntry<float> PosX;
        public static ConfigEntry<float> PosY;
        public static ConfigEntry<float> ScaleX;
        public static ConfigEntry<float> ScaleY;
        public static ConfigEntry<float> Rotation;
        public static ConfigEntry<float> WidthAppend;

        public static ConfigEntry<float> DisplayDuration;
        public static ConfigEntry<bool> DisplayDurationReset;
        public static ConfigEntry<bool> RefreshOnScan;

        public static ConfigEntry<bool> ShowLine;
        public static ConfigEntry<string> LineColor;
        public static ConfigEntry<float> LineAlpha;

        public static ConfigEntry<bool> AllCaps;
        public static ConfigEntry<float> FontSize;
        public static ConfigEntry<float> CharacterSpacing;
        public static ConfigEntry<float> WordSpacing;
        public static ConfigEntry<Fonts.FontList> SelectedFont;
        public static ConfigEntry<TextAlignmentOptions> TextAlignment;
        public static ConfigEntry<float> TextAlpha;

        public static ConfigEntry<bool> ShowLineOne;
        public static ConfigEntry<string> LineOneColor;
        public static ConfigEntry<string> LineOneFormat;

        public static ConfigEntry<bool> ShowLineTwo;
        public static ConfigEntry<string> LineTwoColor;
        public static ConfigEntry<string> LineTwoFormat;

        public static ConfigEntry<bool> ShowLineThree;
        public static ConfigEntry<string> LineThreeColor;
        public static ConfigEntry<string> LineThreeFormat;

        public static ConfigEntry<bool> DeadlineUseColors;
        public static ConfigEntry<bool> DeadlineReplaceZero;
        public static ConfigEntry<string> DeadlineLastDay;
        public static ConfigEntry<string> DeadlineTwoColor;
        public static ConfigEntry<string> DeadlineOneColor;
        public static ConfigEntry<string> DeadlineZeroColor;

        public static ConfigEntry<bool> MoonShowFullName;
        public static ConfigEntry<bool> MoonReplaceCompany;
        public static ConfigEntry<string> MoonCompanyReplacement;

        public static ConfigEntry<string> WeatherNoneReplacement;
        public static ConfigEntry<bool> WeatherUseColors;
        public static ConfigEntry<string> WeatherColorDustClouds;
        public static ConfigEntry<string> WeatherColorEclipsed;
        public static ConfigEntry<string> WeatherColorFlooded;
        public static ConfigEntry<string> WeatherColorFoggy;
        public static ConfigEntry<string> WeatherColorNone;
        public static ConfigEntry<string> WeatherColorRainy;
        public static ConfigEntry<string> WeatherColorStormy;
        public static ConfigEntry<string> WeatherColorHell;

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

            category = "On Scan";
            DisplayDuration = config.Bind<float>(category, "Display Duration", 5f, "How long in seconds should the items stay on screen. (This is ignored if Always Show is true)");
            DisplayDurationReset = config.Bind<bool>(category, "Reset Duration Timer On Scan", false, "Should the duration timer get reset if you scan?");
            RefreshOnScan = config.Bind<bool>(category, "Refresh Data On Scan", false, "Should a data refresh be forced when scanning?\n\nAll data is kept updated when events are triggered (player grabs an item, items get moved into the ship, etc.) so this isn't required.\n\n<b>IMPORTANT</B>: This could cause issues with any mod that makes the scanner always on");

            category = "Layout";
            PosX = config.Bind<float>(category, "Position: X (Left/Right)", 115f, "The X position of the UI element group");
            PosY = config.Bind<float>(category, "Position: Y (Up/Down)", -169f, "The Y position of the UI element group");
            ScaleX = config.Bind<float>(category, "Scale: X (Left/Right)", 0.6f, "The X scale of the UI element group");
            ScaleY = config.Bind<float>(category, "Scale: Y (Up/Down)", 0.6f, "The Y scale of the UI element group");
            Rotation = config.Bind<float>(category, "Rotation: Z (Tilt)", 356f, "This changes how much the UI element group is rotated on the screen");
            WidthAppend = config.Bind<float>(category, "Text Field Width Offset", 0f, "This value allows you to offset the text field width if you want to show more or less characters on screen");

            category = "Line Graphic";
            ShowLine = config.Bind<bool>(category, "Show Line", true, "Shows the line element");
            LineColor = config.Bind<string>(category, "Line Color", "2D5122", "Line color (hex code)");
            LineAlpha = config.Bind<float>(category, "Transparency", 0.75f, "Make the line element more or less transparent");

            category = "Font Settings";
            AllCaps = config.Bind<bool>(category, "All Caps", false, "Should text be in all caps?");
            SelectedFont = config.Bind(category, "Font", Fonts.FontList.Vanilla, "Font to use for the UI elements");
            FontSize = config.Bind<float>(category, "Size", 19f, "Adjust the font size");
            CharacterSpacing = config.Bind<float>(category, "Character Spacing", -6f, "Adjust the spacing between characters");
            WordSpacing = config.Bind<float>(category, "Word Spacing", -20f, "Adjust the spacing between words");
            TextAlignment = config.Bind(category, "Text Alignment", TextAlignmentOptions.TopLeft, "Change the default text alignment for all elements\n\n<b>**IMPORTANT**</b> The elements are built to stay Top Left aligned, changing this may produce unwanted outcomes");
            TextAlpha = config.Bind<float>(category, "Transparency", 0.95f, "Make the text elements more or less transparent");

            category = "Line #1";
            ShowLineOne = config.Bind<bool>(category, "Show", true, $"Shows {category} on the hud.");
            LineOneColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LineOneFormat = config.Bind<string>(category, "Format", "Ship: $%ShipLootValue%(%ShipLootCount%) / $%MoonLootValue%(%MoonLootCount%) <i>[%MoonName%:%Weather%]</i>", $"{category} text format.\n\n{sb}");

            category = "Line #2";
            ShowLineTwo = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            LineTwoColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LineTwoFormat = config.Bind<string>(category, "Format", "Quota: $%FulfilledValue% / $%QuotaValue% - Profit: $%ExpectedProfit%(%CompanyRate%%)", $"{category} text format.\n\n{sb}");

            category = "Line #3";
            ShowLineThree = config.Bind<bool>(category, "Show", true, $"Shows the {category} on the scan hud.");
            LineThreeColor = config.Bind<string>(category, "Color", "19D56C", $"{category} text color. (hex code)");
            LineThreeFormat = config.Bind<string>(category, "Format", "Deadline: %Deadline% - %DayNumberHuman% day", $"{category} text format.\n\n{sb}");

            category = "DataPoint: Deadline";
            DeadlineUseColors = config.Bind<bool>(category, "Use Colors", false, "Enables color for the deadline number");
            DeadlineReplaceZero = config.Bind<bool>(category, "Replace Zero", true, "Replace the number 0 with the custom text below, otherwise leave it as a number");
            DeadlineLastDay = config.Bind<string>(category, "Zero Replacement", "<b>NOW!</b>", $"Text to replace the number Zero if 'Replace Zero' is enabled");
            DeadlineTwoColor = config.Bind<string>(category, "Color: 2+ days", "00FF00", "Color for when the deadline has two or more days remaining");
            DeadlineOneColor = config.Bind<string>(category, "Color: 1 day", "FFA500", "Color for when the deadline has one day remaining");
            DeadlineZeroColor = config.Bind<string>(category, "Color: Zero days", "FF0000", "Color for when the deadline is due");

            category = "DataPoint: MoonName";
            MoonShowFullName = config.Bind<bool>(category, "Show Full Name", false, "Show the full moon name (do not remove any leading numbers)");
            MoonReplaceCompany = config.Bind<bool>(category, "Replace Company Name", true, "Replace the name used for the company moon");
            MoonCompanyReplacement = config.Bind<string>(category, "Company Name Replacement", "Company Building", "Text to replace 'Gordion' if 'Replace Company Name' is enabled");

            category = "DataPoint: Weather";
            WeatherNoneReplacement = config.Bind<string>(category, "Clear Weather Text", "Clear", "Text to use instead of 'None' for when the weather is clear (set to blank if you want it to show None)");
            WeatherUseColors = config.Bind<bool>(category, "Use Colors", false, "Enables color for each weather type");
            WeatherColorNone = config.Bind<string>(category, "Color: Clear/None", "69FF6B", "Color for Clear/None weather");
            WeatherColorDustClouds = config.Bind<string>(category, "Color: DustClouds", "B56C4C", "Color for DustClouds weather");
            WeatherColorRainy = config.Bind<string>(category, "Color: Rainy", "FFFF00", "Color for Rainy weather");
            WeatherColorStormy = config.Bind<string>(category, "Color: Stormy", "FF7700", "Color for Stormy weather");
            WeatherColorFoggy = config.Bind<string>(category, "Color: Foggy", "666666", "Color for Foggy weather");
            WeatherColorFlooded = config.Bind<string>(category, "Color: Flooded", "FF0000", "Color for Flooded weather");
            WeatherColorEclipsed = config.Bind<string>(category, "Color: Eclipsed", "BA0B0B", "Color for Eclipsed weather");
            WeatherColorHell = config.Bind<string>(category, "Color: Hell", "AA0000", "Color for Hell weather (from the mod 'HellWeather')");

            // Config conversions #ba0b0b
            int oldShort = 3;
            bool doSave = false;
            PropertyInfo orphanedEntriesProp = config.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            Dictionary<ConfigDefinition, string> orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(config, null);
            if (orphanedEntries.Count > 0)
            {
                foreach (var v in orphanedEntries)
                {
                    switch (v.Key.Key)
                    {
                        case "Short Character Count":
                            oldShort = (int)ConvertToType(typeof(int), v.Value);
                            Log.LogMessage($"[ConfigConversion] Converting old '{v.Key.Key}' with value '{v.Value}' to new config item");
                            //ShortCharLength.Value = (int)ConvertToType(typeof(int), v.Value);
                            break;
                        case "Show Line":
                            Log.LogMessage($"[ConfigConversion] Converting old '{v.Key.Key}' with value '{v.Value}' to new config item");
                            ShowLine.Value = (bool)ConvertToType(typeof(bool), v.Value);
                            break;
                        case "Line Color":
                            Log.LogMessage($"[ConfigConversion] Converting old '{v.Key.Key}' with value '{v.Value}' to new config item");
                            LineColor.Value = v.Value;
                            break;
                        case "All Caps":
                            Log.LogMessage($"[ConfigConversion] Converting old '{v.Key.Key}' with value '{v.Value}' to new config item");
                            AllCaps.Value = (bool)ConvertToType(typeof(bool), v.Value);
                            break;
                        case "Display Duration":
                            Log.LogMessage($"[ConfigConversion] Converting old '{v.Key.Key}' with value '{v.Value}' to new config item");
                            DisplayDuration.Value = (float)ConvertToType(typeof(float), v.Value);
                            break;
                    }
                }

                orphanedEntries.Clear();
                doSave = true;
            }

            if (ConvertOldFormat("%DeadlineWithColors%", "%Deadline%"))
            {
                Log.LogMessage("[ConfigConversion] Enabling 'Use Colors' on Deadline data point as user wanted to use colors before");
                DeadlineUseColors.Value = true;
                doSave = true;
            }

            if (ConvertOldFormat("%MoonLongName%", "%MoonName%")) doSave = true;
            if (ConvertOldFormat("%WeatherShort%", $"%Weather:{oldShort}%")) doSave = true;
            if (ConvertOldFormat("%MoonShortName%", $"%MoonName:{oldShort}%")) doSave = true;

            if (doSave)
            {
                Log.LogMessage("[ConfigConversion] Saving config changes...");
                config.Save();
            }

            try
            {
                if (PluginExists("ainavt.lc.lethalconfig")) SetupLethalConfig(description);
            }
            catch { }
        }

        /// <summary>
        /// Converts a datapoint format to a new version
        /// </summary>
        /// <param name="oldPattern"></param>
        /// <param name="newPattern"></param>
        /// <param name="raw"></param>
        /// <returns>true if anything changed</returns>
        public static bool ConvertOldFormat(string oldPattern, string newPattern, bool raw = false)
        {
            bool ret = false;
            string pattern = Regex.Escape(oldPattern);
            if (raw) pattern = oldPattern;

            if (Regex.IsMatch(LineOneFormat.Value, pattern))
            {
                Log.LogMessage($"[ConfigConversion] Converting old datapoint '{oldPattern}' with new datapoint '{newPattern}' on Line #1 format");
                LineOneFormat.Value = Regex.Replace(LineOneFormat.Value, pattern, newPattern, RegexOptions.IgnoreCase);
                ret = true;
            }

            if (Regex.IsMatch(LineTwoFormat.Value, pattern))
            {
                Log.LogMessage($"[ConfigConversion] Converting old datapoint '{oldPattern}' with new datapoint '{newPattern}' on Line #2 format");
                LineTwoFormat.Value = Regex.Replace(LineTwoFormat.Value, pattern, newPattern, RegexOptions.IgnoreCase);
                ret = true;
            }

            if (Regex.IsMatch(LineThreeFormat.Value, pattern))
            {
                Log.LogMessage($"[ConfigConversion] Converting old datapoint '{oldPattern}' with new datapoint '{newPattern}' on Line #3 format");
                LineThreeFormat.Value = Regex.Replace(LineThreeFormat.Value, pattern, newPattern, RegexOptions.IgnoreCase);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// Converts the passed value to the passed type and returns it as a castable generic object
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertToType(Type targetType, string value)
        {
            var tryParseMethod = targetType.GetMethod("TryParse", new[] { typeof(string), targetType.MakeByRefType() });

            if (tryParseMethod != null)
            {
                var parameters = new object[] { value, null };
                var success = (bool)tryParseMethod.Invoke(null, parameters);

                if (success)
                {
                    return parameters[1];
                }
                else
                {
                    Log.LogWarning($"Failed to convert '{value}' to type {targetType.Name}");
                }
            }
            else
            {
                Log.LogWarning($"Type {targetType.Name} does not support TryParse method");
            }

            return value;
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

            //On Scan
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(DisplayDuration, new FloatSliderOptions { Min = 1f, Max = 60f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(DisplayDurationReset, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(RefreshOnScan, false));

            //Layout
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(PosX, new FloatSliderOptions { Min = -469f, Max = 333f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(PosY, new FloatSliderOptions { Min = -280f, Max = 190f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(ScaleX, new FloatSliderOptions { Min = 0.3f, Max = 1.2f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(ScaleY, new FloatSliderOptions { Min = 0.3f, Max = 1.2f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(Rotation, new FloatSliderOptions { Min = 0f, Max = 360f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(WidthAppend, new FloatSliderOptions { Min = -300f, Max = 2000f, RequiresRestart = false }));

            //Line Graphic
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(ShowLine, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(LineColor, false));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(LineAlpha, new FloatSliderOptions { Min = 0f, Max = 1f, RequiresRestart = false }));

            //Font
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(AllCaps, false));
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<Fonts.FontList>(SelectedFont, false));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(FontSize, new FloatSliderOptions { Min = 1f, Max = 100f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(CharacterSpacing, new FloatSliderOptions { Min = -20f, Max = 10f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(WordSpacing, new FloatSliderOptions { Min = -70f, Max = 50f, RequiresRestart = false }));
            LethalConfigManager.AddConfigItem(new EnumDropDownConfigItem<TextAlignmentOptions>(TextAlignment, false));
            LethalConfigManager.AddConfigItem(new FloatSliderConfigItem(TextAlpha, new FloatSliderOptions { Min = 0f, Max = 1f, RequiresRestart = false }));

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

            //DataPoint: Deadline
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(DeadlineUseColors, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(DeadlineReplaceZero, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DeadlineLastDay, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DeadlineTwoColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DeadlineOneColor, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(DeadlineZeroColor, false));

            //DataPoint: MoonName
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(MoonShowFullName, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(MoonReplaceCompany, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(MoonCompanyReplacement, false));

            //DataPoint: Weather
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherNoneReplacement, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(WeatherUseColors, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorNone, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorDustClouds, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorRainy, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorStormy, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorFoggy, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorFlooded, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorEclipsed, false));
            LethalConfigManager.AddConfigItem(new TextInputFieldConfigItem(WeatherColorHell, false));
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
