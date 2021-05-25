// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointSupportDefine : GH_Component
    {
        public PointSupportDefine() : base("PointSupport.Define", "Define", "Define a PointSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point where to place the PointSupport.", GH_ParamAccess.item);
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
            Point3d point = Point3d.Origin;
            Releases.Motions motions = null;
            Releases.Rotations rotations = null;
            Releases.MotionsPlasticLimits motionsPlasticLimit = null;
            Releases.RotationsPlasticLimits rotationsPlasticLimit = null;
            string identifier = "S";
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
            DA.GetData(3, ref motionsPlasticLimit);
            DA.GetData(4, ref rotationsPlasticLimit);
            DA.GetData(5, ref identifier);
            if (point == null || motions == null || rotations == null || identifier == null)
            {
                return;
            }

            // Convert geometry
            Geometry.FdPoint3d fdPoint = point.FromRhino();

            Supports.GenericSupportObject obj = new Supports.GenericSupportObject();
            obj.PointSupport = new Supports.PointSupport(fdPoint, motions, motionsPlasticLimit, rotations, rotationsPlasticLimit, identifier);

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