using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            EnumPatcher<CamMovementType>.SetField("Global", 6);
            EnumPatcher<CamMovementType>.SetField("Player", 33);
            EnumPatcher<CamMovementType>.RemoveField("LastPosition");
            EnumPatcher<CamMovementType>.AddField("FUCKKKK", 2332);
            EnumPatcher<CamMovementType>.AddField("FUCKINGSDJIJ", 345436);
            foreach (var e in Enum.GetValues(typeof(CamMovementType)))
                Logger.Log($"{e} - {(int)e}");
        }
        public static readonly MethodInfo GCVAN = typeof(Enum).GetMethod("GetCachedValuesAndNames", (BindingFlags)15420);
        public static readonly MethodInfo GEN = Type.GetType("System.RuntimeType").GetMethod("GetEnumName", (BindingFlags)15420);
        public static readonly AccessTools.FieldRef<object, string[]> VAN_Names = AccessTools.FieldRefAccess<string[]>(typeof(Enum).GetNestedType("ValuesAndNames", (BindingFlags)15420), "Names");
        public static readonly AccessTools.FieldRef<object, ulong[]> VAN_Values = AccessTools.FieldRefAccess<ulong[]>(typeof(Enum).GetNestedType("ValuesAndNames", (BindingFlags)15420), "Values");
    }
}
