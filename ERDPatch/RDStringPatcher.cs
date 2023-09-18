using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ERDPatch
{
    public static class RDStringPatcher
    {
        private static Dictionary<string, string> texts;
        public static void AddString(string key, string str)
        {
            texts.Add(key, str);
        }
        public static void RemoveString(string key)
        {
            texts[key] = null;
        }
        public static void SetString(string key, string str)
        {
            texts[key] = str;
        }
        static RDStringPatcher()
        {
            texts = new Dictionary<string, string>();
            Main.Harmony.Patch(Main.GWC, new HarmonyMethod(typeof(RDStringPatcher).GetMethod(nameof(GWCPatch), (BindingFlags)15420)));
        }
        private static bool GWCPatch(string key, out bool exists, Dictionary<string, object> parameters, ref string __result)
        {
            exists = false;
            if (texts.TryGetValue(key, out var text))
            {
                exists = text != null;
                __result = text ?? string.Empty;
                if (exists) __result = RDString.ReplaceParameters(__result, parameters);
                return false;
            }
            return true;
        }
    }
}
