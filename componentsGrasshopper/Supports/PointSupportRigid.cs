// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class PointSupportRigid: GH_Component
    {
        public PointSupportRigid(): base("PointSupport.Rigid", "Rigid", "Create a Rigid PointSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point where to place the PointSupport.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointSupport", "PointSupport", "Rigid PointSupport.", GH_ParamAccess.item);
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
            obj.pointSupport = FemDesign.Supports.PointSupport.Rigid(fdPoint);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointSupportRigid;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("fda89a34-ef1f-4ccc-a563-e5892288ea7b"); }
        }
    }
}