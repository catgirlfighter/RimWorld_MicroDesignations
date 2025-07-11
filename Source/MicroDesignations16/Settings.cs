using System;
using UnityEngine;
using Verse;

namespace MicroDesignations
{
    public class Settings : ModSettings
    {
        public static bool hide_unresearched = false;
        public static bool hide_empty = false;
        public static bool hide_inactive = false;
        public static int lastSelectTick = 0;
        public static bool betterLoadingActive = false;

        public static void ResetSelectTick()
        {
            var now = DateTime.Now;
            lastSelectTick = now.Hour * 360000 + now.Minute * 60000 + now.Second * 1000 + now.Millisecond;
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            if(Settings.betterLoadingActive)
                listing_Standard.Label("WARNING: BetterLoading is active. You have to place this mod at the bottom of your MOD List if something doesn't show up for you");
            listing_Standard.CheckboxLabeled("hide_unresearched".Translate(), ref hide_unresearched);
            listing_Standard.CheckboxLabeled("hide_empty".Translate(), ref hide_empty);
            listing_Standard.CheckboxLabeled("hide_inactive".Translate(), ref hide_inactive);
            listing_Standard.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hide_unresearched, "hide_unresearched", true, false);
            Scribe_Values.Look(ref hide_empty, "hide_empty", false, false);
            Scribe_Values.Look(ref hide_inactive, "hide_inactive", false, false);
        }
    }
}
