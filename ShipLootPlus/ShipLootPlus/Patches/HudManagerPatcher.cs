using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HarmonyLib;
using ShipLootPlus.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static ShipLootPlus.ShipLootPlus;
using Object = UnityEngine.Object;

namespace ShipLootPlus.Patches
{
    [HarmonyPatch]
    internal class HudManagerPatcher
    {
        public static List<ShipLootPlusItem> UiElementList { get; set; }
        private static List<string> ItemsToIgnore { get; set; }
        private static float timeLeftDisplay;
        private const float DisplayTime = 5f;

        /// <summary>
        /// Method called when scanning
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="context"></param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HUDManager), nameof(HUDManager.PingScan_performed))]
        private static void OnScan(HUDManager __instance, InputAction.CallbackContext context)
        {
            if (GameNetworkManager.Instance.localPlayerController == null) return;
            if (!context.performed || !__instance.CanPlayerScan() || __instance.playerPingingScan > -0.5f) return;

            if (!ConfigSettings.AllowOutside.Value)
            {
                if (!StartOfRound.Instance.inShipPhase && !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
                    return;
            }

            ItemsToIgnore = new List<string>
            {
                "ClipboardManual",
                "StickyNoteItem"
            };
#if DEBUG
            Log.LogWarning("in OnScan");
#endif

            if (UiElementList == null) { UiElementList = LoadObjectList(); }
            if (UiElementList.Any(e => e.gameOjbect == null)) CreateUiElements();

#if DEBUG
            Log.LogInfo($"Objects added: {UiElementList.Count}");
#endif

            if (UiElementList.Count == 3)
            {
                foreach (var item in UiElementList.Where(t => t.textMeshProUGui != null))
                {
                    item.textMeshProUGui.fontSize = ShipLootPlusPositions.TwoElementFontSize;
                }
            }
            else if (UiElementList.Count == 2)
            {
                foreach (var item in UiElementList.Where(t => t.textMeshProUGui != null))
                {
                    item.textMeshProUGui.fontSize = ShipLootPlusPositions.OneElementFontSize;
                }
            }

            float value = CalculateLootValue(ItemsToIgnore);
            TimeOfDay tod = TimeOfDay.Instance;

            Dictionary<string, string> LineReplacements = new Dictionary<string, string>
            {
                { "%LootValue%", value.ToString("F0") },
                { "%FulfilledValue%", tod.quotaFulfilled.ToString() },
                { "%QuotaValue%", tod.profitQuota.ToString() },
                { "%DaysLeft%", tod.daysUntilDeadline.ToString() }
            };

            foreach (ShipLootPlusItem slpi in UiElementList.Where(e => e.ShowText))
            {
#if DEBUG
                Log.LogInfo($"Setting [ {slpi.Name} ] text");
                Log.LogInfo($"Pre-Expansion: {slpi.format}");
#endif
                string textContent = ExpandVariables(slpi.format, LineReplacements);
                slpi.textMeshProUGui.text = textContent;
                if (ConfigSettings.AllCaps.Value) { slpi.textMeshProUGui.text = slpi.textMeshProUGui.text.ToUpper(); }
            }

            timeLeftDisplay = DisplayTime;
            if (UiElementList.Any(e => !e.gameOjbect.activeSelf))
            {
                GameNetworkManager.Instance.StartCoroutine(DisplayDataCoroutine());
            }

        }

        /// <summary>
        /// Replace all found variables in the passed string from the passed dictionary
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Substitutions"></param>
        /// <returns>string</returns>
        public static string ExpandVariables(string format, Dictionary<string, string> Substitutions = null)
        {
            string stReturn = format;
            if (Substitutions != null)
            {
                foreach (KeyValuePair<string, string> item in Substitutions)
                {
                    string rxPattern = Regex.Escape(item.Key);
                    stReturn = Regex.Replace(stReturn, rxPattern, item.Value, RegexOptions.IgnoreCase);
#if DEBUG
                    Log.LogInfo($"Replacing [ {rxPattern} ] in [ {format} ] with [ {item.Value} ] => [{stReturn}]");
#endif
                }
            }

            return stReturn;
        }

        /// <summary>
        /// Load the main object with elements
        /// </summary>
        /// <returns>List<ShipLootPlusItem></returns>
        private static List<ShipLootPlusItem> LoadObjectList()
        {
#if DEBUG
            Log.LogInfo("Adding Line object.");
#endif
            List <ShipLootPlusItem> TempObj = new List<ShipLootPlusItem>
            {
                new ShipLootPlusItem
                {
                    Name = "Line",
                    color = ConvertHexColor(ConfigSettings.LineColor.Value, 0.75f),
                    ShowImg = true,
                    ShowText = false,
                    Enabled = true,
                    OffsetX = 60f,
                    OffsetY = -54.5f
                }
            };

#if DEBUG
            Log.LogInfo($"Show Loot? {ConfigSettings.ShowShipLoot.Value}");
#endif
            if (ConfigSettings.ShowShipLoot.Value)
            {
#if DEBUG
                Log.LogInfo("Adding Loot object.");
#endif
                TempObj.Add(new ShipLootPlusItem
                {
#if DEBUG
                    Name = "ShipLoot",
#endif
                    Location = ConfigSettings.ShipLootPosition.Value,
                    color = ConvertHexColor(ConfigSettings.ShipLootColor.Value),
                    format = ConfigSettings.ShipLootFormat.Value,
                    Enabled = true,
                    OffsetX = GetXOffset(ConfigSettings.ShipLootPosition.Value),
                    OffsetY = GetYOffset(ConfigSettings.ShipLootPosition.Value)
                });

            }

#if DEBUG
            Log.LogInfo($"Show Quota? {ConfigSettings.ShowShipLoot.Value}");
#endif
            if (ConfigSettings.ShowQuota.Value)
            {
#if DEBUG
                Log.LogInfo("Adding Quota object.");
#endif
                TempObj.Add(new ShipLootPlusItem
                {
                    Name = "Quota",
                    Location = ConfigSettings.QuotaPosition.Value,
                    color = ConvertHexColor(ConfigSettings.QuotaColor.Value),
                    format = ConfigSettings.QuotaFormat.Value,
                    Enabled = true,
                    OffsetX = GetXOffset(ConfigSettings.QuotaPosition.Value),
                    OffsetY = GetYOffset(ConfigSettings.QuotaPosition.Value)
                });
            }

#if DEBUG
            Log.LogInfo($"Show Days? {ConfigSettings.ShowShipLoot.Value}");
#endif
            if (ConfigSettings.ShowDays.Value)
            {
#if DEBUG
                Log.LogInfo("Adding Days object.");
#endif
                TempObj.Add(new ShipLootPlusItem
                {
                    Name = "Days",
                    Location = ConfigSettings.DaysPosition.Value,
                    color = ConvertHexColor(ConfigSettings.DaysColor.Value),
                    format = ConfigSettings.DaysFormat.Value,
                    Enabled = true,
                    OffsetX = GetXOffset(ConfigSettings.DaysPosition.Value),
                    OffsetY = GetYOffset(ConfigSettings.DaysPosition.Value)
                });
            }

            return TempObj;
        }

        /// <summary>
        /// Convert a HTML hex color code to a Color object
        /// </summary>
        /// <param name="hColor"></param>
        /// <param name="alpha"></param>
        /// <returns>Color</returns>
        private static Color ConvertHexColor(string hColor, float alpha = 0.95f)
        {
            if (!Regex.IsMatch(hColor, "^#")) { hColor = string.Concat("#", hColor); }

            if (ColorUtility.TryParseHtmlString(hColor, out Color elemColor))
            {
                return elemColor;
            }

            return new Color(25, 213, 108);
        }

        /// <summary>
        /// Convert location enum to X float value
        /// </summary>
        /// <param name="location"></param>
        /// <returns>float</returns>
        private static float GetXOffset(ElementLocation location)
        {
            switch (location)
            {
                case ElementLocation.Top:
                    return ShipLootPlusPositions.TopOffsetX;
                case ElementLocation.Middle:
                    return ShipLootPlusPositions.MiddleOffsetX;
                case ElementLocation.Bottom:
                    return ShipLootPlusPositions.BottomOffsetX;
            }

            return 60f;
        }

        /// <summary>
        /// Convert location enum to Y float value
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static float GetYOffset(ElementLocation location)
        {
            switch (location)
            {
                case ElementLocation.Top:
                    return ShipLootPlusPositions.TopOffsetY;
                case ElementLocation.Middle:
                    return ShipLootPlusPositions.MiddleOffsetY;
                case ElementLocation.Bottom:
                    return ShipLootPlusPositions.BottomOffsetY;
            }

            return -54.5f;
        }

        /// <summary>
        /// Create actual UI objects
        /// </summary>
        private static void CreateUiElements()
        {
            GameObject valueCounter = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD/BottomMiddle/ValueCounter");
            if (!valueCounter)
            {
                Log.LogError("Failed to find ValueCounter object to copy!");
                return;
            }

            IEnumerable<ShipLootPlusItem> enabledTextElements = UiElementList.Where(e => e.Enabled && e.ShowText);
            if (enabledTextElements.Count() == 1)
            {
                ShipLootPlusItem SingleElement = enabledTextElements.FirstOrDefault();
#if DEBUG
                Log.LogWarning($"[PrePos-{SingleElement.Name}] X: {SingleElement.OffsetX} => Y: {SingleElement.OffsetY}");
#endif
                SingleElement.Location = ElementLocation.Middle;
                SingleElement.OffsetX = GetXOffset(SingleElement.Location);
                SingleElement.OffsetY = GetYOffset(SingleElement.Location) + 2;
#if DEBUG
                Log.LogWarning($"[PostPos-{SingleElement.Name}] Loc: Middle => X: {SingleElement.OffsetX} => Y: {SingleElement.OffsetY}");
                Log.LogInfo("--");
#endif
            }
            else if (enabledTextElements.Count() == 2)
            {
                IEnumerable<ShipLootPlusItem> topElements = enabledTextElements.Where(e => e.Location == ElementLocation.Top);
                IEnumerable<ShipLootPlusItem> middleElements = enabledTextElements.Where(e => e.Location == ElementLocation.Middle);
                IEnumerable<ShipLootPlusItem> bottomElements = enabledTextElements.Where(e => e.Location == ElementLocation.Bottom);
#if DEBUG
                Log.LogInfo($"topElements   : {topElements.Count()}");
                Log.LogInfo($"middleElements: {middleElements.Count()}");
                Log.LogInfo($"bottomElements: {bottomElements.Count()}");
#endif
                if (bottomElements.Count() != 0)
                {
#if DEBUG
                    Log.LogInfo("Has Bottom");
#endif
                    if (topElements.Count() != 0)
                    {
#if DEBUG
                        Log.LogInfo("Has Top");
#endif
                        foreach (var item in bottomElements)
                        {
#if DEBUG
                            Log.LogWarning($"[PrePos-{item.Name}] Loc: Bottom => X: {item.OffsetX} => Y: {item.OffsetY}");
#endif
                            item.Location = ElementLocation.Middle;
                            item.OffsetX = GetXOffset(ElementLocation.Middle) + ShipLootPlusPositions.TwoElementOffsetX;
                            item.OffsetY = GetYOffset(ElementLocation.Middle) + ShipLootPlusPositions.TwoElementOffsetY;
#if DEBUG
                            Log.LogWarning($"[PostPos-{item.Name}] Loc: Middle => X: {item.OffsetX} => Y: {item.OffsetY}");
                            Log.LogInfo("--");
#endif
                        }
                    }
                    else
                    {
#if DEBUG
                        Log.LogInfo("No Top - Modifying Middle objects to Top");
#endif
                        foreach (var item in middleElements)
                        {
#if DEBUG
                            Log.LogWarning($"[PrePos-{item.Name}] Loc: Middle => X: {item.OffsetX} => Y: {item.OffsetY}");
#endif
                            item.Location = ElementLocation.Top;
                            item.OffsetX = GetXOffset(ElementLocation.Top);
                            item.OffsetY = GetYOffset(ElementLocation.Top);
#if DEBUG
                            Log.LogWarning($"[PostPos-{item.Name}] Loc: Top => X: {item.OffsetX} => Y: {item.OffsetY}");
                            Log.LogInfo("--");
#endif
                        }
#if DEBUG
                        Log.LogInfo("No Top - Modifying Bottom objects to Middle");
#endif
                        foreach (var item in bottomElements)
                        {
#if DEBUG
                            Log.LogWarning($"[PrePos-{item.Name}] Loc: Bottom => X: {item.OffsetX} => Y: {item.OffsetY}");
#endif
                            item.Location = ElementLocation.Middle;
                            item.OffsetX = GetXOffset(ElementLocation.Middle) + ShipLootPlusPositions.TwoElementOffsetX;
                            item.OffsetY = GetYOffset(ElementLocation.Middle) + ShipLootPlusPositions.TwoElementOffsetY;
#if DEBUG
                            Log.LogWarning($"[PostPos-{item.Name}] Loc: Middle => X: {item.OffsetX} => Y: {item.OffsetY}");
                            Log.LogInfo("--");
#endif
                        }
                    }
                }
            }

            foreach (ShipLootPlusItem slpi in UiElementList.Where(e => e.gameOjbect == null))
            {
                slpi.gameOjbect = Object.Instantiate(valueCounter.gameObject, valueCounter.transform.parent, false);
#if DEBUG
                Log.LogWarning($"[Entry-{slpi.Name}] X: {slpi.gameOjbect.transform.position.x} => Y: {slpi.gameOjbect.transform.position.y}");
#endif
                slpi.gameOjbect.transform.Translate(0f, 1f, 0f);
#if DEBUG
                Log.LogWarning($"[Trans-{slpi.Name}] X: {slpi.gameOjbect.transform.position.x} => Y: {slpi.gameOjbect.transform.position.y}");
#endif
                TextMeshProUGUI textObj = slpi.gameOjbect.GetComponentInChildren<TextMeshProUGUI>();
                textObj.enabled = false;

                Image imgObj = slpi.gameOjbect.GetComponentInChildren<Image>();
                imgObj.enabled = false;

                if (slpi.ShowText)
                {
                    textObj.enabled = true;
                    textObj.color = slpi.color;
                    textObj.fontSize = ShipLootPlusPositions.DefaultFontSize;
                    slpi.textMeshProUGui = textObj;

                    if (enabledTextElements.Count() == 1)
                    {
                        textObj.fontSize = ShipLootPlusPositions.OneElementFontSize;
                    }
                    else if (enabledTextElements.Count() == 2)
                    {
                        textObj.fontSize = ShipLootPlusPositions.TwoElementFontSize;
                    }
                }

                if (slpi.ShowImg)
                {
                    imgObj.enabled = true;
                    imgObj.color = slpi.color;
                    RectTransform rectTransform = imgObj.GetComponent<RectTransform>();
                    float newWidth = rectTransform.rect.width / 0.25f;
                    float newHeight = rectTransform.rect.height;
                    rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
                }

                Vector3 pos = slpi.gameOjbect.transform.localPosition;

                float posX = pos.x;
                float posY = -54.5f;
#if DEBUG
                Log.LogWarning($"[{slpi.Name}] X: {posX} => Y: {posY}");
#endif
                posX = pos.x + slpi.OffsetX;
                posY = slpi.OffsetY;
#if DEBUG
                Log.LogWarning($"[Pre-{slpi.Name}] X: {slpi.gameOjbect.transform.position.x} => Y: {slpi.gameOjbect.transform.position.y}");
#endif
                slpi.gameOjbect.transform.localPosition = new Vector3(posX, posY, pos.z);
#if DEBUG
                Log.LogWarning($"[Post-{slpi.Name}] X: {slpi.gameOjbect.transform.position.x} => Y: {slpi.gameOjbect.transform.position.y}");
                Log.LogInfo("--------------");
#endif
            }
        }

        /// <summary>
        /// Calculate the value of all scrap in the ship except ignored items
        /// </summary>
        /// <returns>float</returns>
        private static float CalculateLootValue(List<string> Ignored)
        {
            GameObject ship = GameObject.Find("/Environment/HangarShip");
            List<GrabbableObject> loot = ship.GetComponentsInChildren<GrabbableObject>().Where(obj => !Ignored.Contains(obj.name)).ToList();
#if DEBUG
            Log.LogInfo($"[CalculateLootValue] Valid item count: {loot.Count}");
            Log.LogInfo("Calculating total ship scrap value.");
            loot.Do(scrap => Log.LogInfo($"{scrap.name} - ${scrap.scrapValue}"));
#endif
            Log.LogDebug("Calculating total ship scrap value.");
            loot.Do(scrap => Log.LogDebug($"{scrap.name} - ${scrap.scrapValue}"));
            return loot.Sum(scrap => scrap.scrapValue);
        }

        /// <summary>
        /// Display the new elements for a time and then hide them
        /// </summary>
        /// <returns></returns>
        private static IEnumerator DisplayDataCoroutine()
        {
#if DEBUG
            Log.LogWarning($"Showing [ {UiElementList.Count} ] objects...");
#endif
            UiElementList.ForEach(obj => obj.gameOjbect.SetActive(true));

            while (timeLeftDisplay > 0f)
            {
                float time = timeLeftDisplay;
                timeLeftDisplay = 0f;
                yield return new WaitForSeconds(time);
            }
#if DEBUG
            Log.LogWarning($"Hiding [ {UiElementList.Count} ] objects...");
#endif
            UiElementList.ForEach(obj => obj.gameOjbect.SetActive(false));
        }
    }

    /// <summary>
    /// Gameobject custom object
    /// </summary>
    public class ShipLootPlusItem
    {
        public string Name;
        public ElementLocation Location;
        public float OffsetX;
        public float OffsetY;
        public Color color;
        public string format;
        public GameObject gameOjbect;
        public TextMeshProUGUI textMeshProUGui;
        public bool ShowText = true;
        public bool ShowImg = false;
        public bool Enabled = false;
    }

    /// <summary>
    /// Size and postion constants
    /// </summary>
    public class ShipLootPlusPositions
    {
        public const float TopOffsetX = 37f;
        public const float TopOffsetY = -50f;
        public const float MiddleOffsetX = 41f;
        public const float MiddleOffsetY = -56f;
        public const float BottomOffsetX = 45f;
        public const float BottomOffsetY = -62f;
        public const float TwoElementOffsetX = 4f;
        public const float TwoElementOffsetY = -4f;
        public const float DefaultFontSize = 13f;
        public const float TwoElementFontSize = 18f;
        public const float OneElementFontSize = 23f;
    }
}
