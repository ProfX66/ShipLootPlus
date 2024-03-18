using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    internal class GrabbableObjectPatcher
    {
        [HarmonyPatch(typeof(GrabbableObject), nameof(OnHitGround))]
        [HarmonyPostfix]
        private static void OnHitGround(GrabbableObject __instance)
        {
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[OnHitGround] Item was thrown and hit the ground");
            if (__instance != null && __instance.itemProperties != null && GameNetworkManager.Instance != null)
            {
                if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[OnHitGround] Required objects are not null - Continuing...");
                if (__instance.itemProperties.isScrap && !UiHelper.IsRefreshing)
                {
                    GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
                }
                else
                {
                    if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[OnHitGround] Item was not scrap or there is a current data refresh already - Skipping refresh...");
                }
            }
        }
    }
}
