using HarmonyLib;
using ShipLootPlus.Utils;
using UnityEngine.InputSystem;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class HudManagerPatcher
    {
        /// <summary>
        /// Method called when scanning
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="context"></param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.PingScan_performed))]
        private static void OnScan(HUDManager __instance, InputAction.CallbackContext context)
        {
            if (GameNetworkManager.Instance.localPlayerController == null) return;
            if (!context.performed || !__instance.CanPlayerScan() || __instance.playerPingingScan > -0.5f) return;
#if DEBUG
            Log.LogWarning("in OnScan");
#endif
            UiHelper.timeLeftDisplay = ConfigSettings.DisplayDuration.Value;
            UiHelper.RefreshElementValues();
            UiHelper.TryToggleUi(true);
        }

        /// <summary>
        /// Create the UI elements when HUDManager starts
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HUDManager), "Start")]
        private static void PostfixStart(HUDManager __instance)
        {
            UiHelper.CreateUiElements();
            if (GameNetworkManager.Instance.localPlayerController == null) return;
        }

        /// <summary>
        /// Toggle UI when state changes happen (moving between zones)
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(HUDManager), "Update")]
        private static void Postfix(HUDManager __instance)
        {
            if (GameNetworkManager.Instance.localPlayerController == null) return;
            UiHelper.TryToggleUi();
        }

        /// <summary>
        /// Refresh data values when new scrap is added to the ship
        /// </summary>
        [HarmonyPatch(typeof(HUDManager), nameof(DisplayNewScrapFound))]
        [HarmonyPostfix]
        private static void DisplayNewScrapFound()
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data values when the round days left is updated
        /// </summary>
        [HarmonyPatch(typeof(HUDManager), nameof(DisplayDaysLeft))]
        [HarmonyPostfix]
        private static void DisplayDaysLeft()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
