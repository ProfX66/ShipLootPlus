using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class TimeOfDayPatcher
    {
        /// <summary>
        /// Update values when profit/quota is changed
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TimeOfDay), nameof(UpdateProfitQuotaCurrentTime))]
        private static void UpdateProfitQuotaCurrentTime()
        {
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[UpdateProfitQuotaCurrentTime] Quota and time has been updated");
            if (GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }
    }
}
