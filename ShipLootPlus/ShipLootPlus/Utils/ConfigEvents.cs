using Figgle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Utils
{
    public static class ConfigEvents
    {
        /// <summary>
        /// Initialize all config changed events
        /// </summary>
        public static void Initialize()
        {
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[ConfigEvents.Initialize] Initializing all 'SettingChanged' events");
            ConfigSettings.AlwaysShow.SettingChanged += ToggleUi_SettingChanged;
            ConfigSettings.AllCaps.SettingChanged += RefreshUi_SettingChanged;
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
            ConfigSettings.SelectedFont.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.CharacterSpacing.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.FontSize.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.LineAlpha.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.PosX.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.PosY.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.ScaleX.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.ScaleY.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.Rotation.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.TextAlignment.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.TextAlpha.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.WordSpacing.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.WidthAppend.SettingChanged += RedrawRequired_SettingChanged;

            ConfigSettings.DeadlineUseColors.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.DeadlineReplaceZero.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.DeadlineLastDay.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.DeadlineTwoColor.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.DeadlineOneColor.SettingChanged += RedrawRequired_SettingChanged;
            ConfigSettings.DeadlineZeroColor.SettingChanged += RedrawRequired_SettingChanged;

            ConfigSettings.MoonShowFullName.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.MoonReplaceCompany.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.MoonCompanyReplacement.SettingChanged += RefreshUi_SettingChanged;

            ConfigSettings.WeatherNoneReplacement.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherUseColors.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorNone.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorDustClouds.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorRainy.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorStormy.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorFoggy.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorFlooded.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorEclipsed.SettingChanged += RefreshUi_SettingChanged;
            ConfigSettings.WeatherColorHell.SettingChanged += RefreshUi_SettingChanged;

            ConfigSettings.DebugMode.SettingChanged += DebugMode_SettingChanged;
            ConfigSettings.DisableRpcHooks.SettingChanged += DisableRpcHooks_SettingChanged;
        }

        /// <summary>
        /// When a setting is changed that requires a UI refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RedrawRequired_SettingChanged(object sender, System.EventArgs e)
        {
            if (UiHelper.ContainerObject == null) return;
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[ConfigEvents] UI setting requiring a redraw was changed");
            UiHelper.ResetUiElements();
        }

        /// <summary>
        /// Enable/Disable the UI elements based on AlwaysShow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ToggleUi_SettingChanged(object sender, System.EventArgs e)
        {
            if (UiHelper.ContainerObject == null) return;
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[ConfigEvents] UI setting which enables or disables the display has changed");
            UiHelper.ContainerObject.SetActive(ConfigSettings.AlwaysShow.Value);
        }

        /// <summary>
        /// Refresh the UI data when AllCaps is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RefreshUi_SettingChanged(object sender, System.EventArgs e)
        {
            if (UiHelper.ContainerObject == null) return;
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[ConfigEvents] UI setting which requires a full data refresh has changed");
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh the UI data when AllCaps is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DebugMode_SettingChanged(object sender, System.EventArgs e)
        {
            if (ConfigSettings.DebugMode.Value) Log.LogWarning($"Enabling debug mode (expect performance drop!)\n{FiggleFonts.Doom.Render("Debug Enabled!")}");
            else Log.LogWarning($"Disabling debug mode\n{FiggleFonts.Doom.Render("Debug Disabled!")}");
        }

        /// <summary>
        /// Refresh the UI data when AllCaps is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DisableRpcHooks_SettingChanged(object sender, System.EventArgs e)
        {
            if (ConfigSettings.DisableRpcHooks.Value) Log.LogWarning("Disabling RPC Hooks - Data will now refresh slower!");
            else Log.LogWarning("Enabling RPC Hooks - Data will now refresh normally!");
        }
    }
}
