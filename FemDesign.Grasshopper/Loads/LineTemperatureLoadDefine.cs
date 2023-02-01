// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LineTemperatureLoadDefine: GH_Component
    {
        public LineTemperatureLoadDefine(): base("LineTemperatureLoad.Define", "Define", "Define a surface temperature load", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve. Line or Arc.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Dir", "Direction of load. If undefined global Y-axis will be used. For more information about direction - see FEM-Design GUI.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("TopVal", "TopVal", "Top value. [\x00B0C]", GH_ParamAccess.item);
            pManager.AddNumberParameter("BottomVal", "BottomVal", "Bottom value.  [\x00B0C]", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineTemperatureLoad", "LnTmpLoad", "LineTemperatureLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Curve crv = null;
            if (!DA.GetData(0, ref crv))
            {
                return;
            }

            Vector3d tan = new Vector3d(crv.PointAtEnd - crv.PointAtStart);
            Vector3d dir = Vector3d.CrossProduct(tan, Vector3d.ZAxis);

            if (!DA.GetData(1, ref dir))
            {
                // pass
            }

            if (DA.GetData(1, ref dir) & Vector3d.CrossProduct(dir, tan).IsZero)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The direction cannot be parallel to the curve.");
            }

            double topVal = 0;
            if (!DA.GetData(2, ref topVal))
            {
                return;
            }

            double bottomVal = 0;
            if (!DA.GetData(3, ref bottomVal))
            {
                return;
            }

            var firstValue = new Loads.TopBotLocationValue(crv.PointAtStart.FromRhino(), topVal, bottomVal);
            var secondValue = new Loads.TopBotLocationValue(crv.PointAtEnd.FromRhino(), topVal, bottomVal);
            var vals = new List<Loads.TopBotLocationValue>() { firstValue, secondValue };
            
            Loads.LoadCase lc = null;
            if (!DA.GetData(4, ref lc))
            {
                return;
            }

            string comment = null;
            if (!DA.GetData(5, ref comment))
            {
                // pass;
            }

            if (crv == null || vals == null || lc == null)
            {
                return;
            }


            // convert geometry
            Geometry.Edge edge = crv.FromRhinoLineOrArc1();
            Geometry.Vector3d v = dir.FromRhino();

            // create obj
            Loads.LineTemperatureLoad obj = new Loads.LineTemperatureLoad(edge, v, vals, lc, comment);


            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineTempLoad;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("C5A47F2F-6949-409A-A337-833D99A55234"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}