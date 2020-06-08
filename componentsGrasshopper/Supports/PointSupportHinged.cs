// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class PointSupportHinged: GH_Component
    {
        public PointSupportHinged(): base("PointSupport.Hinged", "Hinged", "Create a Hinged PointSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point where to place the PointSupport.", GH_ParamAccess.item);
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
            if (point == null)
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.FdPoint3d fdPoint = FemDesign.Geometry.FdPoint3d.FromRhino(point);
            
            //
            FemDesign.Supports.GenericSupportObject obj = new FemDesign.Supports.GenericSupportObject();
            obj.pointSupport = FemDesign.Supports.PointSupport.Hinged(fdPoint);

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
    }
}