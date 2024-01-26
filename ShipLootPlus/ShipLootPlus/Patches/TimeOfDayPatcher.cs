using HarmonyLib;
using ShipLootPlus.Utils;
using System.Threading.Tasks;

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
            UiHelper.RefreshElementValues();
        }
    }
}
