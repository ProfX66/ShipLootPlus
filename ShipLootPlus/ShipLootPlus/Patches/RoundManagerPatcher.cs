using HarmonyLib;
using ShipLootPlus.Utils;
using System.Threading.Tasks;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class RoundManagerPatcher
    {
        /// <summary>
        /// Refresh data when the level has loaded all objects
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoundManager), nameof(RefreshEnemyVents))]
        private static void RefreshEnemyVents()
        {
            UiHelper.RefreshElementValues();
        }
    }
}
