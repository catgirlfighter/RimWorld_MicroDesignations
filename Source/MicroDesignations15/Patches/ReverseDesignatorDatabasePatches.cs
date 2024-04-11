using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;


namespace MicroDesignations.Patches
{
    [HarmonyPatch(typeof(ReverseDesignatorDatabase), "InitDesignators")]
    static class ReverseDesignatorDatabase_InitDesignators_MicroDesignatorsPatch
    {
        static FieldInfo LdesList = null;
        internal static bool Prepare()
        {

            LdesList = AccessTools.Field(typeof(ReverseDesignatorDatabase), "desList");
            if (LdesList == null)
                throw new Exception("Can't get field ReverseDesignatorDatabase.desList");

            return true;
        }

        internal static void Postfix(ReverseDesignatorDatabase __instance)
        {
            List<Designator> desList = (List<Designator>)LdesList.GetValue(__instance);
            List<RecipeDef> list = DefDatabase<RecipeDef>.AllDefsListForReading;

            foreach (var rec in list.Where(x => x.AllRecipeUsers?.OfType<BuildableDef>().Any() == true && x.ingredients?.Count() == 1 && x.ingredients[0]?.filter?.AnyAllowedDef?.stackLimit < 2))
            {
                //Log.Message($"{rec.defName}");
                Designator_MicroRecipe designator = new Designator_MicroRecipe(rec);
                if (designator.designationDef != null)
                    desList.Add(designator);
            }
        }
    }
}
