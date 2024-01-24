using GameNetcodeStuff;
using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class PlayerControllerBPatcher
    {
        [HarmonyPatch(typeof(PlayerControllerB), nameof(SetItemInElevator))]
        [HarmonyPostfix]
        private static void SetItemInElevator()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
