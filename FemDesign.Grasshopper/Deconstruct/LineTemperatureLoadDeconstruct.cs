// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LineTemperatureLoadDeconstruct : GH_Component
    {
        public LineTemperatureLoadDeconstruct() : base("LineTemperatureLoad.Deconstruct", "Deconstruct", "Deconstruct a LineTemperatureLoad.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LineTemperatureLoad", "LnTmpLoad", "LineTemperatureLoad. Use SortLoads to extract LineTemperatureLoads.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve", "Curve", "Curve.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("TopBottomLocationValue1", "TopBotLocVal1", "Top bottom location value. [°C]", GH_ParamAccess.item);
            pManager.AddGenericParameter("TopBottomLocationValue2", "TopBotLocVal2", "Top bottom locaiton value. [°C]", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.LineTemperatureLoad obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.LoadCaseGuid);
            DA.SetData(1, obj.Edge.ToRhino());
            DA.SetData(2, obj.Direction.ToRhino());
            DA.SetData(3, obj.TopBotLocVal[0]);
            DA.SetData(4, obj.TopBotLocVal[1]);
            DA.SetData(5, obj.LoadCaseGuid);
            DA.SetData(6, obj.Comment);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineTempLoadDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("018c237a-2af8-449f-b188-680c67e7ffb9"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}