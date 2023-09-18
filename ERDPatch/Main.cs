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
        #endregion
        #region RDStringPatch
        public static readonly MethodInfo GWC = typeof(RDString).GetMethod("GetWithCheck", (BindingFlags)15420);
        #endregion
    }
}
