// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class PointSupportDefine: GH_Component
    {
        public PointSupportDefine(): base("PointSupport.Define", "Define", "Define a PointSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point where to place the PointSupport.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motion springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotation springs.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointSupport", "PointSupport", "User-defined PointSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            Point3d point = Point3d.Origin;
            FemDesign.Releases.Motions motions = null;
            FemDesign.Releases.Rotations rotations = null;
            if (!DA.GetData(0, ref point))
            {
                return;
            }
            if (!DA.GetData(1, ref motions))
            {
                return;
            }
            if (!DA.GetData(2, ref rotations))
            {
                return;
            }
            if (point == null || motions == null || rotations == null)
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.FdPoint3d fdPoint = FemDesign.Geometry.FdPoint3d.FromRhino(point);
            
            //
            FemDesign.Supports.GenericSupportObject obj = new FemDesign.Supports.GenericSupportObject();
            obj.pointSupport = new FemDesign.Supports.PointSupport(fdPoint, motions, rotations);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointSupportDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("42cf6be5-9876-49f4-944f-2cde153e9a05"); }
        }
    }
}