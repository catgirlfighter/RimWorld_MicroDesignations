using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MicroDesignations.Patches
{  
    [HarmonyPatch]
    static class PawnPriorityTracker_ctor_MicroDesignatorsPatch
    {
        static Type TPawnPriorityTracker = null;
        static MethodBase Lget_Item = null;

        internal static bool Prepare()
        {
            TPawnPriorityTracker = AccessTools.TypeByName("WorkTab.PawnPriorityTracker");
            if (TPawnPriorityTracker != null) Lget_Item = TPawnPriorityTracker.GetMethod("get_Item");
            else return false;

            return true;
        }

        internal static MethodBase TargetMethod()
        {
            return Lget_Item;
        }

        internal static void Prefix(ref WorkGiverDef workgiver)
        {
            if (workgiver is MicroWorkGiverDef) workgiver = (workgiver as MicroWorkGiverDef).workGiverDef;
        }
    }
    
}
