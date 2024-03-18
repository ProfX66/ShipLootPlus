using GameNetcodeStuff;
using HarmonyLib;
using ShipLootPlus.Utils;
using Unity.Netcode;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class PlayerControllerBPatcher
    {
        /// <summary>
        /// Refresh data when a scrap object being grabbed RPC is received
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="grabbedObject"></param>
        [HarmonyPatch(typeof(PlayerControllerB), nameof(GrabObjectClientRpc))]
        [HarmonyPostfix]
        private static void GrabObjectClientRpc(PlayerControllerB __instance, NetworkObjectReference grabbedObject)
        {
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[GrabObjectClientRpc] Recieved a ClientRpc when an object was picked up");
            if (!ConfigSettings.DisableRpcHooks.Value && __instance != null && __instance.currentlyHeldObjectServer != null && __instance.currentlyHeldObjectServer.itemProperties != null && GameNetworkManager.Instance != null)
            {
                if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[GrabObjectClientRpc] Required objects are not null - Continuing...");
                if (__instance.currentlyHeldObjectServer.itemProperties.isScrap && !UiHelper.IsRefreshing)
                {
                    GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
                }
                else
                {
                    if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[GrabObjectClientRpc] Item was not scrap or there is a current data refresh already - Skipping refresh...");
                }
            }
        }
    }
}
