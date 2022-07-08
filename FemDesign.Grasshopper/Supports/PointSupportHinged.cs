// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointSupportHinged: GH_Component
    {
        public PointSupportHinged(): base("PointSupport.Hinged", "Hinged", "Create a Hinged PointSupport element.", "FEM-Design", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point where to place the PointSupport. [m]", GH_ParamAccess.item);
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
            Point3d point = Point3d.Origin;
            if (!DA.GetData(0, ref point))
            {
                return;
            }
            string identifier = "S";
            if (!DA.GetData(1, ref identifier))
            {
                // pass
            }
            if (point == null || identifier == null)
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.FdPoint3d fdPoint = point.FromRhino();
            
            var obj = FemDesign.Supports.PointSupport.Hinged(fdPoint, identifier);

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