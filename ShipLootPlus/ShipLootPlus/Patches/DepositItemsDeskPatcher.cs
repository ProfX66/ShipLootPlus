using GameNetcodeStuff;
using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    internal class DepositItemsDeskPatcher
    {
        [HarmonyPatch(typeof(DepositItemsDesk), nameof(PlaceItemOnCounter))]
        [HarmonyPostfix]
        private static void PlaceItemOnCounter(PlayerControllerB __instance)
        {
            UiHelper.RefreshElementValues();
        }
    }
}
