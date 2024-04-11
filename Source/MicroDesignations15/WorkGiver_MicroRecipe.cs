using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using System.Reflection;
using System.Linq;
using System.Text;


namespace MicroDesignations
{
    public class WorkGiver_MicroRecipe : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            MicroWorkGiverDef mdef = def as MicroWorkGiverDef;
            if (mdef == null)
                yield break;

            foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(mdef.designationDef))
            {
                if (!des.target.HasThing)
                {
                    Log.ErrorOnce("MicroRecipe designation has no target.", 63126);
                }
                else
                {
                    yield return des.target.Thing;
                }
            }
            yield break;
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.ClosestTouch;
            }
        }

        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            MicroWorkGiverDef mdef = def as MicroWorkGiverDef;
            if (mdef == null)
                return false;

            if (t.Map.designationManager.DesignationOn(t, mdef.designationDef) == null)
            {
                return false;
            }

            if (pawn.CanReserve(t, 1, -1, null, forced))
            {
                List<Building> list = new List<Building>();
                foreach (var user in mdef.recipeDef.AllRecipeUsers)
                    if (t.Map.listerBuildings.AllBuildingsColonistOfDef(user).Where(
                            x => !x.IsForbidden(pawn)
                            && pawn.CanReserve(x, 1, -1, null, forced)
                            && (x as IBillGiver) != null
                            && (x as IBillGiver).CurrentlyUsableForBills()
                            && pawn.CanReach(x, PathEndMode.InteractionCell, Danger.Deadly)   
                        ).FirstOrDefault() != null)
                        return true;
            }

            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            MicroWorkGiverDef mdef = def as MicroWorkGiverDef;
            if (mdef == null)
                return null;

            LocalTargetInfo target = t;
            if (!pawn.CanReserve(target, 1, -1, null, forced) || t.IsBurning() || t.IsForbidden(pawn))
                return null;

            List<Building> list = new List<Building>();
            foreach (var user in mdef.recipeDef.AllRecipeUsers)
            {
                List<Building> buildings = t.Map.listerBuildings.AllBuildingsColonistOfDef(user).Where(
                    x => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, forced) 
                    && (x as IBillGiver) != null && (x as IBillGiver).CurrentlyUsableForBills()
                    && pawn.CanReach(x, PathEndMode.InteractionCell, Danger.Deadly)).ToList();
                list.AddRange(buildings);
            }

            if (list.NullOrEmpty())
                return null;
            //
            list.SortBy(x => x.TryGetComp<CompRefuelable>() == null || x.TryGetComp<CompRefuelable>().HasFuel ? 0f : 99999f + x.Position.DistanceTo(t.Position));

            Thing building = null;
            foreach (var l in list)
            {
                CompRefuelable compRefuelable = l.TryGetComp<CompRefuelable>();
                if (compRefuelable != null && !compRefuelable.HasFuel)
                {
                    if (RefuelWorkGiverUtility.CanRefuel(pawn, l, forced))
                        return RefuelWorkGiverUtility.RefuelJob(pawn, l, forced, null, null);
                }
                else
                {
                    building = l;
                    break;
                }
            }

            Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, building as IBillGiver, null);
            if (job != null)
            {
                return job;
            }

            job = new Job(mdef.jobDef, building);
            job.targetQueueB = new List<LocalTargetInfo>(1);
            job.countQueue = new List<int>(1);
            job.targetQueueB.Add(t);
            job.countQueue.Add(1);
            job.haulMode = HaulMode.ToCellNonStorage;
            Bill bill = mdef.recipeDef.MakeNewBill();
            Bill_Production bp = bill as Bill_Production;
            if (bp != null)
            {
                bp.repeatCount++;
                bp.SetStoreMode(BillStoreModeDefOf.DropOnFloor);
            }

            job.bill = bill;

            return job;
        }
    }
}
