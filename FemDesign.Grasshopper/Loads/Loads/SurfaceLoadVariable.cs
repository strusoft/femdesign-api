// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Loads;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceLoadVariable : FEM_Design_API_Component
    {
        public SurfaceLoadVariable() : base("SurfaceLoad.Variable", "Variable", "Create a variable surface load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Vector. Direction of force.", GH_ParamAccess.item);
            pManager.AddPointParameter("Positions", "Position", "Location Values. List of 3 items [pt1, pt2, pt3].", GH_ParamAccess.list);
            pManager.AddNumberParameter("Intensity", "Intensity", "Load Values. List of 3 items [q1, q2, q3]. [kN/m²]", GH_ParamAccess.list);
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
            List<Point3d> locationValues = new List<Point3d>();
            List<double> loadValues = new List<double>();
            FemDesign.Loads.LoadCase loadCase = null;
            bool loadProjection = false;
            string comment = "";
            if (!DA.GetData("Surface", ref surface)) { return; }
            if (!DA.GetData("Direction", ref direction)) { return; }
            if (!DA.GetDataList("Positions", locationValues)) { return; }
            if (!DA.GetDataList("Intensity", loadValues)) { return; }
            DA.GetData("LoadProjection", ref loadProjection);
            if (!DA.GetData("LoadCase", ref loadCase)) { return; }
            DA.GetData("Comment", ref comment);


            if (locationValues.Count != 3)
            {
                throw new System.ArgumentException("'locationValues' must contain exactly 3 items");
            }

            if (loadValues.Count != 3)
            {
                throw new System.ArgumentException("'LoadValues' must contain exactly 3 items");
            }

            // Convert geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
            FemDesign.Geometry.Vector3d fdVector = direction.FromRhino().Normalize();

            var loadLocationValues = new List<Loads.LoadLocationValue>();

            int index = 0;
            foreach (var pos in locationValues)
            {
                var loadLocation = new LoadLocationValue(pos.FromRhino(), loadValues[index]);
                loadLocationValues.Add(loadLocation);
                index++;
            }

            FemDesign.Loads.SurfaceLoad obj = FemDesign.Loads.SurfaceLoad.Variable(region, fdVector, loadLocationValues, loadCase, loadProjection, comment);

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
            get { return new Guid("{FDB73ABC-1EEA-48F1-9087-68EC76A95964}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}