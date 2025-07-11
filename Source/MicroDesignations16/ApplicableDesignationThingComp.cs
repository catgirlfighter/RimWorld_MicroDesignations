using Verse;
namespace MicroDesignations
{
    public class ApplicableDesignationThingComp: ThingComp
    {
        public bool? Allowed = null;
        public CompProperties_ApplicableDesignation Props
        {
            get
            {
                return (CompProperties_ApplicableDesignation)props;
            }
        }
    }

    public class CompProperties_ApplicableDesignation: CompProperties
    {
        public DesignationDef designationDef = null;
        public CompProperties_ApplicableDesignation()
        {
            compClass = typeof(ApplicableDesignationThingComp);
        }
    }
}
