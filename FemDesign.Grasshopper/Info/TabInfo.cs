using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper;

namespace FemDesign.Info
{
    public class FEMDesignCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon("FemDesign", FemDesign.Properties.Resources.Fd_TabIcon_24_24);
            Instances.ComponentServer.AddCategorySymbolName("FemDesign", 'F');
            return GH_LoadingInstruction.Proceed;
        }
    }
}