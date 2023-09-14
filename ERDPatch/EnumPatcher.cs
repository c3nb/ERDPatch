using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace ERDPatch
{
    public static class EnumPatcher<T> where T : Enum
    {
        private static Type thisType = typeof(T);
        private static Dictionary<string, ulong> values;
        private static readonly object van;
        public static void AddField(string name, ulong value)
        {
            values.Add(name, value);
            UpdateVan();
        }
        public static void RemoveField(string name)
        {
            values.Remove(name);
            UpdateVan();
        }
        public static void SetField(string name, ulong value)
        {
            values[name] = value;
            UpdateVan();
        }
        public static ulong GetField(string name)
        {
            if (values.TryGetValue(name, out var value))
                return value;
            return unchecked((ulong)-1);
        }
        static EnumPatcher()
        {
            values = new Dictionary<string, ulong>();
            van = Main.GCVAN.Invoke(null, new object[] { thisType, true });
            var n = Main.VAN_Names(van);
            var v = Main.VAN_Values(van);
            for (int i = 0; i < n.Length; i++)
                values[n[i]] = v[i];
            Main.Harmony.Patch(Main.GEN, new HarmonyMethod(typeof(EnumPatcher<T>).GetMethod(nameof(GENPatch), (BindingFlags)15420)));
        }
        private static bool GENPatch(Type __instance, object value, ref string __result)
        {
            if (__instance != thisType) return true;
            if (value == null) return true;
            Type type = value.GetType();
            var i = Array.IndexOf(Main.VAN_Values(van), Convert.ToUInt64(value));
            if (i >= 0) __result = Main.VAN_Names(van)[i];
            else __result = null;
            return false;
        }
        private static void UpdateVan()
        {
            Main.VAN_Names(van) = values.Keys.ToArray();
            Main.VAN_Values(van) = values.Values.ToArray();
        }
    }
}
