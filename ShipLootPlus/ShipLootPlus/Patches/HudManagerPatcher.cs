using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using Figgle;
using GameNetcodeStuff;
using HarmonyLib;
using ShipLootPlus.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static ShipLootPlus.ShipLootPlus;
using Object = UnityEngine.Object;

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
            UiHelper.RefreshElementValues();
            if (ConfigSettings.AlwaysShow.Value) return;

            if (!ConfigSettings.AllowOutside.Value)
            {
#if DEBUG
                Log.LogWarning("!AllowOutside");
#endif
                if (!StartOfRound.Instance.inShipPhase && !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
                    return;
            }

            if (!ConfigSettings.AlwaysShow.Value)
            {
#if DEBUG
                Log.LogWarning("!AlwaysShow");
#endif
                UiHelper.timeLeftDisplay = ConfigSettings.DisplayDuration.Value;
                GameNetworkManager.Instance.StartCoroutine(UiHelper.DisplayDataCoroutine());
                if (UiHelper.UiElementList.Any(e => !e.gameOjbect.activeSelf))
                {
#if DEBUG
                    Log.LogWarning("!activeSelf");
#endif
                }
            }
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
