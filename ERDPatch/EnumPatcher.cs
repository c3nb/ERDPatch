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
        internal static readonly object van;
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
            Main.Harmony.Patch(Main.GEN, new HarmonyMethod(typeof(Main).GetMethod(nameof(Main.GENPatch), (BindingFlags)15420).MakeGenericMethod(typeof(T))));
        }
        private static void UpdateVan()
        {
            Main.VAN_Names(van) = values.Keys.ToArray();
            Main.VAN_Values(van) = values.Values.ToArray();
        }
    }
}
