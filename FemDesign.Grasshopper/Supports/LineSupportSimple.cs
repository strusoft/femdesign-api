// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Releases;


namespace FemDesign.Grasshopper
{
    public class LineSupportSimple : GH_Component
    {
        public LineSupportSimple() : base("LineSupport.Simple", "Simple", "Define a LineSupport element.", 
            CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve along where to place the LineSupport.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Tx", "Tx", "Motion in x.\nTrue:  Fix, 1e10 kN/m/m\nFalse: Free, 0.00 kN/m/m", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Ty", "Ty", "Motion in y.\nTrue:  Fix, 1e10 kN/m/m\nFalse: Free, 0.00 kN/m/m", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Tz", "Tz", "Motion in z.\nTrue:  Fix, 1e10 kN/m/m\nFalse: Free, 0.00 kN/m/m", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Rx", "Rx", "Rotation in x.\nTrue:  Fix, 1e10 kNm/m/rad\nFalse: Free, 0.00 kNm/m/rad", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Ry", "Ry", "Rotation in y.\nTrue:  Fix, 1e10 kNm/m/rad\nFalse: Free, 0.00 kNm/m/rad", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Rz", "Rz", "Rotation in z.\nTrue:  Fix, 1e10 kNm/m/rad\nFalse: Free, 0.00 kNm/m/rad", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("LineSupport", "LineSupport", "LineSupport.");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            if (!DA.GetData(0, ref curve))
            {
                return;
            }

            bool tx = true;
            DA.GetData(1, ref tx);

            bool ty = true;
            DA.GetData(2, ref ty);

            bool tz = true;
            DA.GetData(3, ref tz);

            bool rx = true;
            DA.GetData(4, ref rx);

            bool ry = true;
            DA.GetData(5, ref ry);

            bool rz = true;
            DA.GetData(6, ref rz);

            Vector3d localY = Vector3d.Zero;
            DA.GetData(7, ref localY);


            string identifier = "S";
            DA.GetData(8, ref identifier);

            if (curve == null || identifier == null)
            {
                return;
            }

            bool movingLocal = false;
            bool orientLCS = true;

            Geometry.Edge edge = Convert.FromRhinoLineOrArc1(curve);
            var obj = new Supports.LineSupport(edge, tx, ty, tz, rx, ry, rz, movingLocal, identifier);

            // Set local y-axis
            if (!localY.Equals(Vector3d.Zero))
            {
                obj.Group.LocalY = localY.FromRhino();
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
            get { return new Guid("{C124F68E-324E-4CDA-B5AE-3DD5D6B048E4}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}