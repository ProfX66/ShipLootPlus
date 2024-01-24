using Figgle;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShipLootPlus.ShipLootPlus;
using Object = UnityEngine.Object;

namespace ShipLootPlus.Utils
{
    internal class UiHelper
    {
        private static List<string> ItemsToIgnore => new List<string> { "ClipboardManual", "StickyNoteItem" };
        public static List<ShipLootPlusItem> UiElementList { get; set; }
        public static GameObject ContainerObject { get; set; }
        public static float timeLeftDisplay { get; set; }

        #region Methods

        /// <summary>
        /// Load each UI element data from config values
        /// </summary>
        /// <returns></returns>
        private static List<ShipLootPlusItem> LoadObjectList()
        {
            Log.LogInfo("[LoadObjectList] Adding HUD element object data from config values");
            return new List<ShipLootPlusItem>
            {
                new ShipLootPlusItem
                {
                    name = "LineImage",
                    color = ConvertHexColor(ConfigSettings.LineColor.Value, 0.75f),
                    enabled = ConfigSettings.ShowLine.Value,
                    image = true
                },
                new ShipLootPlusItem
                {
                    name = "ShipLoot",
                    location = ConfigSettings.ShipLootPosition.Value,
                    color = ConvertHexColor(ConfigSettings.ShipLootColor.Value),
                    format = ConfigSettings.ShipLootFormat.Value,
                    enabled = ConfigSettings.ShowShipLoot.Value
                },
                new ShipLootPlusItem
                {
                    name = "Quota",
                    location = ConfigSettings.QuotaPosition.Value,
                    color = ConvertHexColor(ConfigSettings.QuotaColor.Value),
                    format = ConfigSettings.QuotaFormat.Value,
                    enabled = ConfigSettings.ShowQuota.Value
                },
                new ShipLootPlusItem
                {
                    name = "Days",
                    location = ConfigSettings.DaysPosition.Value,
                    color = ConvertHexColor(ConfigSettings.DaysColor.Value),
                    format = ConfigSettings.DaysFormat.Value,
                    enabled = ConfigSettings.ShowDays.Value
                }
            };
        }

        /// <summary>
        /// Update each UI element data from config values
        /// </summary>
        private static void UpdateObjectSettings()
        {
            Log.LogInfo($"[UpdateObjectSettings] Updating HUD element object data from config values");
            foreach (var obj in UiElementList)
            {
                switch (obj.name)
                {
                    case "LineImage":
                        obj.color = ConvertHexColor(ConfigSettings.LineColor.Value, 0.75f);
                        obj.enabled = ConfigSettings.ShowLine.Value;
                        break;
                    case "ShipLoot":
                        obj.color = ConvertHexColor(ConfigSettings.ShipLootColor.Value);
                        obj.format = ConfigSettings.ShipLootFormat.Value;
                        obj.enabled = ConfigSettings.ShowShipLoot.Value;
                        break;
                    case "Quota":
                        obj.color = ConvertHexColor(ConfigSettings.QuotaColor.Value);
                        obj.format = ConfigSettings.QuotaFormat.Value;
                        obj.enabled = ConfigSettings.ShowQuota.Value;
                        break;
                    case "Days":
                        obj.color = ConvertHexColor(ConfigSettings.DaysColor.Value);
                        obj.format = ConfigSettings.DaysFormat.Value;
                        obj.enabled = ConfigSettings.ShowDays.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Actually create each UI element
        /// </summary>
        public static void CreateUiElements()
        {
            Log.LogWarning(string.Format("\n{0}", FiggleFonts.Doom.Render("Adding Objects")));

            GameObject valueCounter = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD/BottomMiddle/ValueCounter");
            if (!valueCounter)
            {
                Log.LogError("Failed to find ValueCounter object to copy!");
                return;
            }

            if (UiElementList == null) { UiElementList = LoadObjectList(); }

            Vector3 offset = new Vector3(0.05f, 0f, 0f);
            Vector3 scale = new Vector3(0.7f, 0.7f, 0.7f);
            Vector3 VecLocPos = Vector3.zero;
            Quaternion QuatLocRot = Quaternion.identity;
            Vector3 VecPos = Vector3.zero;
            Quaternion QuatRot = Quaternion.identity;
            LogPos(valueCounter.gameObject, out VecPos, out QuatRot, out VecLocPos, out QuatLocRot);

            ContainerObject = Object.Instantiate(valueCounter.gameObject, valueCounter.transform.parent, false);
            ContainerObject.name = "ShipLootPlus";
            ContainerObject.transform.Translate(0f, 1f, 0f);
            Vector3 pos = ContainerObject.transform.localPosition;
            ContainerObject.transform.localPosition = new Vector3(pos.x + 50f, -50f, pos.z);
            ContainerObject.transform.Rotate(QuatRot.x, QuatRot.y, 358f);
            TextMeshProUGUI textObj = ContainerObject.GetComponentInChildren<TextMeshProUGUI>();
            LogPos(ContainerObject.gameObject, out _, out _, out _, out _);

            Image imagObj = ContainerObject.GetComponentInChildren<Image>();
            ShipLootPlusItem ImageElem = UiElementList.Where(e => e.image).FirstOrDefault();
            if (ImageElem != null)
            {
                Log.LogWarning("ImageElem not null");
                imagObj.name = ImageElem.name;
                imagObj.color = ImageElem.color;
                RectTransform imagTransform = imagObj.GetComponent<RectTransform>();
                float newWidth = imagTransform.rect.width / 0.15f;
                float newHeight = imagTransform.rect.height;
                imagTransform.sizeDelta = new Vector2(newWidth, newHeight);
                ImageElem.gameOjbect = imagObj.gameObject;
                imagObj.enabled = ImageElem.enabled;
                LogPos(imagObj.gameObject, out _, out _, out _, out _);
            }

            GameObject slp = new GameObject
            {
                name = "SLP"
            };
            slp.transform.SetParent(ContainerObject.transform);
            slp.transform.position = ContainerObject.transform.position;
            slp.transform.localPosition = ContainerObject.transform.localPosition;
            slp.AddComponent<RectTransform>();
            LogPos(slp, out _, out _, out _, out _);

            bool addedFirst = false;
            GameObject tempObj = null;
            foreach (ShipLootPlusItem TextElem in UiElementList.Where(e => e.gameOjbect == null && e.enabled && !e.image))
            {
                TextElem.gameOjbect = Object.Instantiate(textObj.gameObject, textObj.transform.parent);
                TextElem.gameOjbect.name = TextElem.name;

                if (!addedFirst)
                {
                    Vector3 tmpPos = textObj.transform.position;
                    TextElem.gameOjbect.transform.position = new Vector3(tmpPos.x + 0.12f, tmpPos.y + 0.01f, tmpPos.z);
                    addedFirst = true;
                }
                else
                {
                    TextElem.gameOjbect.transform.position = tempObj.transform.position + TextElem.offset;
                }

                TextElem.gameOjbect.transform.SetParent(slp.transform);
                TextElem.textMeshProUGui = TextElem.gameOjbect.GetComponentInChildren<TextMeshProUGUI>();
                TextElem.textMeshProUGui.text = TextElem.format;
                TextElem.textMeshProUGui.characterSpacing = -4f;
                TextElem.textMeshProUGui.color = TextElem.color;
                TextElem.textMeshProUGui.overflowMode = TextOverflowModes.Ellipsis;
                TextElem.textMeshProUGui.enableWordWrapping = false;
                TextElem.textMeshProUGui.maxVisibleLines = 1;
                TextElem.textMeshProUGui.ForceMeshUpdate();

                RectTransform textTransform = TextElem.textMeshProUGui.GetComponent<RectTransform>();
                textTransform.sizeDelta = new Vector2(textTransform.sizeDelta.x + 100, textTransform.sizeDelta.y);

                tempObj = TextElem.gameOjbect;
                LogPos(TextElem.gameOjbect, out _, out _, out _, out _);
            }

            RefreshElementValues();

            RectTransform rectTransform = slp.GetComponent<RectTransform>();
            ScaleAroundRelative(rectTransform.gameObject, slp.transform.localPosition, scale);
            LogPos(slp, out _, out _, out _, out _);

            Vector3 posx = slp.transform.localPosition;
            slp.transform.localPosition = new Vector3(posx.x + 0f, posx.y + 16.5f, posx.z);
            LogPos(slp, out _, out _, out _, out _);

            Object.Destroy(textObj.gameObject);
            Log.LogWarning(string.Format("\n{0}", FiggleFonts.Doom.Render("Showing Objects")));
            if (ConfigSettings.AlwaysShow.Value) ContainerObject.SetActive(true);
        }

        /// <summary>
        /// Resets all element settings when specific config items are refreshed
        /// </summary>
        public static void ResetUiElements()
        {
            UpdateObjectSettings();
#if DEBUG
            Log.LogWarning($"[ResetUiElements] Image objects to refresh: 1");
#endif
            Image imagObj = ContainerObject.GetComponentInChildren<Image>();
            if (imagObj != null)
            {
#if DEBUG
                Log.LogInfo("[imagObj] not null");
#endif
                ShipLootPlusItem item = UiElementList.Where(c => c.name == "LineImage").FirstOrDefault();
                if (item != null)
                {
#if DEBUG
                    Log.LogInfo("[imagObj.item] not null");
#endif
                    imagObj.enabled = item.enabled;
                    imagObj.color = item.color;
                }
            }

            Transform slpObj = ContainerObject.transform.Cast<Transform>().Where(child => child.name == "SLP").FirstOrDefault();
            IEnumerable<TextMeshProUGUI> slpText = slpObj.Cast<Transform>().Select(child => child.GetComponent<TextMeshProUGUI>()).Where(component => component != null);
#if DEBUG
            Log.LogInfo($"[ResetUiElements] Text objects to refresh: {slpText.Count()}");
#endif

            foreach (TextMeshProUGUI textItem in slpText)
            {
#if DEBUG
                Log.LogInfo($"[slpText] Updating: {textItem.name}");
#endif
                ShipLootPlusItem item = UiElementList.Where(c => c.name == textItem.name).FirstOrDefault();
                if (item != null)
                {
#if DEBUG
                    Log.LogInfo("[slpText.item] not null");
#endif
                    textItem.enabled = item.enabled;
                    textItem.color = item.color;
                    textItem.text = item.format;
                    textItem.ForceMeshUpdate();
                }
            }

            RefreshElementValues();
        }

        /// <summary>
        /// Refresh actual data values
        /// </summary>
        public static void RefreshElementValues()
        {
            if (UiElementList == null) return;
            if (ContainerObject == null) return;
            IEnumerable<ShipLootPlusItem> ElementsToUpdate = UiElementList.Where(e => !e.image && e.gameOjbect != null);
#if DEBUG
            Log.LogWarning($"Elements to update: {ElementsToUpdate.Count()}");
#endif
            if (ElementsToUpdate == null || ElementsToUpdate.Count() <= 0) return;

            LootItem shipLootValue = CalculateLootValue(ItemsToIgnore, "Ship");
            LootItem moonLootValue = CalculateLootValue(ItemsToIgnore, "Moon");
            LootItem allLootValue = CalculateLootValue(ItemsToIgnore, "All");

            TimeOfDay tod = TimeOfDay.Instance;
            StartOfRound sor = StartOfRound.Instance;
            string daysUntilDeadline = tod.daysUntilDeadline.ToString();
            if (tod.daysUntilDeadline == -1) { daysUntilDeadline = "NOW!"; }
            if (tod.daysUntilDeadline <= 0) { daysUntilDeadline = "NOW!"; }

            //TODO: Add a datapoint to show what your profit would be like if you collected all scrap and just scrap on the ship
            //      Take into account the company buying rate as another option
            //TODO: Investigate a feature to tell you what scrap on the planet you need to get to make quota (like BetterItemScan)

            Dictionary<string, string> LineReplacements = new Dictionary<string, string>
            {
                { "%ShipLootValue%", shipLootValue.Value.ToString("F0") },
                { "%MoonLootValue%", moonLootValue.Value.ToString("F0") },
                { "%AllLootValue%", allLootValue.Value.ToString("F0") },
                { "%ShipLootCount%", shipLootValue.Count.ToString() },
                { "%MoonLootCount%", moonLootValue.Count.ToString() },
                { "%AllLootCount%", allLootValue.Count.ToString() },
                { "%FulfilledValue%", tod.quotaFulfilled.ToString() },
                { "%QuotaValue%", tod.profitQuota.ToString() },
                { "%DaysLeft%", daysUntilDeadline },
                { "%DayNumber%", sor.gameStats.daysSpent.ToString() },
                { "%DayNumberHuman%", ConvertToHumanFriendly(sor.gameStats.daysSpent) }
            };

            foreach (ShipLootPlusItem slpi in ElementsToUpdate)
            {
#if DEBUG
                Log.LogInfo($"Setting [ {slpi.name} ] text");
                Log.LogInfo($"Pre-Expansion: {slpi.format}");
#endif
                string textContent = ExpandVariables(slpi.format, LineReplacements);
                slpi.textMeshProUGui.text = textContent;
                if (ConfigSettings.AllCaps.Value) { slpi.textMeshProUGui.text = slpi.textMeshProUGui.text.ToUpper(); }
#if DEBUG
                Log.LogInfo("");
#endif
            }
        }

        #endregion

        #region Method Helpers

        /// <summary>
        /// Log UI element postions
        /// </summary>
        /// <param name="InObj"></param>
        /// <param name="VecPos"></param>
        /// <param name="QuatRot"></param>
        /// <param name="VecLocPos"></param>
        /// <param name="QuatLocRot"></param>
        public static void LogPos(GameObject InObj, out Vector3 VecPos, out Quaternion QuatRot, out Vector3 VecLocPos, out Quaternion QuatLocRot)
        {
#if DEBUG
            InObj.transform.GetPositionAndRotation(out VecPos, out QuatRot);
            InObj.transform.GetLocalPositionAndRotation(out VecLocPos, out QuatLocRot);
            Log.LogInfo($"[{InObj.name}] LocalPos: {VecLocPos} => Pos: {VecPos}");
            Log.LogInfo($"[{InObj.name}] LocalRot: {QuatLocRot} => Rot: {QuatRot}");
            Log.LogWarning("--");
#endif
        }

        /// <summary>
        /// Scales the target around an arbitrary point by scaleFactor.
        /// This is relative scaling, meaning using  scale Factor of Vector3.one
        /// will not change anything and new Vector3(0.5f,0.5f,0.5f) will reduce
        /// the object size by half.
        /// The pivot is assumed to be the position in the space of the target.
        /// Scaling is applied to localScale of target.
        /// </summary>
        /// <param name="target">The object to scale.</param>
        /// <param name="pivot">The point to scale around in space of target.</param>
        /// <param name="scaleFactor">The factor with which the current localScale of the target will be multiplied with.</param>
        public static void ScaleAroundRelative(GameObject target, Vector3 pivot, Vector3 scaleFactor)
        {
            // pivot
            var pivotDelta = target.transform.localPosition - pivot;
            pivotDelta.Scale(scaleFactor);
            target.transform.localPosition = pivot + pivotDelta;

            // scale
            var finalScale = target.transform.localScale;
            finalScale.Scale(scaleFactor);
            target.transform.localScale = finalScale;
        }

        /// <summary>
        /// Display the new elements for a time and then hide them
        /// </summary>
        /// <returns></returns>
        public static IEnumerator DisplayDataCoroutine()
        {
#if DEBUG
            Log.LogWarning($"Showing [ ContainerObject ] object...");
#endif
            ContainerObject.SetActive(true);

            while (timeLeftDisplay > 0f)
            {
                float time = timeLeftDisplay;
                timeLeftDisplay = 0f;
                yield return new WaitForSeconds(time);
            }
#if DEBUG
            Log.LogWarning($"Hiding [ ContainerObject ] object...");
#endif
            ContainerObject.SetActive(false);
        }

        /// <summary>
        /// Convert a HTML hex color code to a Color object
        /// </summary>
        /// <param name="hColor"></param>
        /// <param name="alpha"></param>
        /// <returns>Color</returns>
        public static Color ConvertHexColor(string hColor, float alpha = 0.95f)
        {
            if (!Regex.IsMatch(hColor, "^#")) { hColor = string.Concat("#", hColor); }

            if (ColorUtility.TryParseHtmlString(hColor, out Color elemColor))
            {
                return elemColor;
            }

            return new Color(25, 213, 108);
        }

        /// <summary>
        /// Calculate the value of all scrap for the passed scope except ignored items
        /// </summary>
        /// <param name="Ignored"></param>
        /// <param name="scope"></param>
        /// <returns>LootItem</returns>
        public static LootItem CalculateLootValue(List<string> Ignored, string scope)
        {
            List<GrabbableObject> scrapList = null;

            switch (scope)
            {
                case "Ship":
                    scrapList = Object.FindObjectsOfType<GrabbableObject>().Where(s => s.itemProperties.isScrap
                                                                                                && s.isInShipRoom
                                                                                                && s.isInElevator
                                                                                                && !Ignored.Contains(s.name)).ToList();
                    break;
                case "Moon":
                    scrapList = Object.FindObjectsOfType<GrabbableObject>().Where(s => s.itemProperties.isScrap
                                                                                                && !s.isInShipRoom
                                                                                                && !s.isInElevator
                                                                                                && !Ignored.Contains(s.name)).ToList();
                    break;
                case "All":
                    scrapList = Object.FindObjectsOfType<GrabbableObject>().Where(s => s.itemProperties.isScrap
                                                                                                && !Ignored.Contains(s.name)).ToList();
                    break;
            }

#if DEBUG
            Log.LogInfo($"[CalculateLootValue] Valid item count: {scrapList.Count}");
            Log.LogInfo("Calculating total ship scrap value.");
            scrapList.Do(s => Log.LogInfo($"{s.name} - ${s.scrapValue}"));
#endif
            Log.LogDebug("Calculating total ship scrap value.");
            scrapList.Do(s => Log.LogDebug($"{s.name} - ${s.scrapValue}"));

            return new LootItem
            {
                Value = scrapList.Sum(s => s.scrapValue),
                Count = scrapList.Count
            };

        }

        /// <summary>
        /// Convert a float to a human friendly version (1 = 1st, 2 = 2nd, etc)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static string ConvertToHumanFriendly(float number)
        {
            int intValue = (int)number;

            if (intValue % 100 >= 11 && intValue % 100 <= 13)
            {
                return $"{intValue}th";
            }

            switch (intValue % 10)
            {
                case 1:
                    return $"{intValue}st";
                case 2:
                    return $"{intValue}nd";
                case 3:
                    return $"{intValue}rd";
                default:
                    return $"{intValue}th";
            }
        }

        /// <summary>
        /// Replace all found variables in the passed string from the passed dictionary
        /// </summary>
        /// <param name="Item">String to replace items in</param>
        /// <param name="Substitutions">Replace data dictionary</param>
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

        #endregion
    }
}
