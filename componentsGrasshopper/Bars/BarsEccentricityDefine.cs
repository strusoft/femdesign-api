// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class BarsEccentricityDefine: GH_Component
    {
        public BarsEccentricityDefine(): base("Eccentricity.Define", "Define", "Define the eccentricity of bar element along its local axes.", "FemDesign", "Bars")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("y", "y", "Eccentricity local-y", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z", "z", "Eccentricity local-z", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            double y = 0, z = 0;
            if (!DA.GetData(0, ref y))
            {
                // pass
            }
            if (!DA.GetData(1, ref z))
            {
                // pass
            }

            // return
            DA.SetData(0, FemDesign.Bars.Eccentricity.Define(y, z));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BarEccentricityDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("b724a47d-3cd6-4b16-9d8a-bb542d1239ff"); }
        }
    }
}