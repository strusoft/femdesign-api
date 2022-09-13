// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceLoadVariable: GH_Component
    {
        public SurfaceLoadVariable(): base("SurfaceLoad.Variable", "Variable", "Create a variable surface load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Vector. Direction of force.", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadLocationValue", "LoadLocationValue", "LoadLocationValue objects. List of 3 items [q1, q2, q3]. [kN/m²]", GH_ParamAccess.list);
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
            Vector3d direction = Vector3d.Zero;
            List<FemDesign.Loads.LoadLocationValue> loads = new List<FemDesign.Loads.LoadLocationValue>();
            FemDesign.Loads.LoadCase loadCase = null;
            bool loadProjection = false;
            string comment = "";
            if (!DA.GetData("Surface", ref surface)) { return; }
            if (!DA.GetData("Direction", ref direction)) { return; }
            if (!DA.GetDataList("LoadLocationValue", loads)) { return; }
            DA.GetData("LoadProjection", ref loadProjection);
            if (!DA.GetData("LoadCase", ref loadCase)) { return; }
            DA.GetData("Comment", ref comment);
            
            if (surface == null || loads == null || loadCase == null) { return; }
            if (loads.Count != 3)
            {
                throw new System.ArgumentException("Load must contain exactly 3 items");
            }

            // Convert geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
            FemDesign.Geometry.Vector3d fdVector = direction.FromRhino().Normalize();

            FemDesign.Loads.SurfaceLoad obj = FemDesign.Loads.SurfaceLoad.Variable(region, fdVector, loads, loadCase, loadProjection, comment);

            DA.SetData("SurfaceLoad", obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceLoadVariable;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("3be4e5aa-63df-4ea9-bbd1-99d59c694b31"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}