using System;
using System.Linq;
using System.Collections.Generic;
using Verse;
using HarmonyLib;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;

namespace MicroDesignations
{
    public static class StaticConstructorOnStartupUtility_Patch
    {
        [HarmonyPatch(typeof(StaticConstructorOnStartupUtility), "CallAll")]
        //[HarmonyPatch]
        static class StaticConstructorOnStartupUtility_CallAll_MicroDesignationsPatch
        {
            //static bool BetterLoadingActive = false;
            internal static bool Prepare()
            {
                return !(Settings.betterLoadingActive = AccessTools.Method("BetterLoading.Stage.InitialLoad.StageRunStaticCctors:PreCallAll") != null);
            }
            //
            internal static void Postfix()
            {
                CrossReferences.Populate();
            }
        }
    }
}
