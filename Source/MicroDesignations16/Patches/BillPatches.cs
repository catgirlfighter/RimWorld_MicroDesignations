using HarmonyLib;
using RimWorld;

namespace MicroDesignations.Patches
{
    [HarmonyPatch(typeof(Bill), nameof(Bill.DeletedOrDereferenced), MethodType.Getter)]
    static class Bill_DeletedOrDereferenced_MicroDesignationsPatch
    {
        internal static bool Prefix(Bill __instance, ref bool __result)
        {
            if(__instance.billStack == null)
            {
                __result = __instance.deleted;
                return false;
            }

            return true;
        }
    }
}
