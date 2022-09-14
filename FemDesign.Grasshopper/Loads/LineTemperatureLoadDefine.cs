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
            pManager.AddVectorParameter("Direction", "Dir", "Direction of load. If undefined global Z-axis will be used. For more information about direction - see FEM-Design GUI.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("TopBotLocationValue", "TopBotLocVal", "Temperature at top and bottom of surface. List should contain 2 values - start and end value. [\x00B0C]", GH_ParamAccess.list);
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

            Vector3d dir = Vector3d.ZAxis;
            if (!DA.GetData(1, ref dir))
            {
                // pass
            }

            List<Loads.TopBotLocationValue> vals = new List<Loads.TopBotLocationValue>();
            if (!DA.GetDataList(2, vals))
            {
                return;
            }

            Loads.LoadCase lc = null;
            if (!DA.GetData(3, ref lc))
            {
                return;
            }

            string comment = null;
            if (!DA.GetData(4, ref comment))
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
            get { return new Guid("ac8a25ca-1b97-4d32-94f2-20a79575e298"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}