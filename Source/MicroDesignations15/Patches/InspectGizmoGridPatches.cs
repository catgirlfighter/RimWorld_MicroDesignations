using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace MicroDesignations.Patches
{
    /*
    [HarmonyPatch(typeof(InspectGizmoGrid), "DrawInspectGizmoGridFor")]
    static class InspectGizmoGrid_DrawInspectGizmoGridFor_MicroDesignationsPatch
    {
        
        static Command_Action initAction(Command_Action defaultAction, Designator des)
        {
            //Log.Message($"{des}");
            if (des.GetType().IsSubclassOf(typeof(Action_Designator)))
                return (des as Action_Designator).init_Command_Action();
            else
                return defaultAction;
        }

        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instrs)
        {
            object des = null;
            bool b = false;
            ConstructorInfo ctor = typeof(Command_Action).GetConstructors()[0];
            foreach (var i in (instrs))
            {
                if (i.opcode == OpCodes.Stfld && i.operand is FieldInfo && (i.operand as FieldInfo).GetUnderlyingType() == typeof(Designator))
                    des = i.operand;

                if (i.opcode == OpCodes.Newobj && i.operand == (object)ctor)
                {
                    b = true;
                    yield return i;//not pretty, but I have paws for hands
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 6);
                    yield return new CodeInstruction(OpCodes.Ldfld, des);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(InspectGizmoGrid_DrawInspectGizmoGridFor_MicroDesignationsPatch), nameof(InspectGizmoGrid_DrawInspectGizmoGridFor_MicroDesignationsPatch.initAction)));
                } else
                    yield return i;
            }
            if (!b) Log.Error("Couldn't patch InspectGizmoGrid.DrawInspectGizmoGridFor");

        }
    }
    */
}
