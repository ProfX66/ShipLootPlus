using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static ShipLootPlus.ShipLootPlus;
using static ShipLootPlus.Utils.Fonts;

namespace ShipLootPlus.Utils
{
    public class Fonts
    {
        public static AssetBundle FontBundle { get; set; }
        public static string FontFile => "3270Fonts";
        public static List<LcFont> LcFontAssets = new List<LcFont>();

        /// <summary>
        /// Font object structure
        /// </summary>
        public class LcFont
        {
            public string Name;
            public TMP_FontAsset Asset;
        }

        /// <summary>
        /// Custom enum attribute
        /// </summary>
        public class EnumValue : Attribute
        {
            public object Value { get; }

            public EnumValue(object value)
            {
                Value = value;
            }
        }

        /// <summary>
        /// Font type enum
        /// </summary>
        public enum FontList
        {
            [EnumValue("Default")]
            Vanilla,

            [EnumValue("Lethal Company (Fixed)")]
            Fixed,

            [EnumValue("Lethal Company (Fixed) Semi-Condensed")]
            FixedSemiCondensed,

            [EnumValue("Lethal Company (Fixed) Condensed")]
            FixedCondensed
        }
        
        /// <summary>
        /// Load fonts from asset bundle
        /// </summary>
        /// <param name="path"></param>
        public static void Load(string path)
        {
            string fullPath = Path.Combine(path, FontFile);
            LcFontAssets.Add(new LcFont { Name = "Default", Asset = null });

            if (File.Exists(fullPath))
            {
                Log.LogInfo($"[Fonts] Loading font assets from '{fullPath}'");
                FontBundle = AssetBundle.LoadFromFile(fullPath);
                foreach (string item in FontBundle.GetAllAssetNames().Where(a => Regex.IsMatch(a, @"\.asset$", RegexOptions.IgnoreCase)))
                {
                    TMP_FontAsset loadedFont = FontBundle.LoadAsset<TMP_FontAsset>(item);
                    Log.LogInfo($"[Fonts] Loaded font: {loadedFont.name}");
                    LcFontAssets.Add(new LcFont { Name = loadedFont.name, Asset = loadedFont });
                }
            }
            else
            {
                Log.LogWarning($"[Fonts] Unable to find font asset '{fullPath}'");
            }
        }

        /// <summary>
        /// Return the specific LcFont object for the passed font name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static LcFont GetFont(string name)
        {
            return LcFontAssets.FirstOrDefault(a => Regex.IsMatch(a.Name, string.Concat(Regex.Escape(name), "$"), RegexOptions.IgnoreCase));
        }
    }
}

public static class Ext
{
    /// <summary>
    /// Get the value the EnumValue attribute as an object
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public static object GetValue(this Enum Item)
    {
        Type type = Item.GetType();
        FieldInfo fieldInfo = type.GetField(Item.ToString());
        EnumValue[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValue), false) as EnumValue[];
        return attributes.Length > 0 ? attributes[0].Value : Item;
    }

    /// <summary>
    /// Get the value the EnumValue attribute as a string
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public static string GetValueString(this Enum Item)
    {
        Type type = Item.GetType();
        FieldInfo fieldInfo = type.GetField(Item.ToString());
        EnumValue[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumValue), false) as EnumValue[];
        return attributes.Length > 0 ? attributes[0].Value.ToString() : Item.ToString();
    }
}

