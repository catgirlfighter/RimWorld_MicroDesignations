using Verse;
using HarmonyLib;
using Verse.AI;

namespace MicroDesignations.Patches
{
    [HarmonyPatch(typeof(Job), nameof(Job.ExposeData))]
    static class Job_ExposeData_MicroDesignationsPatch
    {
        static void Prefix(Job __instance)
        {
            if (__instance.bill == null || __instance.bill.billStack == null) Scribe_Deep.Look(ref __instance.bill, "billSaved"); 
        }
    }
}
