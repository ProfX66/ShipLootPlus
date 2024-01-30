using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class RoundManagerPatcher
    {
        /// <summary>
        /// Refresh data when the level has generated its scrap
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(SpawnScrapInLevel))]
        private static void SpawnScrapInLevel(RoundManager __instance)
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data when the level has generated its scrap
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(SyncScrapValuesClientRpc))]
        private static void SyncScrapValuesClientRpc()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }
        
    }
}
