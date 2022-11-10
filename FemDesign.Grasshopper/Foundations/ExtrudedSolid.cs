// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class ExtrudedSolid : GH_Component
    {
        public ExtrudedSolid() : base("ExtrudedSolid", "ExtrudedSolid", "",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Thickness", "Thickness", "Thickness [m]", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface definining the foundation footing.", GH_ParamAccess.item);
            pManager.AddNumberParameter("a", "a", "Plinth Base [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("b", "b", "Plinth Width [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("h", "h", "Plinth Height [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ExtrudedSolid", "ExtrudedSolid", "ExtrudedSolid.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double thickness = 0.0;
            DA.GetData(0, ref thickness);

            Brep brep = null;
            DA.GetData(1, ref brep);

            double a = 0.0;
            DA.GetData(2, ref a);

            double b = 0.0;
            DA.GetData(3, ref b);

            double h = 0.0;
            DA.GetData(4, ref h);

            FemDesign.Foundations.CuboidPlinth plinth;
            if(a == 0 || b == 0 || h == 0)
            {
                plinth = null;
            }
            else
            {
                plinth = new FemDesign.Foundations.CuboidPlinth(a, b, h);
            }

            var extrudedSolid = new FemDesign.Foundations.ExtrudedSolid(thickness, brep.FromRhino(), false, plinth);

            // output
            DA.SetData(0, extrudedSolid);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ExtrudedSolid;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{068FDB66-0EC1-40AA-84C8-D7A60209E696}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}