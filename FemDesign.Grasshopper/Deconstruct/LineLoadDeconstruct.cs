// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LineLoadDeconstruct : GH_Component
    {
        public LineLoadDeconstruct() : base("LineLoad.Deconstruct", "Deconstruct", "Deconstruct a LineLoad.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LineLoad", "LineLoad", "LineLoad. Use SortLoads to extract LineLoads.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve", "Curve", "Curve.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
            pManager.AddNumberParameter("q1", "q1", "Load intensity. [kN]/[kNm]", GH_ParamAccess.item);
            pManager.AddNumberParameter("q2", "q2", "Load intensity. [kN]/[kNm]", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.LineLoad obj = null;
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
            DA.SetData(1, obj.LoadType);
            DA.SetData(2, obj.GetRhinoGeometry());
            DA.SetData(3, obj.Direction.ToRhino());
            DA.SetData(4, obj.Load[0].Value);
            DA.SetData(5, obj.Load[1].Value);
            DA.SetData(6, obj.LoadCaseGuid);
            DA.SetData(7, obj.Comment);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineLoadDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("898c511a-bc4b-40e0-81e0-16437416f8ad"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}