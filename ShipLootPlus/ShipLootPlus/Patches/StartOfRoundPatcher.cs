using HarmonyLib;
using ShipLootPlus.Utils;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class StartOfRoundPatcher
    {
        /// <summary>
        /// Refresh data when players are revived
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "ReviveDeadPlayers")]
        private static void PlayerHasRevivedServerRpc()
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data at the end of the game round
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "EndOfGameClientRpc")]
        private static void RefreshDay()
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data when the lobby starts
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartGame))]
        private static void StartGame()
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data on RPC ship sync
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), nameof(SyncShipUnlockablesClientRpc))]
        [HarmonyPostfix]
        private static void SyncShipUnlockablesClientRpc()
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh data when the ship object is reset
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(StartOfRound), nameof(ResetShip))]
        [HarmonyPostfix]
        private static void ResetShip(StartOfRound __instance)
        {
            UiHelper.RefreshElementValues();
        }

        /// <summary>
        /// Refresh the data when the ship loot values have changed
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), nameof(LoadShipGrabbableItems))]
        [HarmonyPostfix]
        private static void LoadShipGrabbableItems()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
