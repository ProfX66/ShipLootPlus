using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class RoundManagerPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(RefreshEnemyVents))]
        private static void RefreshEnemyVents()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
