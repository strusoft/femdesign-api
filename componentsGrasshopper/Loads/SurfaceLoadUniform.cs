// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class SurfaceLoadUniform: GH_Component
    {
        public SurfaceLoadUniform(): base("SurfaceLoad.Uniform", "Uniform", "Create a uniform surface load.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Force", "Force", "Force.", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceLoad", "SurfaceLoad", "SurfaceLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep surface = null;
            Vector3d force = Vector3d.Zero;
            FemDesign.Loads.LoadCase loadCase = null;
            string comment = "";
            if (!DA.GetData(0, ref surface)) { return; }
            if (!DA.GetData(1, ref force)) { return; }
            if (!DA.GetData(2, ref loadCase)) { return; }
            if (!DA.GetData(3, ref comment))
            {
                // pass
            }
            if (surface == null || force == null || loadCase == null) { return; }

            // transform geometry
            FemDesign.Geometry.Region region = FemDesign.Geometry.Region.FromRhino(surface);
            FemDesign.Geometry.FdVector3d _force = FemDesign.Geometry.FdVector3d.FromRhino(force);

            //
            FemDesign.Loads.GenericLoadObject obj = new FemDesign.Loads.GenericLoadObject();
            obj.SurfaceLoad = FemDesign.Loads.SurfaceLoad.Uniform(region, _force, loadCase, comment);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceLoadUniform ;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ba417757-5105-4fd1-a5df-a33faea584a4"); }
        }
    }
}