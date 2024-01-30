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
            if (__instance.itemProperties.isScrap && !UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }
    }
}
