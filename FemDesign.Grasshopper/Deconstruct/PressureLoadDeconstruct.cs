// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class PressureLoadDeconstruct : GH_Component
    {
        public PressureLoadDeconstruct() : base("PressureLoad.Deconstruct", "Deconstruct", "Deconstruct a PressureLoad.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PressureLoad", "PressureLoad", "PressureLoad.  Use SortLoads to extract PointLoads", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
            pManager.AddNumberParameter("z0", "z0", "Surface level of soil/water (on the global Z axis). [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("q0", "q0", "Load intensity at the surface level. [kN/m²/m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("q1", "q1", "Increment of load intensity per meter (along the global Z axis). [kN/m²/m]", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.PressureLoad obj = null;
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
            DA.SetData(4, obj.Z0);
            DA.SetData(5, obj.Q0);
            DA.SetData(6, obj.Qh);
            DA.SetData(7, obj.LoadCaseGuid);
            DA.SetData(8, obj.Comment);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PressureLoadDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("8bba0bf0-7587-4dd8-97dc-ff18a91c52b5"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}