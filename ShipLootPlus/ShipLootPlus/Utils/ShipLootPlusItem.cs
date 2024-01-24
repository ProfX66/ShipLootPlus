using TMPro;
using UnityEngine;
using static ShipLootPlus.ShipLootPlus;

namespace ShipLootPlus.Utils
{
    /// <summary>
    /// UI element data
    /// </summary>
    internal class ShipLootPlusItem
    {
        public string name;
        public ElementLocation location;
        public Vector3 offset = new Vector3(0.05f, -0.083f, 0f);
        public Color color;
        public string format;
        public GameObject gameOjbect;
        public TextMeshProUGUI textMeshProUGui;
        public bool enabled = false;
        public bool image = false;
    }

    /// <summary>
    /// Loot data
    /// </summary>
    internal class LootItem
    {
        public float Value;
        public int Count;
    }
}
