// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class PressureLoadConstruct : GH_Component
    {
        public PressureLoadConstruct(): base("PressureLoad.Construct", "Construct", "Construct a pressure load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("LoadDirection", "Direction", "Vector. Direction of force.", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddNumberParameter("z0", "z0", "Surface level of soil/water (on the global Z axis). [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("q0", "q0", "Load intensity at the surface level. [kN/m²]", GH_ParamAccess.item);
            pManager.AddNumberParameter("qh", "qh", "Increment of load intensity per meter (along the global Z axis). [kN/m²/m]", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[6].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PressureLoad", "PressureLoad", "PressureLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep surface = null;
            Vector3d direction = Vector3d.Zero;            
            LoadCase loadCase = null;
            double z0 = 0, q0 = 0, qh = 0;
            string comment = null;
            if (!DA.GetData(0, ref surface)) { return; }
            if (!DA.GetData(1, ref direction)) { return; }
            if (!DA.GetData(2, ref loadCase)) { return; }
            if (!DA.GetData(3, ref z0)) { return; }
            if (!DA.GetData(4, ref q0)) { return; }
            if (!DA.GetData(5, ref qh)) { return; }
            if (!DA.GetData(6, ref comment)) 
            {
                // pass
            }
            if (surface == null || loadCase == null ) { return; }

            // transform geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
            FemDesign.Geometry.Vector3d loadDirection = direction.FromRhino().Normalize();

            PressureLoad obj = new PressureLoad(region, loadDirection, z0, q0, qh, loadCase, comment, false, ForceLoadType.Force);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceLoadPressureLoad;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("f9748559-39e0-461c-9689-cfe2a9fe4c14"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}