﻿using BepInEx;
using Figgle;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShipLootPlus.ShipLootPlus;
using static ShipLootPlus.Utils.Fonts;
using Object = UnityEngine.Object;

namespace ShipLootPlus.Utils
{
    internal class UiHelper
    {
        private static List<string> ItemsToIgnore => new List<string> { "ClipboardManual", "StickyNoteItem", "PlayerRagdoll", "RagdollGrabbableObject" };
        public static List<string> DataSubSet { get; set; }
        public static List<ShipLootPlusItem> UiElementList { get; set; }
        public static List<ShipLootPlusItem> ElementsToUpdate { get; set; }
        public static List<ReplacementData> DataPoints { get; set; }
        public static GameObject ContainerObject { get; set; }
        public static Vector3 SlpLocalScale { get; set; }
        public static Vector3 SlpLocalPos { get; set; }
        public static float timeLeftDisplay { get; set; }
        public static float timeLeftUpdate { get; set; }
        public static bool IsUpdating { get; set; }
        public static bool IsRefreshing { get; set; }
        public static bool IsDisplaying { get; set; }
        public static bool UpdateReady { get; set; }
        public static bool FirstTimeSync = false;

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
                    color = ConvertHexColor(ConfigSettings.LineColor.Value, ConfigSettings.LineAlpha.Value),
                    enabled = ConfigSettings.ShowLine.Value,
                    image = true
                },
                new ShipLootPlusItem
                {
                    name = "Line #1",
                    color = ConvertHexColor(ConfigSettings.LineOneColor.Value),
                    format = ConfigSettings.LineOneFormat.Value,
                    value = ConfigSettings.LineOneFormat.Value,
                    enabled = ConfigSettings.ShowLineOne.Value
                },
                new ShipLootPlusItem
                {
                    name = "Line #2",
                    color = ConvertHexColor(ConfigSettings.LineTwoColor.Value),
                    format = ConfigSettings.LineTwoFormat.Value,
                    value = ConfigSettings.LineTwoFormat.Value,
                    enabled = ConfigSettings.ShowLineTwo.Value
                },
                new ShipLootPlusItem
                {
                    name = "Line #3",
                    color = ConvertHexColor(ConfigSettings.LineThreeColor.Value),
                    format = ConfigSettings.LineThreeFormat.Value,
                    value = ConfigSettings.LineThreeFormat.Value,
                    enabled = ConfigSettings.ShowLineThree.Value
                }
            };
        }

        /// <summary>
        /// Update each UI element data from config values
        /// </summary>
        private static void UpdateObjectSettings()
        {
            Log.LogInfo($"[UpdateObjectSettings] Updating HUD element object data from config values");
            DataSubSet.Clear();

            ContainerObject.transform.localPosition = new Vector3(ConfigSettings.PosX.Value, ConfigSettings.PosY.Value, 0f);
            ContainerObject.transform.localScale = new Vector3(ConfigSettings.ScaleX.Value, ConfigSettings.ScaleY.Value, 0.5f);
            ContainerObject.transform.rotation = new Quaternion(0, 0 ,0 , 0);
            ContainerObject.transform.Rotate(0f, 0f, ConfigSettings.Rotation.Value);

            foreach (var obj in UiElementList)
            {
                switch (obj.name)
                {
                    case "LineImage":
                        obj.color = ConvertHexColor(ConfigSettings.LineColor.Value, ConfigSettings.LineAlpha.Value);
                        obj.enabled = ConfigSettings.ShowLine.Value;
                        break;
                    case "Line #1":
                        obj.color = ConvertHexColor(ConfigSettings.LineOneColor.Value);
                        obj.format = ConfigSettings.LineOneFormat.Value;
                        obj.enabled = ConfigSettings.ShowLineOne.Value;
                        break;
                    case "Line #2":
                        obj.color = ConvertHexColor(ConfigSettings.LineTwoColor.Value);
                        obj.format = ConfigSettings.LineTwoFormat.Value;
                        obj.enabled = ConfigSettings.ShowLineTwo.Value;
                        break;
                    case "Line #3":
                        obj.color = ConvertHexColor(ConfigSettings.LineThreeColor.Value);
                        obj.format = ConfigSettings.LineThreeFormat.Value;
                        obj.enabled = ConfigSettings.ShowLineThree.Value;
                        break;
                }

                if (!obj.image)
                {
                    obj.textMeshProUGui.alignment = ConfigSettings.TextAlignment.Value;
                    obj.textMeshProUGui.alpha = ConfigSettings.TextAlpha.Value;
                    obj.textMeshProUGui.fontSize = ConfigSettings.FontSize.Value;
                    obj.textMeshProUGui.characterSpacing = ConfigSettings.CharacterSpacing.Value;
                    obj.textMeshProUGui.wordSpacing = ConfigSettings.WordSpacing.Value;
                }

                SetDataSubSet(obj.format);
            }
            Log.LogWarning($"[DataSubset-Changed] Count: {DataSubSet.Count} => {string.Join(";", DataSubSet)}");
        }

        /// <summary>
        /// Create the UI elements
        /// </summary>
        public static void CreateUiElements()
        {
            if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[CreateUiElements] Creating UI objects");
            GameObject PlayerHUD = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD");
            GameObject valueCounter = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD/BottomMiddle/ValueCounter");
            if (!valueCounter)
            {
                Log.LogError("Failed to find ValueCounter object to copy!");
                return;
            }

            if (UiElementList == null)
            {
                UiElementList = LoadObjectList();
                DataSubSet.Clear();
                UiElementList.ForEach(s => SetDataSubSet(s.format));
                Log.LogWarning($"[DataSubset-Added] Count: {DataSubSet.Count} => {string.Join(";", DataSubSet)}");
            }
            IsUpdating = false;
            FirstTimeSync = false;

            Vector3 zero = new Vector3(0f, 0f, 0f);
            Vector3 VecLocPos = Vector3.zero;
            Quaternion QuatLocRot = Quaternion.identity;
            Vector3 VecPos = Vector3.zero;
            Quaternion QuatRot = Quaternion.identity;
            LogPos(valueCounter.gameObject, out VecPos, out QuatRot, out VecLocPos, out QuatLocRot);

            ContainerObject = Object.Instantiate(valueCounter.gameObject, valueCounter.transform.parent, false);
            ContainerObject.transform.SetParent(PlayerHUD.transform);
            ContainerObject.name = "ShipLootPlus";

            CanvasGroup ContainerObjectCanvasGroup = ContainerObject.AddComponent<CanvasGroup>();
            RectTransform ContainerRect = ContainerObject.GetComponent<RectTransform>();
            ContainerRect.pivot = new Vector2(0f, 0f);
            ContainerRect.offsetMax = new Vector2(0f, 0f);
            ContainerRect.offsetMin = new Vector2(0f, 0f);
            ContainerRect.anchorMax = new Vector2(1f, 1f);
            ContainerRect.anchorMin = new Vector2(1f, 0f);
            ContainerRect.anchoredPosition = new Vector2(50f, 50f);

            ContainerObject.transform.Translate(0f, 1f, 0f);
            ContainerObject.transform.localPosition = new Vector3(ConfigSettings.PosX.Value, ConfigSettings.PosY.Value, 0f);
            ContainerObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            ContainerObject.transform.Rotate(0f, 0f, ConfigSettings.Rotation.Value);

            TextMeshProUGUI textObj = ContainerObject.GetComponentInChildren<TextMeshProUGUI>();
            var def = Fonts.LcFontAssets.FirstOrDefault(a => a.Name == "Default");
            def.Asset = textObj.font;

            ContainerObject.transform.localScale = new Vector3(ConfigSettings.ScaleX.Value, ConfigSettings.ScaleY.Value, 0.5f);
            LogPos(ContainerObject, out VecPos, out QuatRot, out VecLocPos, out QuatLocRot);
#if DEBUG
            LogPos(ContainerObject.gameObject, out _, out _, out _, out _);
            Log.LogWarning("========================================");
            Log.LogInfo($"[CON] localScale: {ContainerObject.transform.localScale.x} / {ContainerObject.transform.localScale.y}");
            Log.LogWarning("========================================");
#endif
            Image imagObj = ContainerObject.GetComponentInChildren<Image>();
            ShipLootPlusItem ImageElem = UiElementList.Where(e => e.image).FirstOrDefault();
            if (ImageElem != null)
            {
                imagObj.name = ImageElem.name;
                imagObj.color = ImageElem.color;

                RectTransform imagTransform = imagObj.GetComponent<RectTransform>();
                float newWidth = imagTransform.rect.width / 0.15f;
                float newHeight = imagTransform.rect.height;
                imagTransform.sizeDelta = new Vector2(newWidth, newHeight);
                ImageElem.gameOjbect = imagObj.gameObject;
                imagObj.enabled = ImageElem.enabled;
                imagObj.transform.localPosition = zero;
                imagObj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                imagTransform.pivot = new Vector2(0f, 0f);
                imagTransform.anchorMax = new Vector2(0f, 0f);
                imagTransform.anchorMin = new Vector2(0f, 0f);
                imagTransform.anchoredPosition = new Vector2(30f, 38f);
#if DEBUG
                LogPos(imagObj.gameObject, out _, out _, out _, out _);
#endif
            }
#if DEBUG
            Log.LogWarning("========================================");
            Log.LogInfo($"[IMG] localScale: {imagObj.transform.localScale.x} / {imagObj.transform.localScale.y}");
            Log.LogWarning("========================================");
#endif
            GameObject slp = new GameObject
            {
                name = "SLP"
            };
            slp.transform.SetParent(ContainerObject.transform);
            slp.transform.position = ContainerObject.transform.position;
            slp.transform.localPosition = new Vector3(0f, -1f, 0f);
            RectTransform slpRect = slp.AddComponent<RectTransform>();
            slpRect.SetParent(slp.transform, true);

#if DEBUG
            LogPos(slp, out _, out _, out _, out _);
            Log.LogWarning("========================================");
            Log.LogInfo($"[SLP] localScale: {slp.transform.localScale.x} / {slp.transform.localScale.y}");
            Log.LogWarning("========================================");
#endif
            SlpLocalScale = new Vector3(90f, 90f, 90f);
            slp.transform.localScale = SlpLocalScale;
            slpRect.pivot = new Vector2(0f, 0f);
            slpRect.offsetMax = new Vector2(0f, 0f);
            slpRect.offsetMin = new Vector2(0f, 0f);
            slpRect.anchorMax = new Vector2(0f, 0f);
            slpRect.anchorMin = new Vector2(0f, 0f);
            slpRect.anchoredPosition = new Vector2(50f, 50f);
            SlpLocalPos = slp.transform.localPosition;
#if DEBUG
            Log.LogWarning("========================================");
            Log.LogInfo($"[SLP] localScale: {slp.transform.localScale.x} / {slp.transform.localScale.y}");
            Log.LogWarning("========================================");
#endif
            IEnumerable<ShipLootPlusItem> TextElements = UiElementList.Where(e => e.gameOjbect == null && !e.image);
            int EnabledTextElementsCount = TextElements.Count();
#if DEBUG
            Log.LogWarning("========================================");
            Log.LogInfo($"[ETE] Count: {EnabledTextElementsCount} - Count(): {TextElements.Count()}");
            Log.LogWarning("========================================");
#endif
            LcFont SelectedFont = Fonts.GetFont(ConfigSettings.SelectedFont.Value.GetValueString());
            foreach (ShipLootPlusItem TextElem in TextElements)
            {
                TextElem.gameOjbect = Object.Instantiate(textObj.gameObject, textObj.transform.parent);
                TextElem.gameOjbect.name = TextElem.name;
                TextElem.gameOjbect.transform.SetParent(slp.transform);
                TextElem.textMeshProUGui = TextElem.gameOjbect.GetComponentInChildren<TextMeshProUGUI>();
                TextElem.textMeshProUGui.text = TextElem.format;
                TextElem.textMeshProUGui.font = SelectedFont.Asset;
                TextElem.textMeshProUGui.alignment = ConfigSettings.TextAlignment.Value;
                TextElem.textMeshProUGui.alpha = ConfigSettings.TextAlpha.Value;
                TextElem.textMeshProUGui.fontSize = ConfigSettings.FontSize.Value;
                TextElem.textMeshProUGui.characterSpacing = ConfigSettings.CharacterSpacing.Value;
                TextElem.textMeshProUGui.wordSpacing = ConfigSettings.WordSpacing.Value;
                TextElem.textMeshProUGui.color = TextElem.color;
                TextElem.textMeshProUGui.overflowMode = TextOverflowModes.Ellipsis;
                TextElem.textMeshProUGui.enableWordWrapping = false;
                TextElem.textMeshProUGui.maxVisibleLines = 1;
                TextElem.textMeshProUGui.ForceMeshUpdate();
            }

            Object.Destroy(textObj.gameObject);
            ResizeAndPositionElements(slp);

            Log.LogWarning("Fixing ship scrap tags on first join...");
            List <GrabbableObject> scrapList = Object.FindObjectsOfType<GrabbableObject>().Where(s => s.itemProperties.isScrap).ToList();
            scrapList.ForEach(s =>
            {
                bool isInShipRoom = s.isInShipRoom;
                bool isInElevator = s.isInElevator;
                s.isInShipRoom = true;
                s.isInElevator = true;
                Log.LogInfo($"[{s.name}] isInShipRoom: {isInShipRoom} => {s.isInElevator} | isInElevator: {isInElevator} => {s.isInElevator}");
            });

            ElementsToUpdate = UiElementList.Where(e => !e.image && e.gameOjbect != null).ToList();
            RefreshElementValues();

            if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[CreateUiElements] Enabling UI objects");
            if (ConfigSettings.AlwaysShow.Value) ContainerObject.SetActive(true);
        }

        /// <summary>
        /// Size and position the UI elements
        /// </summary>
        /// <param name="GO"></param>
        public static void ResizeAndPositionElements(GameObject GO)
        {
#if DEBUG
            Log.LogWarning("========================================");
#endif
            IEnumerable<ShipLootPlusItem> ElementsToHide = UiElementList.Where(e => !e.enabled && !e.image);
#if DEBUG
            Log.LogInfo($"[ResizeAndPosition] Elements to hide: {ElementsToHide.Count()}");
#endif
            if (ElementsToHide.Count() > 0)
            {
#if DEBUG
                Log.LogInfo("----------");
#endif
                foreach (ShipLootPlusItem Elem in ElementsToHide)
                {
#if DEBUG
                    Log.LogInfo($"[ResizeAndPosition] Hiding => {Elem.name}");
#endif
                    if (ConfigSettings.AlwaysShow.Value) Elem.gameOjbect.SetActive(false);
                    Elem.textMeshProUGui.enabled = false;
                }
            }
#if DEBUG
            Log.LogWarning("========================================");
#endif
            IEnumerable<ShipLootPlusItem> ElementsToShow = UiElementList.Where(e => e.enabled && !e.image);
#if DEBUG
            Log.LogInfo($"[ResizeAndPosition] Elements to show: {ElementsToShow.Count()}");
#endif
            if (ElementsToShow.Count() > 0)
            {
#if DEBUG
                Log.LogInfo("----------");
#endif
                foreach (ShipLootPlusItem Elem in ElementsToShow)
                {
#if DEBUG
                    Log.LogInfo($"[ResizeAndPosition] Showing => {Elem.name}");
#endif
                    if (ConfigSettings.AlwaysShow.Value) Elem.gameOjbect.SetActive(true);
                    Elem.textMeshProUGui.enabled = true;
                }
            }
#if DEBUG
            Log.LogWarning("========================================");
#endif
            IEnumerable<TextMeshProUGUI> ElementsToResize = GO.GetComponentsInChildren<TextMeshProUGUI>().Where(t => t.enabled);
            int ElementsToShowCount = ElementsToResize.Count();
            Vector3 tempVect = new Vector3(0f, 0f, 0f);
            Vector3 lscale = SlpLocalScale;
            Vector3 lpos = SlpLocalPos;
            Vector2 defaultAnchor = new Vector2(0f, 0f);
            float xAncMult = 0.1f;
            float yAncMult = -0.175f;
            int count = 1;
            int acount = 0;
#if DEBUG
            Log.LogInfo($"[ResizeAndPosition] Elements to resize and position: {ElementsToShowCount}");
            Log.LogWarning("========================================");

            Log.LogMessage($"[GO] Name: {GO.name}");
            Log.LogMessage($"[GO] Scale: {GO.transform.localScale}");

            Log.LogInfo($"[ResizeAndPosition]  LSCA: {lscale}");
            Log.LogInfo($"[ResizeAndPosition]  LPOS: {lpos}");
            Log.LogInfo($"[ResizeAndPosition]  GOLS: {GO.transform.localScale}");
            Log.LogInfo($"[ResizeAndPosition]  GOLP: {GO.transform.localPosition}");
            Log.LogInfo("----------");
#endif
            float scaleMult = 30f;
            float scalePosXMult = 5f;
            float scalePosYMult = -18f;

            switch (ElementsToShowCount)
            {
                case 3:
                    lscale = SlpLocalScale;
                    lpos = SlpLocalPos;
                    break;
                case 2:
                    lscale = new Vector3(SlpLocalScale.x + scaleMult, SlpLocalScale.x + scaleMult, SlpLocalScale.x + scaleMult);
                    lpos = new Vector3(SlpLocalPos.x + scalePosXMult, SlpLocalPos.y + scalePosYMult, SlpLocalPos.z);
                    break;
                case 1:
                    lscale = new Vector3(SlpLocalScale.x + (scaleMult * 2), SlpLocalScale.x + (scaleMult * 2), SlpLocalScale.x + (scaleMult * 2));
                    lpos = new Vector3(SlpLocalPos.x + (scalePosXMult * 2), SlpLocalPos.y + (scalePosYMult * 2), SlpLocalPos.z);
                    break;
            }
#if DEBUG
            Log.LogInfo($"[ResizeAndPosition]> LSCA: {lscale}");
            Log.LogInfo($"[ResizeAndPosition]> LPOS: {lpos}");
#endif
            GO.transform.localScale = lscale;
            GO.transform.localPosition = lpos;
#if DEBUG
            Log.LogInfo($"[ResizeAndPosition]> GOLS: {GO.transform.localScale}");
            Log.LogInfo($"[ResizeAndPosition]> GOLP: {GO.transform.localPosition}");
            Log.LogWarning("========================================");
#endif
            foreach (TextMeshProUGUI TextElem in ElementsToResize)
            {
                RectTransform textTransform = TextElem.GetComponent<RectTransform>();
                Vector2 w = textTransform.sizeDelta;
#if DEBUG
                Log.LogInfo($"[ResizeAndPosition] Modifying => {TextElem.name}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  TELP: {TextElem.transform.localPosition}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  TMVT: {tempVect}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  RTSD: {textTransform.sizeDelta}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  W   : {w}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  AP  : {textTransform.anchoredPosition}");
                Log.LogInfo("----------");
#endif
                float xW = CalculateRectWidth(ElementsToShowCount);
#if DEBUG
                Log.LogWarning($"[SIZEDELTA] {xW} + {ConfigSettings.WidthAppend.Value}) = {xW + ConfigSettings.WidthAppend.Value}");
#endif
                xW += ConfigSettings.WidthAppend.Value;
                w = new Vector2(xW, textTransform.sizeDelta.y);

                TextElem.transform.localPosition = tempVect;
                textTransform.pivot = new Vector2(0f, 0f);
                textTransform.sizeDelta = w;

                float xt = defaultAnchor.x + (xAncMult * acount);
                float yt = defaultAnchor.y + (yAncMult * (acount));
#if DEBUG
                Log.LogWarning($"[ANCHOR] {defaultAnchor.x} + ({xAncMult} * {acount}) = {xt}");
                Log.LogWarning($"[ANCHOR] {defaultAnchor.y} + ({yAncMult} * {acount}) = {yt}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> TELP: {TextElem.transform.localPosition}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> TMVT: {tempVect}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> RTSD: {textTransform.sizeDelta}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> W   : {w}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  AP  : {textTransform.anchoredPosition}");
                Log.LogInfo("----------");
#endif
                textTransform.anchoredPosition = new Vector2(xt, yt);
                count++;
                acount++;
            }
#if DEBUG
            Log.LogInfo($"[ResizeAndPosition] Child objects changed: {ElementsToShowCount}");
            Log.LogWarning("========================================");
#endif
        }

        /// <summary>
        /// Get the float width for the scaled rect
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static float CalculateRectWidth(int count)
        {
            float minWidth = 300f;
            float maxWidth = 440f;
            float width;

            if (count == 1)
            {
                width = minWidth;
            }
            else if (count == 2)
            {
                width = (maxWidth + minWidth) / 2f;
            }
            else
            {
                width = maxWidth;
            }
#if DEBUG
            Log.LogInfo($"count: {count}");
            Log.LogInfo($"minWidth: {minWidth} / maxWidth: {maxWidth}");
            Log.LogInfo($"ret width: {width}");
#endif
            return width;
        }

        /// <summary>
        /// Resets all element settings when specific config items are refreshed
        /// </summary>
        public static void ResetUiElements()
        {
            if (UiElementList == null || ContainerObject == null) return;
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
            LcFont SelectedFont = Fonts.GetFont(ConfigSettings.SelectedFont.Value.GetValueString());

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
                    textItem.font = SelectedFont.Asset;
                    textItem.alignment = ConfigSettings.TextAlignment.Value;
                    textItem.alpha = ConfigSettings.TextAlpha.Value;
                    textItem.fontSize = ConfigSettings.FontSize.Value;
                    textItem.characterSpacing = ConfigSettings.CharacterSpacing.Value;
                    textItem.wordSpacing = ConfigSettings.WordSpacing.Value;
                    textItem.ForceMeshUpdate();
                }
            }

            ResizeAndPositionElements(slpObj.gameObject);
            ElementsToUpdate = UiElementList.Where(e => !e.image && e.gameOjbect != null).ToList();
            RefreshElementValues();
        }

        /// <summary>
        /// Creates a data subset to limit only refreshing data values which exist in the config
        /// </summary>
        /// <param name="LineItem"></param>
        public static void SetDataSubSet(string LineItem)
        {
            if (string.IsNullOrEmpty(LineItem)) return;
            string pattern = @"%([^%]+)%";
            Regex regex = new Regex(pattern);

            foreach (Match match in regex.Matches(LineItem))
            {
                string item = match.Groups[1].Value;
                string sanFormat = Regex.Replace(item, @":.*", "");
                if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[SetDataSubSet] New Data Subset => {item} => {sanFormat}");
                if (!DataSubSet.Contains(sanFormat)) DataSubSet.Add(sanFormat);
            }
        }

        /// <summary>
        /// Refresh actual data values
        /// </summary>
        public static void RefreshElementValues()
        {
            if (ConfigSettings.DebugMode.Value)
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase callingMethod = stackTrace.GetFrames().Skip(1).Select(frame => frame.GetMethod()).FirstOrDefault();
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Caller => {callingMethod}");
            }

            if (!IsUpdating) { IsUpdating = true; }
            if (UiElementList == null || ContainerObject == null)
            {
                if (ConfigSettings.DebugMode.Value) Log.LogWarning($"[RefreshElementValues:{IsUpdating}] UiElementList or ContainerObject is NULL!");
                return;
            }
            
            if (ElementsToUpdate == null || ElementsToUpdate.Count() <= 0)
            {
                if (ConfigSettings.DebugMode.Value) Log.LogWarning($"[RefreshElementValues:{IsUpdating}] ElementsToUpdate is NULL or ZERO!");
                return;
            }

            if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Elements to update: {ElementsToUpdate.Count()}");

            //TODO: Add a datapoint to show what your profit would be like if you collected all scrap and just scrap on the ship
            //      Take into account the company buying rate as another option
            //TODO: Investigate a feature to tell you what scrap on the planet you need to get to make quota (like BetterItemScan)

            TimeOfDay tod = TimeOfDay.Instance;
            StartOfRound sor = StartOfRound.Instance;
            string ColorPattern = "<color={0}>{1}</color>";
            LevelWeatherType? currentWeatherEnum = sor.currentLevel?.currentWeather;
            string currentWeather = currentWeatherEnum.ToString();
            if (currentWeatherEnum == LevelWeatherType.None && !string.IsNullOrEmpty(ConfigSettings.WeatherNoneReplacement.Value))
            {
                currentWeather = ConfigSettings.WeatherNoneReplacement.Value;
            }

            if (ConfigSettings.DebugMode.Value)
            {
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Weather list");
                foreach (var item in Enum.GetValues(typeof(LevelWeatherType)))
                {
                    Log.LogInfo($"[Weather] {item}");
                }

                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Current String: {currentWeather}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Current Enum  : {currentWeatherEnum}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Current Type  : {currentWeatherEnum.GetType()}");
                Log.LogMessage("");
            }

            string weatherColorCode = "#ffffffff";
            if (ConfigSettings.WeatherUseColors.Value)
            {
                switch (currentWeatherEnum)
                {
                    case LevelWeatherType.DustClouds:
                        weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorDustClouds.Value, "ff");
                        break;
                    case LevelWeatherType.Eclipsed:
                        weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorEclipsed.Value, "ff");
                        break;
                    case LevelWeatherType.Flooded:
                        weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorFlooded.Value, "ff");
                        break;
                    case LevelWeatherType.Foggy:
                        weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorFoggy.Value, "ff");
                        break;
                    case LevelWeatherType.None:
                        weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorNone.Value, "ff");
                        break;
                    case LevelWeatherType.Rainy:
                        weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorRainy.Value, "ff");
                        break;
                    case LevelWeatherType.Stormy:
                        weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorStormy.Value, "ff");
                        break;
                }

                //Custom weather types
                if (Regex.IsMatch(currentWeather, @"^Hell$", RegexOptions.IgnoreCase))
                {
                    weatherColorCode = SanitizeHexColorString(ConfigSettings.WeatherColorHell.Value, "ff");
                }

                currentWeather = string.Format(ColorPattern, weatherColorCode, currentWeather);
            }
            
            string currentMoon = sor.currentLevel.PlanetName;
            if (ConfigSettings.DebugMode.Value)
            {
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] currentMoon: {currentMoon}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] MoonShowFullName: {ConfigSettings.MoonShowFullName.Value}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] MoonReplaceCompany: {ConfigSettings.MoonReplaceCompany.Value}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] MoonCompanyReplacement: {ConfigSettings.MoonCompanyReplacement.Value}");
                Log.LogMessage("");
            }

            if (!ConfigSettings.MoonShowFullName.Value)
            {
                currentMoon = Regex.Replace(currentMoon, @"^\d{1,} ?", "", RegexOptions.IgnoreCase);
            }

            if (ConfigSettings.MoonReplaceCompany.Value)
            {
                if (Regex.IsMatch(currentMoon, "Gordion", RegexOptions.IgnoreCase)) { currentMoon = ConfigSettings.MoonCompanyReplacement.Value; }
            }

            int count = 1;
            if (ConfigSettings.DebugMode.Value)
            {
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Current Data Point values:");
                foreach (ReplacementData item in DataPoints)
                {
                    Log.LogInfo($"[DataPoint #{count:D2}]  {item.Pattern} => {item.Value}");
                    count++;
                }
                Log.LogMessage("");
            }

            string deadlineDueText = tod.daysUntilDeadline.ToString();
            string deadlineColorCode = "#ffffffff";

            if (ConfigSettings.DeadlineReplaceZero.Value && tod.daysUntilDeadline <= 0)
            {
                deadlineDueText = ConfigSettings.DeadlineLastDay.Value;
            }

            if (ConfigSettings.DeadlineUseColors.Value)
            {
                if (tod.daysUntilDeadline >= 2)
                {
                    deadlineColorCode = SanitizeHexColorString(ConfigSettings.DeadlineTwoColor.Value, "ff");
                }
                else if (tod.daysUntilDeadline == 1)
                {
                    deadlineColorCode = SanitizeHexColorString(ConfigSettings.DeadlineOneColor.Value, "ff");
                }
                else
                {
                    deadlineColorCode = SanitizeHexColorString(ConfigSettings.DeadlineZeroColor.Value, "ff");
                }

                deadlineDueText = string.Format(ColorPattern, deadlineColorCode, deadlineDueText);
            }

            LootItem shipLootValue = CalculateLootValue(ItemsToIgnore, "Ship");
            LootItem moonLootValue = CalculateLootValue(ItemsToIgnore, "Moon");
            LootItem allLootValue = CalculateLootValue(ItemsToIgnore, "All");
            LootItem invLootValue = CalculateLootValue(ItemsToIgnore, "Inv");
            int companyRate = ((int)(sor.companyBuyingRate * 100f));
            double profitValue = Math.Round((shipLootValue.Value - tod.quotaFulfilled) * sor.companyBuyingRate);
            if (shipLootValue.Value <= 0) { profitValue = 0; }

            if (ConfigSettings.DebugMode.Value)
            {
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] shipLootValue: {shipLootValue.Value}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] moonLootValue: {moonLootValue.Value}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] allLootValue: {allLootValue.Value}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] invLootValue: {invLootValue.Value}");
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] deadlineDueColorText: {deadlineDueText} => {tod.daysUntilDeadline}");
                Log.LogMessage("");
            }

            Parallel.ForEach(DataPoints, (dataPoint) =>
            {
                switch (dataPoint.Pattern)
                {
                    case string s when Regex.IsMatch(s, @"ShipLootValue", RegexOptions.IgnoreCase):
                        dataPoint.Value = shipLootValue.Value.ToString("F0");
                        break;
                    case string s when Regex.IsMatch(s, @"ShipLootCount", RegexOptions.IgnoreCase):
                        dataPoint.Value = shipLootValue.Count.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"MoonLootValue", RegexOptions.IgnoreCase):
                        dataPoint.Value = moonLootValue.Value.ToString("F0");
                        break;
                    case string s when Regex.IsMatch(s, @"MoonLootCount", RegexOptions.IgnoreCase):
                        dataPoint.Value = moonLootValue.Count.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"AllLootValue", RegexOptions.IgnoreCase):
                        dataPoint.Value = allLootValue.Value.ToString("F0");
                        break;
                    case string s when Regex.IsMatch(s, @"AllLootCount", RegexOptions.IgnoreCase):
                        dataPoint.Value = allLootValue.Count.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"InventoryLootValue", RegexOptions.IgnoreCase):
                        dataPoint.Value = invLootValue.Value.ToString("F0");
                        break;
                    case string s when Regex.IsMatch(s, @"InventoryLootCount", RegexOptions.IgnoreCase):
                        dataPoint.Value = invLootValue.Count.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"FulfilledValue", RegexOptions.IgnoreCase):
                        dataPoint.Value = tod.quotaFulfilled.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"QuotaValue", RegexOptions.IgnoreCase):
                        dataPoint.Value = tod.profitQuota.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"CompanyRate", RegexOptions.IgnoreCase):
                        dataPoint.Value = companyRate.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"ExpectedProfit", RegexOptions.IgnoreCase):
                        dataPoint.Value = profitValue.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"Deadline.$", RegexOptions.IgnoreCase):
                        dataPoint.Value = deadlineDueText;
                        break;
                    case string s when Regex.IsMatch(s, @"DayNumber.$", RegexOptions.IgnoreCase):
                        dataPoint.Value = sor.gameStats.daysSpent.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"DayNumberHuman", RegexOptions.IgnoreCase):
                        dataPoint.Value = ConvertToHumanFriendly(sor.gameStats.daysSpent + 1f);
                        break;
                    case string s when Regex.IsMatch(s, @"Weather.$", RegexOptions.IgnoreCase):
                        dataPoint.Value = currentWeather;
                        break;
                    case string s when Regex.IsMatch(s, @"MoonName", RegexOptions.IgnoreCase):
                        dataPoint.Value = currentMoon;
                        break;
                }
            });

            count = 1;
            if (ConfigSettings.DebugMode.Value)
            {
                Log.LogMessage($"[RefreshElementValues:{IsUpdating}] New Data Point values:");
                foreach (ReplacementData item in DataPoints)
                {
                    Log.LogInfo($"[DataPoint #{count:D2}]> {item.Pattern} => {item.Value}");
                    count++;
                }
                Log.LogMessage("");
            }

            Parallel.ForEach(ElementsToUpdate, (slpi) =>
            {
                if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[RefreshElementValues:{IsUpdating}] Updating value for element: {slpi.name}");
                string textContent = ReplaceValues(slpi.format, DataPoints.Where(d => DataSubSet.Contains(d.Name)).ToList());
                if (ConfigSettings.AllCaps.Value) { textContent = textContent.ToUpper(); }
                slpi.value = textContent;
            });

            IsUpdating = false;
            UpdateReady = true;
        }

        /// <summary>
        /// Update GameObjects from main thread
        /// </summary>
        public static void UpdateUiObjects()
        {
            if (!UpdateReady && ContainerObject == null) return;
            foreach (ShipLootPlusItem item in ElementsToUpdate.Where(g => g.gameOjbect != null))
            {
                if (item.textMeshProUGui.text != item.value)
                {
                    if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[UpdateUiObjects] Updating text for element: {item.name}");
                    item.textMeshProUGui.text = item.value;
                }
            }
            UpdateReady = false;
        }

        #endregion

        #region Method Helpers

        /// <summary>
        /// Log UI element positions and return the values
        /// </summary>
        /// <param name="InObj"></param>
        /// <param name="VecPos"></param>
        /// <param name="QuatRot"></param>
        /// <param name="VecLocPos"></param>
        /// <param name="QuatLocRot"></param>
        public static void LogPos(GameObject InObj, out Vector3 VecPos, out Quaternion QuatRot, out Vector3 VecLocPos, out Quaternion QuatLocRot)
        {
            VecLocPos = Vector3.zero;
            QuatLocRot = Quaternion.identity;
            VecPos = Vector3.zero;
            QuatRot = Quaternion.identity;
            InObj.transform.GetPositionAndRotation(out VecPos, out QuatRot);
            InObj.transform.GetLocalPositionAndRotation(out VecLocPos, out QuatLocRot);
#if DEBUG
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
        /// Ensures the passed hex color string is in the right format
        /// </summary>
        /// <param name="hColor"></param>
        /// <param name="alpha"></param>
        /// <returns>string</returns>
        public static string SanitizeHexColorString(string hColor, string alpha = "")
        {
            if (!Regex.IsMatch(hColor, "^#")) { hColor = string.Concat("#", hColor); }
            return string.Concat(hColor.Substring(0, 7), alpha);
        }

        /// <summary>
        /// Convert a HTML hex color code to a Color object
        /// </summary>
        /// <param name="hColor"></param>
        /// <param name="alpha"></param>
        /// <returns>Color</returns>
        public static Color ConvertHexColor(string hColor, float alpha = 0.95f)
        {
            hColor = SanitizeHexColorString(hColor);
            if (ColorUtility.TryParseHtmlString(hColor, out Color elemColor))
            {
                return new Color(elemColor.r, elemColor.g, elemColor.b, alpha);
            }

            return new Color(25, 213, 108, alpha);
        }

        /// <summary>
        /// Toggle UI based on events
        /// </summary>
        /// <param name="Timed"></param>
        public static void TryToggleUi(bool Timed = false)
        {
            if (ConfigSettings.AlwaysShow.Value && !Timed)
            {
#if DEBUG
                //Log.LogWarning($"AlwaysShow => {ConfigSettings.AlwaysShow.Value} | Timed => {Timed}");
#endif
                if (StartOfRound.Instance.inShipPhase)
                {
                    ContainerObject.SetActive(true);
                    return;
                }

                if (GameNetworkManager.Instance.localPlayerController.isInsideFactory)
                {
#if DEBUG
                    //Log.LogInfo($"[Inside] Show ShipLootPlus? {ConfigSettings.AllowInside.Value}");
#endif
                    ContainerObject.SetActive(ConfigSettings.AllowInside.Value);
                }
                else if (!GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom && !GameNetworkManager.Instance.localPlayerController.isInsideFactory)
                {
#if DEBUG
                    //Log.LogInfo($"[Outside] Show ShipLootPlus? {ConfigSettings.AllowOutside.Value}");
#endif
                    ContainerObject.SetActive(ConfigSettings.AllowOutside.Value);
                }
                else if (GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
                {
#if DEBUG
                    //Log.LogInfo("[Ship] Show ShipLootPlus");
#endif
                    ContainerObject.SetActive(true);
                }
            }
            else if (!ConfigSettings.AlwaysShow.Value && Timed)
            {
#if DEBUG
                Log.LogWarning($"AlwaysShow => {ConfigSettings.AlwaysShow.Value} | Timed => {Timed}");
#endif
                bool CanShow = true;
                if (!StartOfRound.Instance.inShipPhase)
                {
                    CanShow = false;

                    if (ConfigSettings.AllowInside.Value && GameNetworkManager.Instance.localPlayerController.isInsideFactory)
                    {
#if DEBUG
                        Log.LogInfo($"[Inside] Show ShipLootPlus? {ConfigSettings.AllowInside.Value}");
#endif
                        CanShow = true;
                    }
                    else if (ConfigSettings.AllowOutside.Value && !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom && !GameNetworkManager.Instance.localPlayerController.isInsideFactory)
                    {
#if DEBUG
                        Log.LogInfo($"[Outside] Show ShipLootPlus? {ConfigSettings.AllowOutside.Value}");
#endif
                        CanShow = true;
                    }
                    else if (GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
                    {
                        CanShow = true;
                    }
                }

                if (!IsDisplaying && CanShow)
                {
#if DEBUG
                    Log.LogWarning("Invoking DisplayDataCoroutine!");
#endif
                    GameNetworkManager.Instance.StartCoroutine(DisplayDataCoroutine());
                }
                else
                {
#if DEBUG
                    Log.LogWarning("Skipping DisplayDataCoroutine...");
#endif
                }
            }
        }

        /// <summary>
        /// Display the new elements for a time and then hide them
        /// </summary>
        /// <returns></returns>
        public static IEnumerator DisplayDataCoroutine()
        {
            if (!IsDisplaying) { IsDisplaying = true; }

            timeLeftDisplay = ConfigSettings.DisplayDuration.Value;
            if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[DisplayDataCoroutine:{IsDisplaying}] Showing UI => timeLeftDisplay: {timeLeftDisplay} - {ConfigSettings.DisplayDuration.Value}");
            bool hadError = false;
            try { ContainerObject.SetActive(true); }
            catch
            {
                Log.LogWarning("Unable to show UI - Likely because we are not in a lobby anymore => Resetting states...");
                hadError = true;
            }

            while (timeLeftDisplay > 0f)
            {
                if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[DisplayDataCoroutine:{IsDisplaying}]> timeLeftDisplay: {timeLeftDisplay} - {ConfigSettings.DisplayDuration.Value}");
                float time = timeLeftDisplay;
                timeLeftDisplay = 0f;
                yield return new WaitForSeconds(time);
            }

            if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[DisplayDataCoroutine:{IsDisplaying}] Hiding UI => timeLeftDisplay: {timeLeftDisplay} - {ConfigSettings.DisplayDuration.Value}");
            if (!hadError)
            {
                try { ContainerObject.SetActive(false); }
                catch
                {
                    Log.LogWarning("Unable to hide UI - Likely because we are not in a lobby anymore => Resetting states...");
                }
            }

            IsDisplaying = false;
        }

        /// <summary>
        /// Rate limit updating all data points
        /// </summary>
        /// <returns></returns>
        public static IEnumerator UpdateDatapoints(float delay = 0f, int loop = 1)
        {
            if (!IsRefreshing) { IsRefreshing = true; }
            if (delay > 0f)
            {
                if (ConfigSettings.DebugMode.Value) Log.LogInfo($"[UpdateDatapoints] Delaying datapoint refresh for [ {delay} ] seconds");
                yield return new WaitForSeconds(delay);
            }
            timeLeftUpdate = 0.5f;

            if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[UpdateDatapoints:{IsRefreshing}] Callers: {GetStackTraceInfo("Patcher")} => timeLeftUpdate: {timeLeftUpdate} => Loop: {loop}");
            for (int i = 0; i < loop; i++)
            {
                RefreshElementValues();
                if (loop > 1) yield return new WaitForSeconds(0.5f);
            }

            while (timeLeftUpdate > 0f)
            {
                if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[UpdateDatapoints:{IsRefreshing}] Refreshing => timeLeftUpdate: {timeLeftUpdate}");
                float time = timeLeftUpdate;
                timeLeftUpdate = 0f;
                yield return new WaitForSeconds(time);
            }

            if (ConfigSettings.DebugMode.Value) Log.LogMessage($"[UpdateDatapoints:{IsRefreshing}] Refresh complete => timeLeftUpdate: {timeLeftUpdate}");
            IsRefreshing = false;
        }

        /// <summary>
        /// Gets the stack trace caller that matches the passed regex pattern
        /// </summary>
        /// <param name="targetClassName"></param>
        /// <returns></returns>
        static string GetStackTraceInfo(string targetClassName)
        {
            StackTrace stackTrace = new StackTrace();

            Regex classRegex = new Regex(targetClassName);

            var matchingFrame = stackTrace.GetFrames()
                .Skip(1)
                .FirstOrDefault(frame =>
                {
                    MethodBase method = frame.GetMethod();
                    string className = method.DeclaringType?.Name ?? "UnknownClass";
                    return classRegex.IsMatch(className);
                });

            if (matchingFrame != null)
            {
                MethodBase method = matchingFrame.GetMethod();
                string methodName = method.Name;
                int lineNumber = matchingFrame.GetFileLineNumber();
                string frameInfo = $"{targetClassName}.{methodName}() in {matchingFrame.GetFileName()}:{lineNumber}";
                return frameInfo;
            }
            else
            {
                return $"No matching frame found for class {targetClassName}";
            }
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
            LootItem lootItem = new LootItem { Count = 0, Value = 0 };
            GrabbableObject[] FoundObjects = Object.FindObjectsOfType<GrabbableObject>();
            if (FoundObjects == null || FoundObjects.Length == 0)
            {
                if (ConfigSettings.DebugMode.Value) Log.LogWarning("[CalculateLootValue] GrabbableObject list was null => Returning zeros...");
                return lootItem;
            }
            FoundObjects = FoundObjects.Where(s => s != null && s.itemProperties != null).ToArray();
            FoundObjects = FoundObjects.Where(s => s.itemProperties.isScrap
                                                && !IsIgnored(Ignored, s.name)).ToArray();

            switch (scope)
            {
                case "All":
                    scrapList = FoundObjects.ToList();
                    break;
                case "Ship":
                    scrapList = FoundObjects.Where(s => s.isInShipRoom
                                                     && s.isInElevator).ToList();
                    break;
                case "Moon":
                    scrapList = FoundObjects.Where(s => !s.isInShipRoom
                                                     && !s.isInElevator).ToList();
                    break;
                
                case "Inv":
                    scrapList = FoundObjects.Where(s => (s.isHeld || s.isPocketed)
                                                     && s.playerHeldBy.IsLocalPlayer).ToList();
                    break;
            }

            if (scrapList == null)
            {
                if (ConfigSettings.DebugMode.Value) Log.LogWarning($"[CalculateLootValue] {scope} GrabbableObject list was null => Returning zeros...");
                return lootItem;
            }

            lootItem.Count = scrapList.Count;
            lootItem.Value = scrapList.Sum(s => s.scrapValue);

            if (ConfigSettings.DebugMode.Value)
            {
                Log.LogMessage($"[CalculateLootValue] Calculating total {scope} scrap value => Valid item count: {lootItem.Count} => Value: ${lootItem.Value}");
                scrapList.ForEach(s =>
                {
                    if (s != null) Log.LogMessage($"[CalculateLootValue] {s.name} - ${s.scrapValue}");
                    else Log.LogMessage($"[CalculateLootValue] Item was NULL - Skipping...");
                });
                Log.LogMessage("");
            }
            
            return lootItem;
        }

        /// <summary>
        /// Check if the passed item exists in the passed list using regex
        /// </summary>
        /// <param name="Ignored"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static bool IsIgnored(List<string> Ignored, string Item)
        {
#if DEBUG
            Log.LogInfo($"Testing [{Item}] against [{string.Join("; ", Ignored)}]");
#endif
            string list = Ignored.FirstOrDefault(i => Regex.IsMatch(Item, i, RegexOptions.IgnoreCase));
            if (list != null)
            {
#if DEBUG
                Log.LogInfo($"FOUND: {Item} - Ignoring...");
#endif
                return true;
            }
#if DEBUG
            Log.LogInfo($"NO FIND: {Item} - Adding...");
#endif
            return false;
        }

        /// <summary>
        /// Convert a float to a human friendly version (1 = 1st, 2 = 2nd, etc)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static string ConvertToHumanFriendly(float number)
        {
            int intValue = (int)number;

            if (intValue <= 0) { return "Zero"; }

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
        /// Replaces found tokens in the passed string with the specific data for that token
        /// </summary>
        /// <param name="Line"></param>
        /// <param name="Data"></param>
        /// <returns>string</returns>
        public static string ReplaceValues(string Line, List<ReplacementData> Data)
        {
            string stReturn = Line;
            string pattern = @"%([^%]+)%";
            Regex regex = new Regex(pattern);

            foreach (Match match in regex.Matches(stReturn))
            {
                if (!match.Success) continue;
                string dataPoint = match.Groups[1].Value;
                int trunc = GetTrunc(dataPoint);
                string sanDataPoint = Regex.Replace(dataPoint, ":.*", "").Trim();
                string rxPattern = Regex.Escape($"%{sanDataPoint}%");

                ReplacementData item = Data.FirstOrDefault(v => Regex.IsMatch(v.Pattern, rxPattern, RegexOptions.IgnoreCase));
                if (item == null) continue;

                string replaceValue = item.Value;
                if (trunc > 0 && trunc < replaceValue.Length)
                {
                    replaceValue = replaceValue.Substring(0, trunc);
                }

                string rxPattern2 = Regex.Escape($"%{dataPoint}%");
                stReturn = Regex.Replace(stReturn, rxPattern2, replaceValue, RegexOptions.IgnoreCase);
#if DEBUG
                Log.LogInfo($"[{dataPoint.ToUpper()}] Replacing [ {rxPattern2} ] in [ {Line} ] with [ {replaceValue} ] => [{stReturn}]");
#endif
            }

            return stReturn;
        }

        /// <summary>
        /// Gets the specified truncate value if it existed in the datapoint
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int GetTrunc(string item)
        {
            if (!Regex.IsMatch(item, ":")) return 0;
            int ret = 0;
            string pattern = @"(?<=:).*";
            Regex regex = new Regex(pattern);
            Match m = regex.Match(item);
            if (m.Success)
            {
                string san = Regex.Replace(m.Value, @"\D", "");
                ret = Convert.ToInt32(san);
            }

            return ret;
        }

#endregion
    }
}
