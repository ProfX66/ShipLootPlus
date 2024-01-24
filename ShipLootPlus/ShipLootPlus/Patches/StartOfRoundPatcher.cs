using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class StartOfRoundPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "ReviveDeadPlayers")]
        private static void PlayerHasRevivedServerRpc()
        {
            UiHelper.RefreshElementValues();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "EndOfGameClientRpc")]
        private static void RefreshDay()
        {
            UiHelper.RefreshElementValues();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartGame))]
        private static void StartGame()
        {
            UiHelper.RefreshElementValues();
        }

        [HarmonyPatch(typeof(StartOfRound), nameof(SyncShipUnlockablesClientRpc))]
        [HarmonyPostfix]
        private static void SyncShipUnlockablesClientRpc()
        {
            UiHelper.RefreshElementValues();
        }

        [HarmonyPatch(typeof(StartOfRound), nameof(ResetShip))]
        [HarmonyPostfix]
        private static void ResetShip(StartOfRound __instance)
        {
            UiHelper.RefreshElementValues();
        }

        [HarmonyPatch(typeof(StartOfRound), nameof(LoadShipGrabbableItems))]
        [HarmonyPostfix]
        private static void LoadShipGrabbableItems()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
