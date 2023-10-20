// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
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
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Dir", "Vector. Direction of force.", GH_ParamAccess.item);
            //pManager.AddGenericParameter("LoadLocationValue", "LoadLocationValue", "LoadLocationValue objects. List of 3 items [q1, q2, q3]. [kN/m²]", GH_ParamAccess.list);
            pManager.AddPointParameter("Positions", "Pos", "Positions of Load.  Either 1 value (uniform) or 3 values (variable). [m]", GH_ParamAccess.list);
            pManager.AddNumberParameter("Intensity", "Int", "Intensity of load. Either 1 value (uniform) or 3 values (variable). [kN/m˛]", GH_ParamAccess.list);
            
            pManager.AddBooleanParameter("LoadProjection", "Proj", "LoadProjection. \nFalse: Intensity meant along action line (eg. dead load). \nTrue: Intensity meant perpendicular to direction of load (eg. snow load).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "Case", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comm", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceLoad", "SurfaceLoad", "SurfaceLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Brep surface = null;
            if (!DA.GetData("Surface", ref surface)) { return; }
            
            Vector3d direction = Vector3d.Zero;
            if (!DA.GetData("Direction", ref direction)) { return; }

            List<Point3d> points = null;
            if (!DA.GetDataList("Positions", points)) { return; }
            if (points.Count < 1 || points.Count == 2 || points.Count > 3)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Length of Positions must be 1 (uniform load) or 3 (variable load), but it is {points.Count}.");
                return;
            }

            List<double> intensity = null;
            if (!DA.GetDataList("Intensity", intensity)) { return; }
            if(intensity.Count > points.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Number of positions ({points.Count}) is less than Intensity length ({intensity.Count}).");
            }
            
            bool loadProjection = false;
            DA.GetData("LoadProjection", ref loadProjection);
            
            FemDesign.Loads.LoadCase loadCase = null;
            if (!DA.GetData("LoadCase", ref loadCase)) { return; }
            
            string comment = "";
            DA.GetData("Comment", ref comment);

            // check input
            if (surface == null || points == null || intensity == null || loadCase == null) { return; }

            // Convert geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
            FemDesign.Geometry.Vector3d fdVector = direction.FromRhino().Normalize();
            List<FemDesign.Geometry.Point3d> positions = points.Select(p => p.FromRhino()).ToList();

            List<LoadLocationValue> loads = new List<LoadLocationValue>();
            if (intensity.Count == 1)
            {
                foreach (var pos in positions)
                {
                    var load = new LoadLocationValue(pos, intensity[0]);
                    loads.Add(load);
                }
            }
            else if (intensity.Count == 3)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var load = new LoadLocationValue(positions[i], intensity[i]);
                    loads.Add(load);
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Length of Intensity must be 1 (uniform load) or 3 (variable load), but it is {intensity.Count}.");
                return;
            }

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
            get { return new Guid("{B19B7E82-6265-4627-83F6-F448B9A68DD4}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}