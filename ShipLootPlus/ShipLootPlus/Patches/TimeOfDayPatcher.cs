using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class TimeOfDayPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TimeOfDay), nameof(UpdateProfitQuotaCurrentTime))]
        private static void UpdateProfitQuotaCurrentTime()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
