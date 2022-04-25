// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointSupportRigid: GH_Component
    {
        public PointSupportRigid(): base("PointSupport.Rigid", "Rigid", "Create a Rigid PointSupport element.", "FemDesign", "Supports")
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
            
            var obj = FemDesign.Supports.PointSupport.Rigid(fdPoint, identifier);

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