using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class RoundManagerPatcher
    {
        /// <summary>
        /// Refresh data when the level has loaded all objects
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(RefreshEnemyVents))]
        private static void RefreshEnemyVents()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }
    }
}
