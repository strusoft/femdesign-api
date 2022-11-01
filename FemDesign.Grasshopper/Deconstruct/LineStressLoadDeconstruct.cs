// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LineStressLoadDeconstruct : GH_Component
    {
        public LineStressLoadDeconstruct() : base("LineStressLoad.Deconstruct", "Deconstruct", "Deconstruct a LineStressLoad.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LineStressLoad", "LineStressLoad", "Line stress load. Use SortLoads to extract LineStressLoads.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve", "Curve", "Curve.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
            pManager.AddNumberParameter("n1", "n1", "Force at start.", GH_ParamAccess.item);
            pManager.AddNumberParameter("n2", "n2", "Force at end.", GH_ParamAccess.item);
            pManager.AddNumberParameter("m1", "m1", "Moment at start.", GH_ParamAccess.item);
            pManager.AddNumberParameter("m2", "m2", "Moment at end.", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.LineStressLoad stressLoad = null;
            if (!DA.GetData(0, ref stressLoad))
            {
                return;
            }
            if (stressLoad == null)
            {
                return;
            }

            DA.SetData("Guid", stressLoad.LoadCaseGuid);
            DA.SetData("Curve", stressLoad.Edge.ToRhino());
            DA.SetData("Direction", stressLoad.Direction.ToRhino());
            DA.SetData("n1", stressLoad.TopBotLocVal[0].TopVal);
            DA.SetData("m1", stressLoad.TopBotLocVal[0].BottomVal);
            DA.SetData("n2", stressLoad.TopBotLocVal[1].TopVal);
            DA.SetData("m2", stressLoad.TopBotLocVal[1].BottomVal);
            DA.SetData("LoadCaseGuid", stressLoad.LoadCaseGuid);
            DA.SetData("Comment", stressLoad.Comment);
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.LineStressLoadDeconstruct;
        public override Guid ComponentGuid => new Guid("6b4b792a-a7cc-44de-af6a-5e30a9429fa1");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}