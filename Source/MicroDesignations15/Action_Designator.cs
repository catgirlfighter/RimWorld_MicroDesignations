using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MicroDesignations
{
    public abstract class Action_Designator: Designator
    {
        public abstract Command_Action init_Command_Action(Thing t);
    }
}
