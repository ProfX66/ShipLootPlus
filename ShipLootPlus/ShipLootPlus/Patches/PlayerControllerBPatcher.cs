using GameNetcodeStuff;
using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class PlayerControllerBPatcher
    {
        /// <summary>
        /// Refresh data when user grabs an object
        /// </summary>
        [HarmonyPatch(typeof(PlayerControllerB), nameof(BeginGrabObject))]
        [HarmonyPostfix]
        private static void BeginGrabObject()
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data when user drops an object
        /// </summary>
        [HarmonyPatch(typeof(PlayerControllerB), nameof(DiscardHeldObject))]
        [HarmonyPostfix]
        private static void DiscardHeldObject()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
