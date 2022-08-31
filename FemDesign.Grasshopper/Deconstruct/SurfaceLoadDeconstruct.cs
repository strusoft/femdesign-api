// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SurfaceLoadDeconstruct : GH_Component
    {
        public SurfaceLoadDeconstruct() : base("SurfaceLoad.Deconstruct", "Deconstruct", "Deconstruct a SurfaceLoad.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceLoad", "SurfaceLoad", "SurfaceLoad. Use SortLoads to extract SurfaceLoads.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
            pManager.AddNumberParameter("q1", "q1", "Load intensity. [kN/m²]", GH_ParamAccess.item);
            pManager.AddNumberParameter("q2", "q2", "Load intensity. [kN/m²]", GH_ParamAccess.item);
            pManager.AddNumberParameter("q3", "q3", "Load intensity. [kN/m²]", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.SurfaceLoad obj = null;
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
            DA.SetData(4, obj.Loads[0].Value);

            // if uniform
            if (obj.Loads.Count == 1)
            {
                DA.SetData(5, obj.Loads[0].Value);
                DA.SetData(6, obj.Loads[0].Value);
            }

            // if variable
            else if (obj.Loads.Count == 3)
            {
                DA.SetData(5, obj.Loads[1].Value);
                DA.SetData(6, obj.Loads[2].Value);
            }

            // else
            else
            {
                throw new System.ArgumentException("Length of load should be 1 or 3.");
            }

            DA.SetData(7, obj.LoadCaseGuid);
            DA.SetData(8, obj.Comment);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceLoadDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("df9cfb5a-93b7-4ed2-b2c3-37bac5d711c7"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}