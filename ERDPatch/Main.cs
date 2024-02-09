using HarmonyLib;
using System;
using System.Reflection;
using static UnityModManagerNet.UnityModManager;

namespace ERDPatch
{
    public static class Main
    {
        public static ModEntry Mod { get; private set; }
        public static ModEntry.ModLogger Logger { get; private set; }
        public static Harmony Harmony { get; private set; }
        public static void Load(ModEntry modEntry)
        {
            Mod = modEntry;
            Logger = modEntry.Logger;
            Harmony = new Harmony(modEntry.Info.Id);
        }
        #region EnumPatch
        public static readonly MethodInfo GCVAN = typeof(Enum).GetMethod("GetCachedValuesAndNames", (BindingFlags)15420);
        public static readonly MethodInfo GEN = Type.GetType("System.RuntimeType").GetMethod("GetEnumName", (BindingFlags)15420);
        public static readonly AccessTools.FieldRef<object, string[]> VAN_Names = AccessTools.FieldRefAccess<string[]>(typeof(Enum).GetNestedType("ValuesAndNames", (BindingFlags)15420), "Names");
        public static readonly AccessTools.FieldRef<object, ulong[]> VAN_Values = AccessTools.FieldRefAccess<ulong[]>(typeof(Enum).GetNestedType("ValuesAndNames", (BindingFlags)15420), "Values");
        public static bool GENPatch<T>(Type __instance, object value, ref string __result) where T : Enum
        {
            if (__instance != typeof(T)) return true;
            if (value == null) return true;
            Type type = value.GetType();
            var i = Array.IndexOf(VAN_Values(EnumPatcher<T>.van), Convert.ToUInt64(value));
            if (i >= 0) __result = VAN_Names(EnumPatcher<T>.van)[i];
            else __result = null;
            return false;
        }
        #endregion
        #region RDStringPatch
        public static readonly MethodInfo GWC = typeof(RDString).GetMethod("GetWithCheck", (BindingFlags)15420);
        #endregion
    }
}
