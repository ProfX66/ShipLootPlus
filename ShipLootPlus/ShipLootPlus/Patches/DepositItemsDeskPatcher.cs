using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class DepositItemsDeskPatcher
    {
        [HarmonyPatch(typeof(DepositItemsDesk), nameof(SellItemsOnServer))]
        [HarmonyPostfix]
        private static void SellItemsOnServer()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
