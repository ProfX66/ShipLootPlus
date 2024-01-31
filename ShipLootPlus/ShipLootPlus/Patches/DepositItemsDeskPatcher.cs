using GameNetcodeStuff;
using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    internal class DepositItemsDeskPatcher
    {
        /// <summary>
        /// Updated data when placing objects on the company sale counter
        /// NOTE: Needs to be not-rate limited!
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(DepositItemsDesk), nameof(PlaceItemOnCounter))]
        [HarmonyPostfix]
        private static void PlaceItemOnCounter(PlayerControllerB __instance)
        {
            UiHelper.RefreshElementValues();
        }
    }
}
