// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Releases;


namespace FemDesign.Grasshopper
{
    public class StiffnessPoint : GH_Component
    {
        public StiffnessPoint() : base("StiffnessPoint", "StiffnessPoint", "Add Stiffness Point to a SurfaceSupport element.", CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceSupport", "SurfaceSupport", "", GH_ParamAccess.item);
            pManager.AddPointParameter("Position", "Position", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motions stiffness for the point.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimM", "Plastic limits forces for motion springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("StiffnessPoint", "StiffnessPoint", "Define StiffnessPoint.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Supports.SurfaceSupport surfaceSupport = null;
            Point3d point = Point3d.Origin;
            Motions motions = null;
            MotionsPlasticLimits motionsPlasticLimit = null;

            if (!DA.GetData(0, ref surfaceSupport)) { return; }
            if (!DA.GetData(1, ref point)) { return; }
            if (!DA.GetData(2, ref motions)) { return; }
            DA.GetData(3, ref motionsPlasticLimit);

            var stiffPoint = new FemDesign.Supports.StiffnessPoint(surfaceSupport, point.FromRhino(), motions, motionsPlasticLimit);


            DA.SetData(0, stiffPoint);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.StiffnessPoint;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{3FA53044-C6D9-4483-BE65-AC8539FE43F6}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}