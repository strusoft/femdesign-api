// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointSupportConstruct : GH_Component
    {
        public PointSupportConstruct() : base("PointSupport.Construct", "Construct", "Construct a PointSupport element.",
            CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Position", "Position", "Point|Plane location to place the PointSupport. [m]\nDefault orientation is WorldXY Plane.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motion springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotation springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimM", "Plastic limits forces for motion springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Plastic Limits Moments Rotations", "PlaLimR", "Plastic limits moments for rotation springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointSupport", "PointSupport", "User-defined PointSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane plane = Plane.WorldXY;
            Releases.Motions motions = null;
            Releases.Rotations rotations = null;
            Releases.MotionsPlasticLimits motionsPlasticLimit = null;
            Releases.RotationsPlasticLimits rotationsPlasticLimit = null;
            string identifier = "S";
            if (!DA.GetData(0, ref plane))
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
            DA.GetData(3, ref motionsPlasticLimit);
            DA.GetData(4, ref rotationsPlasticLimit);
            DA.GetData(5, ref identifier);

            // Convert geometry
            var fdPlane = plane.FromRhinoPlane();

            var obj = new Supports.PointSupport(fdPlane, motions, motionsPlasticLimit, rotations, rotationsPlasticLimit, identifier);

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

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}