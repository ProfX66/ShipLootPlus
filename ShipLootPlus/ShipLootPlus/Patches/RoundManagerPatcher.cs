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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[SpawnScrapInLevel] Scrap has generated on the moon");
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data when the level has generated its scrap
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(SyncScrapValuesClientRpc))]
        private static void SyncScrapValuesClientRpc()
        {
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[SyncScrapValuesClientRpc] Received ClientRpc to sync scrap values");
            if (!ConfigSettings.DisableRpcHooks.Value && !UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }
        
    }
}
