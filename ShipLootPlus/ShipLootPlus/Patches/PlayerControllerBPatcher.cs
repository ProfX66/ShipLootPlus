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
        /// Refresh data when a scrap object being grabbed RPC is recieved
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="grabbedObject"></param>
        [HarmonyPatch(typeof(PlayerControllerB), nameof(GrabObjectClientRpc))]
        [HarmonyPostfix]
        private static void GrabObjectClientRpc(PlayerControllerB __instance, NetworkObjectReference grabbedObject)
        {
            if (__instance != null)
            {
                if (__instance.currentlyHeldObjectServer.itemProperties.isScrap && !UiHelper.IsRefreshing)
                {
                    GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
                }
            }
        }
    }
}
