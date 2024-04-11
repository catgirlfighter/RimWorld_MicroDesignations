using HarmonyLib;
using Verse;

namespace MicroDesignations.Patches
{
    [HarmonyPatch(typeof(Designator), "CreateReverseDesignationGizmo")]
    static class InspectGizmoGrid_DrawInspectGizmoGridFor_MicroDesignationsPatch
    {
        internal static bool Prefix(Designator __instance, ref Command_Action __result, Thing t)
        {
            if (__instance is Action_Designator)
            {
                var desu = __instance as Action_Designator;
                __result = desu.init_Command_Action(t);
            }
            return __result == null;
        }
    }
}