// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceLoadVariableOBSOLETE: GH_Component
    {
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public SurfaceLoadVariableOBSOLETE(): base("SurfaceLoad.Variable", "Variable", "Create a variable surface load.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Vector. Direction of force.", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadLocationValue", "LoadLocationValue", "LoadLocationValue objects. List of 3 items [q1, q2, q3].", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceLoad", "SurfaceLoad", "SurfaceLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep surface = null;
            Vector3d direction = Vector3d.Zero;
            List<FemDesign.Loads.LoadLocationValue> loads = new List<FemDesign.Loads.LoadLocationValue>();
            FemDesign.Loads.LoadCase loadCase = null;
            string comment = null;
            if (!DA.GetData(0, ref surface)) { return; }
            if (!DA.GetData(1, ref direction)) { return; }
            if (!DA.GetDataList(2, loads)) { return; }
            if (!DA.GetData(3, ref loadCase)) { return; }
            if (!DA.GetData(4, ref comment))
            {
                // pass
            }
            if (surface == null || loads == null || loadCase == null) { return; }
            if (loads.Count != 3)
            {
                throw new System.ArgumentException("Load must contain exactly 3 items");
            }

            // transform geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
            FemDesign.Geometry.Vector3d fdVector = direction.FromRhino().Normalize();

            //
            FemDesign.Loads.SurfaceLoad obj = FemDesign.Loads.SurfaceLoad.Variable(region, fdVector, loads, loadCase, false, comment);

            // return
            DA.SetData(0, obj);
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
            get { return new Guid("1a47edc8-1db0-4b85-a372-3f2e55a82b00"); }
        }
    }
}
