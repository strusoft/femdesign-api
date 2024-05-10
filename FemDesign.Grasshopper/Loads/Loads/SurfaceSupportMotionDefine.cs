// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Loads;
using System.Runtime.InteropServices;
using Grasshopper.Kernel;
using Rhino.Geometry;
using StruSoft.Interop.StruXml.Data;
using System.Collections;

namespace FemDesign.Grasshopper
{
    public class SurfaceSupportMotionDefine : FEM_Design_API_Component
    {
        public SurfaceSupportMotionDefine() : base("SurfaceSupportMotion.Define", "SurfaceSupportMotion", "Create a surface support motion.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Displacements", "Displacements", "Displacements. List of 1 or 3 items [u1, u2, u3]. [m]", GH_ParamAccess.list);
            pManager.AddPointParameter("Positions", "Positions", "Location Values. List of 1 or 3 items [pt1, pt2, pt3].", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceMotionLoad", "SurfaceMotionLoad", "SurfaceMotionLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep surface = null;
            DA.GetData("Surface", ref surface);

            List<Vector3d> displacements = new List<Vector3d>();
            DA.GetDataList("Displacements", displacements);

            List<Point3d> points = new List<Point3d>();
            DA.GetDataList("Positions", points);

            Loads.LoadCase loadCase = null;
            DA.GetData("LoadCase", ref loadCase);

            string comment = "";
            DA.GetData("Comment", ref comment);


            // check input

            //if (displacements.Count == 3)
            //{
            //    if (displacements.Count != points.Count)
            //    {
            //        throw new System.ArgumentException("'displacements' and 'positions' must contain the same number of items");
            //    }
            //}

            //if (displacements.Count != 3 && displacements.Count != 1)
            //{
            //    throw new System.ArgumentException("'displacements' must contain 3 items for variable support or 1 item for uniform support");
            //}

            if (displacements.Count != points.Count)
            {
                if (!(displacements.Count == 1 && points.Count == 0))
                {
                    throw new ArgumentException("`displacements` and `points` must have the same number of values.");
                }
            }


            // Convert geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
            FemDesign.Geometry.Vector3d direction = displacements[0].FromRhino().Normalize();

            var loadLocationValues = new List<Loads.LoadLocationValue>();

            int index = 0;
            foreach (var pos in points)
            {
                var loadLocation = new LoadLocationValue(pos.FromRhino(), displacements[index].Length);
                loadLocationValues.Add(loadLocation);
                index++;
            }


            FemDesign.Loads.SurfaceSupportMotion obj = null;
            if (loadLocationValues.Count == 3)
            {
                obj = FemDesign.Loads.SurfaceSupportMotion.Variable(region, direction, loadLocationValues, loadCase, comment);
            }
            else if(loadLocationValues.Count == 1 || loadLocationValues.Count == 0)
            {
                obj = FemDesign.Loads.SurfaceSupportMotion.Uniform(region, direction, loadCase, comment);
            }



            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceMotion;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{4AA9EA05-1DD1-4A14-9C25-D89231B92A57}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}