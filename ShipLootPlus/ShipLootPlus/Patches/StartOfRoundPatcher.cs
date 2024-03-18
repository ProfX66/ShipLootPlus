using HarmonyLib;
using ShipLootPlus.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[PlayerHasRevivedServerRpc] Received ClientRpc notifying a player was revived");
            if (!ConfigSettings.DisableRpcHooks.Value && GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[EndOfGameClientRpc] Received ClientRpc notifying that the round has ended");
            if (!ConfigSettings.DisableRpcHooks.Value && GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[StartGame] Lobby has been started");
            if (GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[Start] Round has started");
            if (GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[SetTimeAndPlanetToSavedSettings] Moon change has been saved");
            if (GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        /// <summary>
        /// Refresh data on RPC ship sync
        /// NOTE: Needs to be not-rate limited!
        /// </summary>
        [HarmonyPatch(typeof(StartOfRound), nameof(SyncShipUnlockablesClientRpc))]
        [HarmonyPostfix]
        private static void SyncShipUnlockablesClientRpc()
        {
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[SyncShipUnlockablesClientRpc] Received ClientRpc to sync ship unlockable items - Needs first time sync? {UiHelper.FirstTimeSync}");
            if (!ConfigSettings.DisableRpcHooks.Value)
            {
                if (!UiHelper.FirstTimeSync)
                {
                    ShipLootPlus.Log.LogWarning("Fixing ship scrap tags on first RPC sync...");
                    List<GrabbableObject> scrapList = Object.FindObjectsOfType<GrabbableObject>().Where(s => s.itemProperties.isScrap).ToList();
                    if (scrapList.Count == 0)
                    {
                        ShipLootPlus.Log.LogWarning("List was empty - Attempting to get objects from ship game object...");
                        scrapList = GameObject.Find("/Environment/HangarShip").GetComponentsInChildren<GrabbableObject>().ToList();
                    }

                    scrapList.ForEach(s =>
                    {
                        bool isInShipRoom = s.isInShipRoom;
                        bool isInElevator = s.isInElevator;
                        s.isInShipRoom = true;
                        s.isInElevator = true;
                        ShipLootPlus.Log.LogInfo($"[{s.name}] isInShipRoom: {isInShipRoom} => {s.isInElevator} | isInElevator: {isInElevator} => {s.isInElevator}");
                    });

                    UiHelper.FirstTimeSync = true;
                }
                UiHelper.RefreshElementValues();
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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[ResetShip] Ship state has been reset");
            if (GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[LoadShipGrabbableItems] Loading all items in the ship container");
            if (GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
            {
                GameNetworkManager.Instance.StartCoroutine(UiHelper.UpdateDatapoints());
            }
        }

        [HarmonyPatch(typeof(StartOfRound), nameof(AllPlayersHaveRevivedClientRpc))]
        [HarmonyPostfix]
        private static void AllPlayersHaveRevivedClientRpc()
        {
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[PlayerHasRevivedServerRpc] Received ClientRpc notifying that all players were revived");
            if (!ConfigSettings.DisableRpcHooks.Value && GameNetworkManager.Instance != null && !UiHelper.IsRefreshing)
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
            if (ConfigSettings.DebugMode.Value) ShipLootPlus.Log.LogMessage($"[SetMapScreenInfoToCurrentLevel] Moon has changed");
            UiHelper.RefreshElementValues();
        }
    }
}
