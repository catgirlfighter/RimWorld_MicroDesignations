using System.Collections.Generic;
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


    [HarmonyPatch(typeof(Pawn_JobTracker), "EndCurrentJob")]
    static class Pawn_JobTracker_EndCurrentJob_MicroDesignationsPatch
    {
        //private void CleanupCurrentJob(JobCondition condition, bool releaseReservations, bool cancelBusyStancesSoft = true, bool canReturnToPool = false, bool? carryThingAfterJobOverride = null)
        static void UnmarkDesignation(Pawn_JobTracker __instance, JobCondition condition, bool releaseReservations, bool cancelBusyStancesSoft, bool canReturnToPool, bool? carryThingAfterJobOverride = null)
        {
            if (__instance?.curJob?.bill == null || __instance.curJob.bill.billStack != null /*|| condition != JobCondition.Succeeded*/)
                return;

            if (__instance.curJob.targetB != null && __instance.curJob.targetB.HasThing && !__instance.curJob.targetB.ThingDestroyed)
            {
                RecipeDef rec = __instance.curJob.bill.recipe;
                if (!(__instance.curJob.targetB.Thing is ThingWithComps thing))
                    return;
                //
                DesignationDef dDef = DefDatabase<DesignationDef>.AllDefsListForReading.FirstOrDefault(x => x.defName == rec.defName + "Designation");
                
                if (dDef == null)
                    return;

                Designation d = thing.Map.designationManager.DesignationOn(__instance.curJob.targetB.Thing, dDef);

                if (d == null)
                    return;

                thing.Map.designationManager.RemoveDesignation(d);
                if (condition != JobCondition.Succeeded)
                {
                    Settings.ResetSelectTick();
                    if (thing.AllComps?.FirstOrDefault(x => x is ApplicableDesignationThingComp && (x as ApplicableDesignationThingComp).Props.designationDef == dDef) is ApplicableDesignationThingComp comp)
                        comp.Allowed = null;
                    var ds = Find.ReverseDesignatorDatabase.AllDesignators.FirstOrDefault(x => (x as Designator_MicroRecipe) != null && (x as Designator_MicroRecipe).designationDef == dDef);
                    if (ds == null || !ds.CanDesignateThing(thing))
                        return;
                    //
                    thing.Map.designationManager.AddDesignation(new Designation(thing, dDef));
                }
            }
        }

        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instrs)
        {
            bool b = false;
            MethodInfo m = AccessTools.Method(typeof(Pawn_JobTracker), "CleanupCurrentJob");
            foreach (var i in (instrs))
            {
                if (i.opcode == OpCodes.Call && i.operand == (object)m)
                {
                    b = true;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Pawn_JobTracker_EndCurrentJob_MicroDesignationsPatch), nameof(Pawn_JobTracker_EndCurrentJob_MicroDesignationsPatch.UnmarkDesignation)));
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return new CodeInstruction(OpCodes.Ldarg_3);
                    yield return new CodeInstruction(OpCodes.Ldloc_2);
                }

                yield return i;
            }

            if (!b)  Log.Error("Couldn't patch Pawn_JobTracker.EndCurrentJob");
        }
    }
}
