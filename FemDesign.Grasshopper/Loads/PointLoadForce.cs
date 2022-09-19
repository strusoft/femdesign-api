// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class PointLoadForce: GH_Component
    {
        public PointLoadForce(): base("PointLoad.Force", "Force", "Create force point load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point. [m]", GH_ParamAccess.item);
            pManager.AddVectorParameter("Force", "Force", "Force. [kN]", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointLoad", "PointLoad", "PointLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Point3d point = Point3d.Origin;
            Vector3d force = Vector3d.Zero;
            FemDesign.Loads.LoadCase loadCase = null;
            string comment = null;
            if (!DA.GetData(0, ref point)) { return; }
            if (!DA.GetData(1, ref force)) { return; }
            if (!DA.GetData(2, ref loadCase)) { return; }
            if (!DA.GetData(3, ref comment))
            {
                // pass
            }
            if (force == null || loadCase == null) { return; };

            // Convert geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();
            FemDesign.Geometry.Vector3d _force = force.FromRhino();

            PointLoad obj = new FemDesign.Loads.PointLoad(fdPoint, _force, loadCase, comment, ForceLoadType.Force);

            DA.SetData(0, obj);
        }
        
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointLoadForce;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("734c164f-e273-4aca-9115-d855ab066725"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}