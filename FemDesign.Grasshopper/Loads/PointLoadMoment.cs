// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointLoadMoment: GH_Component
    {
        public PointLoadMoment(): base("PointLoad.Moment", "Moment", "Create moment point load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point. [m]", GH_ParamAccess.item);
            pManager.AddVectorParameter("Moment", "Moment", "Moment. [kNm]", GH_ParamAccess.item);
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
            Vector3d moment = Vector3d.Zero;
            FemDesign.Loads.LoadCase loadCase = null;
            string comment = null;
            if (!DA.GetData(0, ref point)) { return; }
            if (!DA.GetData(1, ref moment)) { return; }
            if (!DA.GetData(2, ref loadCase)) { return; }
            if (!DA.GetData(3, ref comment))
            {
                // pass
            }
            if (moment == null || loadCase == null) { return; };

            // convert geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();
            FemDesign.Geometry.Vector3d _moment = moment.FromRhino();

            //
            FemDesign.Loads.PointLoad obj = new FemDesign.Loads.PointLoad(fdPoint, _moment, loadCase, comment, Loads.ForceLoadType.Moment);
        
            // return 
            DA.SetData(0, obj);
        }
        
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointLoadMoment;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("7d23fc57-ea12-46cc-8a32-d76beaafb9e9"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}