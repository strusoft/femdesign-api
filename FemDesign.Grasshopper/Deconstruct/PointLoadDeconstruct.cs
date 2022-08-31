// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class PointLoadDeconstruct : GH_Component
    {
        public PointLoadDeconstruct() : base("PointLoad.Deconstruct", "Deconstruct", "Deconstruct a PointLoad.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PointLoad", "PointLoad", "PointLoad. Use SortLoads to extract PointLoads.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "Point", "Point.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
            pManager.AddNumberParameter("q", "q", "Load intensity. [kN]", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.PointLoad obj = null;
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
            DA.SetData(4, obj.Load.Value);
            DA.SetData(5, obj.LoadCaseGuid);
            DA.SetData(6, obj.Comment);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadsDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("145f6339-bf19-4d29-9e81-9e5e0d137f87"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}