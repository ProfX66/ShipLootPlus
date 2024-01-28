using Figgle;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        public static List<string> DataSubSet { get; set; }
        public static List<ShipLootPlusItem> UiElementList { get; set; }
        public static List<ReplacementData> DataPoints { get; set; }
        public static GameObject ContainerObject { get; set; }
        public static float timeLeftDisplay { get; set; }
        public static bool IsUpdating { get; set; }
        public static bool IsDisplaying { get; set; }

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
                    name = "Line #1",
                    color = ConvertHexColor(ConfigSettings.LineOneColor.Value),
                    format = ConfigSettings.LineOneFormat.Value,
                    enabled = ConfigSettings.ShowLineOne.Value
                },
                new ShipLootPlusItem
                {
                    name = "Line #2",
                    color = ConvertHexColor(ConfigSettings.LineTwoColor.Value),
                    format = ConfigSettings.LineTwoFormat.Value,
                    enabled = ConfigSettings.ShowLineTwo.Value
                },
                new ShipLootPlusItem
                {
                    name = "Line #3",
                    color = ConvertHexColor(ConfigSettings.LineThreeColor.Value),
                    format = ConfigSettings.LineThreeFormat.Value,
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

            foreach (var obj in UiElementList)
            {
                switch (obj.name)
                {
                    case "LineImage":
                        obj.color = ConvertHexColor(ConfigSettings.LineColor.Value, 0.75f);
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

                SetDataSubSet(obj.format);
            }
            Log.LogWarning($"[DataSubset-Changed] Count: {DataSubSet.Count} => {string.Join(";", DataSubSet)}");
        }

        /// <summary>
        /// Create the UI elements
        /// </summary>
        public static void CreateUiElements()
        {
#if DEBUG
            Log.LogWarning(string.Format("\n{0}", FiggleFonts.Doom.Render("Adding Objects")));
#endif
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

            Vector3 zero = new Vector3(0f, 0f, 0f);
            Vector3 VecLocPos = Vector3.zero;
            Quaternion QuatLocRot = Quaternion.identity;
            Vector3 VecPos = Vector3.zero;
            Quaternion QuatRot = Quaternion.identity;
            LogPos(valueCounter.gameObject, out VecPos, out QuatRot, out VecLocPos, out QuatLocRot);

            ContainerObject = Object.Instantiate(valueCounter.gameObject, valueCounter.transform.parent, false);
            ContainerObject.name = "ShipLootPlus";
            ContainerObject.transform.Translate(0f, 1f, 0f);
            ContainerObject.transform.localPosition = new Vector3(300f, -81.5f, 0f);
            ContainerObject.transform.Rotate(QuatRot.x, QuatRot.y, 358f);
            TextMeshProUGUI textObj = ContainerObject.GetComponentInChildren<TextMeshProUGUI>();
            ContainerObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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
            slp.transform.localPosition = new Vector3(100f, 2f, 0f);
#if DEBUG
            LogPos(slp, out _, out _, out _, out _);
            Log.LogWarning("========================================");
            Log.LogInfo($"[SLP] localScale: {slp.transform.localScale.x} / {slp.transform.localScale.y}");
            Log.LogWarning("========================================");
#endif
            slp.transform.localScale = new Vector3(90f, 90f, 90f);
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
            foreach (ShipLootPlusItem TextElem in TextElements)
            {
                TextElem.gameOjbect = Object.Instantiate(textObj.gameObject, textObj.transform.parent);
                TextElem.gameOjbect.name = TextElem.name;
                TextElem.gameOjbect.transform.SetParent(slp.transform);
                TextElem.textMeshProUGui = TextElem.gameOjbect.GetComponentInChildren<TextMeshProUGUI>();
                TextElem.textMeshProUGui.text = TextElem.format;
                TextElem.textMeshProUGui.characterSpacing = -4f;
                TextElem.textMeshProUGui.color = TextElem.color;
                TextElem.textMeshProUGui.overflowMode = TextOverflowModes.Ellipsis;
                TextElem.textMeshProUGui.enableWordWrapping = false;
                TextElem.textMeshProUGui.maxVisibleLines = 1;
                TextElem.textMeshProUGui.ForceMeshUpdate();
            }

            Object.Destroy(textObj.gameObject);
            ResizeAndPositionElements(slp);
            RefreshElementValues();
#if DEBUG
            Log.LogWarning(string.Format("\n{0}", FiggleFonts.Doom.Render("Showing Objects")));
#endif
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
                    Log.LogInfo($"[ResizeAndPosition] Hiding => {Elem.name}");
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
                    Log.LogInfo($"[ResizeAndPosition] Showing => {Elem.name}");
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
            Vector3 lscale = new Vector3(90f, 90f, 90f);
            Vector3 lpos = new Vector3(100f, 2f, 0f);
            float xMult = 0.1f;
            float yMult = -0.16f;
            int count = 1;
#if DEBUG
            Log.LogInfo($"[ResizeAndPosition] Elements to resize and position: {ElementsToShowCount}");
            Log.LogWarning("========================================");
#endif

            foreach (TextMeshProUGUI TextElem in ElementsToResize)
            {
                RectTransform textTransform = TextElem.GetComponent<RectTransform>();
                Vector2 w = new Vector2(330f, textTransform.sizeDelta.y);

                Log.LogInfo($"[ResizeAndPosition] Modifying => {TextElem.name}");
#if DEBUG
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  TELP: {TextElem.transform.localPosition}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  TMVT: {tempVect}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  RTSD: {textTransform.sizeDelta}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]  W   : {w}");
                Log.LogInfo("----------");
#endif

                switch (ElementsToShowCount)
                {
                    case 3:
                        w = new Vector2(330f, textTransform.sizeDelta.y);
                        break;
                    case 2:
                        w = new Vector2(250f, textTransform.sizeDelta.y);
                        break;
                    case 1:
                        w = new Vector2(200f, textTransform.sizeDelta.y);
                        break;
                }

                TextElem.transform.localPosition = tempVect;
                tempVect = new Vector3(tempVect.x + xMult, tempVect.y + yMult, 0f);
                textTransform.sizeDelta = w;
#if DEBUG
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> TELP: {TextElem.transform.localPosition}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> TMVT: {tempVect}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> RTSD: {textTransform.sizeDelta}");
                Log.LogInfo($"[ResizeAndPosition #{count:D2}]> W   : {w}");
                Log.LogInfo("----------");
#endif
                count++;
            }

            Log.LogInfo($"[ResizeAndPosition] Child objects changed: {ElementsToShowCount}");
#if DEBUG
            Log.LogWarning("========================================");
            Log.LogInfo($"[ResizeAndPosition]  LSCA: {lscale}");
            Log.LogInfo($"[ResizeAndPosition]  LPOS: {lpos}");
            Log.LogInfo($"[ResizeAndPosition]  GOLS: {GO.transform.localScale}");
            Log.LogInfo($"[ResizeAndPosition]  GOLP: {GO.transform.localPosition}");
            Log.LogInfo("----------");
#endif

            switch (ElementsToShowCount)
            {
                case 2:
                    lscale = new Vector3(120f, 120f, 120f);
                    lpos = new Vector3(105f, -8f, 0f);
                    break;
                case 1:
                    lscale = new Vector3(150f, 150f, 150f);
                    lpos = new Vector3(110f, -18f, 0f);
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
        }

        /// <summary>
        /// Caclulate available screen space
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static float CalculateAvailableSpace(RectTransform element)
        {
            // Get the screen width
            float screenWidth = Screen.width;

            // Calculate the position of the UI element's left side in screen space
            float elementLeft = element.position.x - element.rect.width * element.lossyScale.x / 2;

            // Calculate the available space from the left side of the UI element to the left side of the screen
            float availableSpace = elementLeft;

            return availableSpace;
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

            ResizeAndPositionElements(slpObj.gameObject);
            RefreshElementValues();
        }

        /// <summary>
        /// 
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
                if (!DataSubSet.Contains(item)) DataSubSet.Add(item);
            }
        }

        /// <summary>
        /// Refresh actual data values
        /// </summary>
        public static void RefreshElementValues()
        {
#if DEBUG
            StackTrace stackTrace = new StackTrace();
            MethodBase callingMethod = stackTrace.GetFrames().Skip(1).Select(frame => frame.GetMethod()).FirstOrDefault();
            Log.LogWarning($"[RefreshElementValues:{IsUpdating}] Caller => {callingMethod}");
#endif
            if (!IsUpdating) { IsUpdating = true; }
            if (UiElementList == null || ContainerObject == null)
            {
#if DEBUG
                Log.LogWarning("UiElementList or ContainerObject => NULL");
#endif
                return;
            }
            IEnumerable<ShipLootPlusItem> ElementsToUpdate = UiElementList.Where(e => !e.image && e.gameOjbect != null);
 #if DEBUG
            Log.LogWarning($"Elements to update: {ElementsToUpdate.Count()}");
#endif
            if (ElementsToUpdate == null || ElementsToUpdate.Count() <= 0)
            {
#if DEBUG
                Log.LogWarning("ElementsToUpdate => NULL or 0");
#endif
                return;
            }

            //TODO: Add a datapoint to show what your profit would be like if you collected all scrap and just scrap on the ship
            //      Take into account the company buying rate as another option
            //TODO: Investigate a feature to tell you what scrap on the planet you need to get to make quota (like BetterItemScan)

            TimeOfDay tod = TimeOfDay.Instance;
            StartOfRound sor = StartOfRound.Instance;
            LevelWeatherType? currentWeatherEnum = sor.currentLevel?.currentWeather;
            string currentWeather = currentWeatherEnum.ToString();
            if (currentWeatherEnum == LevelWeatherType.None) { currentWeather = "Clear"; }

            string currentWeatherShort = currentWeather;
            if (ConfigSettings.ShortCharLength.Value <= currentWeather.Length) { currentWeatherShort = currentWeather.Substring(0, ConfigSettings.ShortCharLength.Value); }

            string currentMoon = Regex.Replace(sor.currentLevel.PlanetName, @"^\d{1,} ?", "", RegexOptions.IgnoreCase);
            if (Regex.IsMatch(currentMoon, "Gordion", RegexOptions.IgnoreCase)) { currentMoon = "Company Building"; }

            string currentMoonShort = currentMoon;
            if (ConfigSettings.ShortCharLength.Value <= currentMoon.Length) { currentMoonShort = currentMoon.Substring(0, ConfigSettings.ShortCharLength.Value); }
#if DEBUG
            Log.LogWarning("========================================");
            int count = 1;
            foreach (ReplacementData item in DataPoints)
            {
                Log.LogInfo($"[DataPoint #{count:D2}]  {item.Pattern} => {item.Value}");
                count++;
            }
            Log.LogWarning("========================================");
#endif

            string deadlineDueText = tod.daysUntilDeadline.ToString();
            string deadlineDueColorPattern = "<color={0}>{1}</color>";
            string deadlineDueColorText = deadlineDueText;
            if (tod.daysUntilDeadline >= 2)
            {
                deadlineDueColorText = string.Format(deadlineDueColorPattern, "#00ff00ff", deadlineDueText);
            }
            else if (tod.daysUntilDeadline == 1)
            {
                deadlineDueColorText = string.Format(deadlineDueColorPattern, "#ffa500ff", deadlineDueText);
            }
            else
            {
                deadlineDueColorText = string.Format(deadlineDueColorPattern, "#ff0000ff", "<b>NOW!</b>");
            }

            LootItem shipLootValue = CalculateLootValue(ItemsToIgnore, "Ship");
            LootItem moonLootValue = CalculateLootValue(ItemsToIgnore, "Moon");
            LootItem allLootValue = CalculateLootValue(ItemsToIgnore, "All");
            LootItem invLootValue = CalculateLootValue(ItemsToIgnore, "Inv");
            int companyRate = ((int)(sor.companyBuyingRate * 100f));
            double profitValue = Math.Round((shipLootValue.Value - tod.quotaFulfilled) * sor.companyBuyingRate);
            if (shipLootValue.Value <= 0) { profitValue = 0; }
#if DEBUG
            Log.LogWarning($"shipLootValue: {shipLootValue.Value}");
            Log.LogWarning($"moonLootValue: {moonLootValue.Value}");
            Log.LogWarning($"allLootValue: {allLootValue.Value}");
            Log.LogWarning($"invLootValue: {invLootValue.Value}");
            Log.LogInfo($"deadlineDueColorText: {deadlineDueColorText} => {tod.daysUntilDeadline}");
#endif
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
                        dataPoint.Value = (tod.daysUntilDeadline <= 0) ? deadlineDueText : tod.daysUntilDeadline.ToString();
                        break;
                    case string s when Regex.IsMatch(s, @"DeadlineWithColors", RegexOptions.IgnoreCase):
                        dataPoint.Value = deadlineDueColorText;
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
                    case string s when Regex.IsMatch(s, @"WeatherShort", RegexOptions.IgnoreCase):
                        dataPoint.Value = currentWeatherShort;
                        break;
                    case string s when Regex.IsMatch(s, @"MoonLongName", RegexOptions.IgnoreCase):
                        dataPoint.Value = currentMoon;
                        break;
                    case string s when Regex.IsMatch(s, @"MoonShortName", RegexOptions.IgnoreCase):
                        dataPoint.Value = currentMoonShort;
                        break;
                }
            });

#if DEBUG
            count = 1; 
            foreach (ReplacementData item in DataPoints)
            {
                Log.LogInfo($"[DataPoint #{count:D2}]> {item.Pattern} => {item.Value}");
                count++;
            }
            Log.LogWarning("========================================");
            Log.LogWarning($"IsUpdating: {IsUpdating}");
#endif
            Parallel.ForEach(ElementsToUpdate, (slpi) =>
            {
#if DEBUG
                Log.LogWarning($"IsUpdating>: {IsUpdating} => {slpi.name}");
#endif
                string textContent = ReplaceValues(slpi.format, DataPoints.Where(d => DataSubSet.Contains(d.Name)).ToList());
                slpi.textMeshProUGui.text = textContent;
                slpi.textMeshProUGui.ForceMeshUpdate();
                if (ConfigSettings.AllCaps.Value) { slpi.textMeshProUGui.text = slpi.textMeshProUGui.text.ToUpper(); }
                IsUpdating = false;
            });
#if DEBUG
            Log.LogWarning($"IsUpdating: {IsUpdating}");
#endif
        }

        #endregion

        #region Method Helpers

        /// <summary>
        /// Log UI element postions and return the values
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
        /// Toggle UI based on events
        /// </summary>
        /// <param name="Timed"></param>
        public static void TryToggleUi(bool Timed = false)
        {
            if (ConfigSettings.AlwaysShow.Value && !Timed)
            {
#if DEBUG
                Log.LogWarning($"AlwaysShow => {ConfigSettings.AlwaysShow.Value} | Timed => {Timed}");
#endif
                if (!StartOfRound.Instance.inShipPhase) return;
                if (!ConfigSettings.AllowOutside.Value && !ConfigSettings.AllowInside.Value)
                {
#if DEBUG
                    Log.LogInfo("Disabling ShipLootPlus UI");
#endif
                    ContainerObject.SetActive(false);
                }
                else
                {
                    if (GameNetworkManager.Instance.localPlayerController.isInsideFactory)
                    {
#if DEBUG
                        Log.LogInfo($"[Inside] Show ShipLootPlus? {ConfigSettings.AllowInside.Value}");
#endif
                        ContainerObject.SetActive(ConfigSettings.AllowInside.Value);
                    }
                    else if (!GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom && !GameNetworkManager.Instance.localPlayerController.isInsideFactory)
                    {
#if DEBUG
                        Log.LogInfo($"[Outside] Show ShipLootPlus? {ConfigSettings.AllowOutside.Value}");
#endif
                        ContainerObject.SetActive(ConfigSettings.AllowOutside.Value);
                    }
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
#if DEBUG
            Log.LogWarning($"Showing [ ContainerObject ] object...");
            Log.LogInfo($"timeLeftDisplay:  {timeLeftDisplay} - {ConfigSettings.DisplayDuration.Value}");
#endif
            ContainerObject.SetActive(true);

            while (timeLeftDisplay > 0f)
            {
#if DEBUG
                Log.LogInfo($"timeLeftDisplay:> {timeLeftDisplay} - {ConfigSettings.DisplayDuration.Value}");
#endif
                float time = timeLeftDisplay;
                timeLeftDisplay = 0f;
                yield return new WaitForSeconds(time);
            }
#if DEBUG
            Log.LogWarning($"Hiding [ ContainerObject ] object...");
            Log.LogInfo($"timeLeftDisplay:  {timeLeftDisplay} - {ConfigSettings.DisplayDuration.Value}");
#endif
            ContainerObject.SetActive(false);
            IsDisplaying = false;
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
                case "Inv":
                    scrapList = Object.FindObjectsOfType<GrabbableObject>().Where(s => s.itemProperties.isScrap
                                                                                                && (s.isHeld || s.isPocketed)
                                                                                                && s.playerHeldBy.IsLocalPlayer
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
                string rxPattern = Regex.Escape($"%{match.Groups[1].Value}%");
                ReplacementData item = Data.FirstOrDefault(v => Regex.IsMatch(v.Pattern, rxPattern, RegexOptions.IgnoreCase));
                if (item == null) continue;
                stReturn = Regex.Replace(stReturn, rxPattern, item.Value, RegexOptions.IgnoreCase);
#if DEBUG
                Log.LogInfo($"Replacing [ {rxPattern} ] in [ {Line} ] with [ {item.Value} ] => [{stReturn}]");
#endif
            }

            return stReturn;
        }

#endregion
    }
}
