using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper;
using System.Drawing;
using System.Reflection;


namespace FemDesign.Info
{
    public class FEMDesignCategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon("FEM-Design", FemDesign.Properties.Resources.Fd_TabIcon_24_24);
            Instances.ComponentServer.AddCategorySymbolName("FEM-Design", 'F');
            return GH_LoadingInstruction.Proceed;
        }
    }

    public class AssemblyInfo : GH_AssemblyInfo
    {
        public override string Name => "FEM-Design";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => FemDesign.Properties.Resources.fdlogo;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "Compatible with FEM-Design 3D Structure only (20 or 21 depending on version of plug-in)." +
            "\nToolbox to communicate with FEM-Design through StruXML allowing its users to create parametric models, run iterative analyses and create automated workflows." +
            "\nThe open-source repository for this project can be found here: https://github.com/strusoft/femdesign-api." +
            "\nThis toolbox contains a plentyfull of functions to:" +
            "\n\tCreate FEM-Design objects (bars, shells, covers, loads, supports, reinforcement etc.)," +
            "\n\tAppend objects to FEM-Design models," +
            "\n\tRun analysis," +
            "\n\tRun design calculations(RC-design, Steel-design, Timber-design)," +
            "\n\tExport results," +
            "\n\tExport documentation";

        //Return a string identifying you or your company.
        public override string AuthorName => "StruSoft";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";


        public override string AssemblyVersion
        {
            get
            {
                IEnumerable<AssemblyName> assembly = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name.Contains("FemDesign.Core"));
                string assemblyVersion = assembly.First().Version?.ToString();
                return assemblyVersion;
            }
        }
    }
}