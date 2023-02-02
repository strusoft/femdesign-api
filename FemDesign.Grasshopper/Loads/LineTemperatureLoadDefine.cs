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
            pManager.AddVectorParameter("Direction", "Dir", "Direction of load. If undefined local Y-axis will be used. For more information about direction - see FEM-Design GUI.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("TopVal", "TopVal", "Top value. If 1 value this value defines both start and end. [\x00B0C]", GH_ParamAccess.list);
            pManager.AddNumberParameter("BottomVal", "BottomVal", "Bottom value. If 1 value this value defines both start and end. [\x00B0C]", GH_ParamAccess.list);
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
            Vector3d dir = new Vector3d();

            //We have to use different default direction vector for a column (its tangent vector is parallel to the Z-axis) otherwise we will get an error message in default mode.
            if (tan.IsParallelTo(Vector3d.ZAxis) == 1)
            {
                dir = Vector3d.CrossProduct(tan, -Vector3d.XAxis);
            }
            else
            {
                dir = Vector3d.CrossProduct(tan, Vector3d.ZAxis);
            }


            if (!DA.GetData(1, ref dir))
            {
                // pass
            }

            if (DA.GetData(1, ref dir) & tan.IsParallelTo(dir) == 1)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The direction cannot be parallel to the curve.");
            }


            List<double> topVal = new List<double>();
            if (!DA.GetDataList(2, topVal))
            {
                return;
            }
            if ( topVal.Count == 1)
            {
                topVal.Add(topVal[0]);
            }
            if (topVal.Count > 2)
            {
                var msg = "Top value must contain 1 or 2 elements!";
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, msg);
                throw new System.ArgumentException(msg);
            }

            List<double> bottomVal = new List<double>();
            if (!DA.GetDataList(3, bottomVal))
            {
                return;
            }
            if (bottomVal.Count == 1)
            {
                bottomVal.Add(bottomVal[0]);
            }
            if (bottomVal.Count > 2)
            {
                var msg = "Bottom value must contain 1 or 2 elements!";
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, msg);
                throw new System.ArgumentException(msg);
            }

            var firstVal = new Loads.TopBotLocationValue(crv.PointAtStart.FromRhino(), bottomVal[0], topVal[0]);
            var secondVal = new Loads.TopBotLocationValue(crv.PointAtEnd.FromRhino(), bottomVal[1], topVal[1]);
            var vals = new List<Loads.TopBotLocationValue>() { firstVal, secondVal };
            

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