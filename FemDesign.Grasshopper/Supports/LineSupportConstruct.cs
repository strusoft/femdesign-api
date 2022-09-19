// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Releases;


namespace FemDesign.Grasshopper
{
    public class LineSupportConstruct : GH_Component
    {
        public LineSupportConstruct() : base("LineSupport.Construct", "Construct", "Construct a LineSupport element.",
            CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve along where to place the LineSupport.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motion springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotation springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimM", "Plastic limits forces for motion springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Plastic Limits Moments Rotations", "PlaLimR", "Plastic limits moments for rotation springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("MovingLocal", "MovingLocal", "LCS changes direction along line? True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineSupport", "LineSupport", "LineSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            Motions motions = null;
            Rotations rotations = null;
            MotionsPlasticLimits motionsPlasticLimit = null;
            RotationsPlasticLimits rotationsPlasticLimit = null;
            bool movingLocal = false;
            if (!DA.GetData(0, ref curve))
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
            DA.GetData(5, ref movingLocal);
            Vector3d v = Vector3d.Zero;
            DA.GetData(6, ref v);
            
            bool orientLCS = true;
            DA.GetData(7, ref orientLCS);
            
            string identifier = "S";
            DA.GetData(8, ref identifier);
            
            if (curve == null || identifier == null)
            {
                return;
            }

            Geometry.Edge edge = Convert.FromRhinoLineOrArc1(curve);
            var obj = new Supports.LineSupport(edge, motions, motionsPlasticLimit, rotations, rotationsPlasticLimit, movingLocal, identifier);

            // Set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                obj.Group.LocalY = v.FromRhino();
            }
            else // Orient coordinate system to GCS
            {
                if (orientLCS)
                {
                    obj.Group.OrientCoordinateSystemToGCS();
                }
            }

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineSupportDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d9254806-ee56-4805-93e6-d9497c735f1c"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}