using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace ERDPatch
{
    public static class RDStringPatcher
    {
        private static Dictionary<string, Text> texts;
        public static void AddString(string key, string str)
        {
            texts.Add(key, new Text(str));
        }
        public static void RemoveString(string key)
        {
            texts[key] = new Text(true);
        }
        public static void SetString(string key, string str)
        {
            texts[key] = new Text(str);
        }
        static RDStringPatcher()
        {
            texts = new Dictionary<string, Text>();
            Main.Harmony.Patch(Main.GWC, new HarmonyMethod(typeof(RDStringPatcher).GetMethod(nameof(GWCPatch), (BindingFlags)15420)));
        }
        private static bool GWCPatch(string key, out bool exists, Dictionary<string, object> parameters, ref string __result)
        {
            exists = false;
            if (texts.TryGetValue(key, out var text))
            {
                exists = !text.removed;
                __result = text.text ?? string.Empty;
                if (exists) __result = RDString.ReplaceParameters(__result, parameters);
                return false;
            }
            return true;
        }
        struct Text
        {
            public Text(string s)
            {
                text = s;
                removed = false;
            }
            public Text(bool removed)
            {
                text = null;
                this.removed = removed;
            }
            public string text;
            public bool removed;
        }
    }
}
