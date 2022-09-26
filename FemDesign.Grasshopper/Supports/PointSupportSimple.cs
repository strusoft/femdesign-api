// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointSupportSimple : GH_Component
    {
        public PointSupportSimple() : base("PointSupport.Simple", "Simple", "Define a PointSupport element.", CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Position", "Position", "Point|Plane location to place the PointSupport. [m]\nDefault orientation is WorldXY Plane.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Tx", "Tx", "Motion in x.\nTrue:  Fix, 1e10 kN/m\nFalse: Free, 0.00 kN/m", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Ty", "Ty", "Motion in y.\nTrue:  Fix, 1e10 kN/m\nFalse: Free, 0.00 kN/m", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Tz", "Tz", "Motion in z.\nTrue:  Fix, 1e10 kN/m\nFalse: Free, 0.00 kN/m", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Rx", "Rx", "Rotation in x.\nTrue:  Fix, 1e10 kNm/rad\nFalse: Free, 0.00 kNm/rad", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Ry", "Ry", "Rotation in y.\nTrue:  Fix, 1e10 kNm/rad\nFalse: Free, 0.00 kNm/rad", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Rz", "Rz", "Rotation in z.\nTrue:  Fix, 1e10 kNm/rad\nFalse: Free, 0.00 kNm/rad", GH_ParamAccess.item, true);
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
            DA.GetData(0, ref plane);

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

            string identifier = "S";
            DA.GetData(7, ref identifier);

            // Convert geometry
            Geometry.CoordinateSystem fdPlane = plane.FromRhinoPlane();

            var obj = new Supports.PointSupport(fdPlane, tx, ty, tz, rx, ry, rz, identifier);

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
            get { return new Guid("{E152FF47-4562-486A-AF5A-3FF3A2196131}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}