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
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh data at the end of the game round
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "EndOfGameClientRpc")]
        private static void RefreshDay()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh data when the lobby starts
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartGame))]
        private static void StartGame()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh data when the lobby starts
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(Start))]
        private static void Start()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh data at the end of the game round
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), nameof(SetTimeAndPlanetToSavedSettings))]
        private static void SetTimeAndPlanetToSavedSettings()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh data on RPC ship sync
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), nameof(SyncShipUnlockablesClientRpc))]
        [HarmonyPostfix]
        private static void SyncShipUnlockablesClientRpc()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh data when the ship object is reset
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(StartOfRound), nameof(ResetShip))]
        [HarmonyPostfix]
        private static void ResetShip(StartOfRound __instance)
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh the data when the ship loot values have changed
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), nameof(LoadShipGrabbableItems))]
        [HarmonyPostfix]
        private static void LoadShipGrabbableItems()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        [HarmonyPatch(typeof(StartOfRound), nameof(AllPlayersHaveRevivedClientRpc))]
        [HarmonyPostfix]
        private static void AllPlayersHaveRevivedClientRpc()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh the data when the ship loot values have changed
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), nameof(SetMapScreenInfoToCurrentLevel))]
        [HarmonyPostfix]
        private static void SetMapScreenInfoToCurrentLevel()
        {
            if (!UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }
    }
}
