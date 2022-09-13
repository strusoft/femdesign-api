// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceLoadUniform: GH_Component
    {
        public SurfaceLoadUniform(): base("SurfaceLoad.Uniform", "Uniform", "Create a uniform surface load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Force", "Force", "Force. [kN/m²]", GH_ParamAccess.item);
            pManager.AddBooleanParameter("LoadProjection", "LoadProjection", "LoadProjection. \nFalse: Intensity meant along action line (eg. dead load). \nTrue: Intensity meant perpendicular to direction of load (eg. snow load).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceLoad", "SurfaceLoad", "SurfaceLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep surface = null;
            Vector3d force = Vector3d.Zero;
            FemDesign.Loads.LoadCase loadCase = null;
            bool loadProjection = false;
            string comment = "";
            if (!DA.GetData("Surface", ref surface)) { return; }
            if (!DA.GetData("Force", ref force)) { return; }
            DA.GetData("LoadProjection", ref loadProjection);
            if (!DA.GetData("LoadCase", ref loadCase)) { return; }
            DA.GetData("Comment", ref comment);
            
            if (surface == null || force == null || loadCase == null) { return; }

            // Convert geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
            FemDesign.Geometry.Vector3d _force = force.FromRhino();

            FemDesign.Loads.SurfaceLoad obj = FemDesign.Loads.SurfaceLoad.Uniform(region, _force, loadCase, loadProjection, comment);

            DA.SetData("SurfaceLoad", obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceLoadUniform;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("2c23e698-bf4b-4887-a5bc-6db97cdfc763"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}