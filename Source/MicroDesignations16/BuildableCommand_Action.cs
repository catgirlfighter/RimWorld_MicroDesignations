using System;
using Verse;
using UnityEngine;

namespace MicroDesignations
{
    public class BuildableCommand_Action: Command_Action
    {
        public BuildableDef buildableDef;
        public ThingDef thingDef;

        public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
        {
            Widgets.DefIcon(rect, buildableDef, thingDef, 0.85f);
        }
    }
}