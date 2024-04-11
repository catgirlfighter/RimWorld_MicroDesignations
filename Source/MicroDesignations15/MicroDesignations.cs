using System;
using HarmonyLib;
using Verse;
using System.Reflection;

namespace MicroDesignations
{
    [StaticConstructorOnStartup]
    public class MicroDesignations : Mod
    {
        public static FieldInfo LdisallowedFilters = null;
        public MicroDesignations(ModContentPack content) : base(content)
        {
            LdisallowedFilters = AccessTools.Field(typeof(ThingFilter), "disallowedSpecialFilters");
            if (LdisallowedFilters == null) throw new Exception("Couldn't get ThingFilter.disallowedSpecialFilters");
            var harmony = new Harmony("net.avilmask.rimworld.mod.MicroDesignations");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            GetSettings<Settings>();
        }

        public void Save()
        {
            LoadedModManager.GetMod<MicroDesignations>().GetSettings<Settings>().Write();
        }

        public override string SettingsCategory()
        {
            return "MicroDesignations";
        }

        public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
        {
            Settings.DoSettingsWindowContents(inRect);
        }
    }
}
