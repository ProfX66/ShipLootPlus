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
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }
    }
}
