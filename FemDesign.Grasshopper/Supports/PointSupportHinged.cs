// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointSupportHinged: GH_Component
    {
        public PointSupportHinged(): base("PointSupport.Hinged", "Hinged", "Create a Hinged PointSupport element.", CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Position", "Position", "Point|Plane location to place the PointSupport. [m]\nDefault orientation is WorldXY Plane.", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
           pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointSupport", "PointSupport", "Hinged PointSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            Plane plane = Plane.WorldXY;
            if (!DA.GetData(0, ref plane))
            {
                return;
            }
            string identifier = "S";
            if (!DA.GetData(1, ref identifier))
            {
                // pass
            }


            // convert geometry
            var fdPlane = plane.FromRhinoPlane();
            
            var obj = FemDesign.Supports.PointSupport.Hinged(fdPlane, identifier);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointSupportHinged;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("74efae30-5ccc-432e-a78e-698d24f1fbbe"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}