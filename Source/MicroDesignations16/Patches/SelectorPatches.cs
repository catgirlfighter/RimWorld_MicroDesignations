using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System;


namespace MicroDesignations.Patches
{
    [HarmonyPatch(typeof(Selector), "ClearSelection")]
    static class Selector_ClearSelection_MicroDesignationsPatch
    {
        internal static void Postfix()
        {
            Settings.ResetSelectTick();
        }
    }

    [HarmonyPatch(typeof(Selector), "Deselect")]
    static class Selector_Deselect_MicroDesignationsPatch
    {
        internal static void Postfix()
        {
            Settings.ResetSelectTick();
        }
    }

    [HarmonyPatch(typeof(Selector), "Select")]
    static class Selector_Select_MicroDesignationsPatch
    {
        internal static void Postfix()
        {
            Settings.ResetSelectTick();
        }
    }
}
