// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceTemperatureLoad : FEM_Design_API_Component
    {
        public SurfaceTemperatureLoad() : base("SurfaceTemperatureLoad.Construct", "Construct", "Construct a surface temperature load. Direction of surface load will be defined by surface normal.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "Point", "Point of top bottom location value. [m]", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("TopVal", "TopVal", "Top value. Either 1 value (uniform) or 3 values (variable). [\x00B0C]", GH_ParamAccess.list);
            pManager.AddNumberParameter("BottomVal", "BottomVal", "Bottom value. Either 1 value (uniform) or 3 values (variable). [\x00B0C]", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceTemperatureLoad", "SrfTmpLoad", "Surface temperature variation load.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep brep = null;
            if (!DA.GetData(0, ref brep))
            {
                return;
            }

            Vector3d direction = brep.Surfaces[0].NormalAt(0, 0);
            Point3d pt = brep.Surfaces[0].PointAt(0, 0);


            List<Point3d> points = new List<Point3d>();
            if (!DA.GetDataList(1, points))
            {
                points.Add(pt);
            }
            
            List<double> topVal = new List<double>();
            if (!DA.GetDataList(2, topVal))
            {
                return;
            }
            if (topVal.Count == 2 || topVal.Count > 3)
            {
                throw new System.ArgumentException("Top value must contain 1 or 3 elements!");
            }

            List<double> bottomVal = new List<double>();
            if (!DA.GetDataList(3, bottomVal))
            {
                return;
            }
            if (bottomVal.Count == 2 || bottomVal.Count > 3)
            {
                throw new System.ArgumentException("Bottom value must contain 1 or 3 elements!");
            }


            var vals = new List<Loads.TopBotLocationValue>();
            if (points.Count == 1)
            {
                if (topVal.Count == 3)
                {
                    throw new System.ArgumentException("TopVal must contain 1 element if Point list length is 1!");
                }
                if (bottomVal.Count == 3)
                {
                    throw new System.ArgumentException("BottomVal must contain 1 element if Point list length is 1!");
                }
                vals.Add(new Loads.TopBotLocationValue(points[0].FromRhino(), topVal[0], bottomVal[0]));
            }
            else if(points.Count == 3)
            {
                if (topVal.Count == 1)
                {
                    topVal.Add(topVal[0]);
                    topVal.Add(topVal[0]);
                }
                if (bottomVal.Count == 1)
                {
                    bottomVal.Add(bottomVal[0]);
                    bottomVal.Add(bottomVal[0]);
                }
                vals.Add(new Loads.TopBotLocationValue(points[0].FromRhino(), topVal[0], bottomVal[0]));
                vals.Add(new Loads.TopBotLocationValue(points[1].FromRhino(), topVal[1], bottomVal[1]));
                vals.Add(new Loads.TopBotLocationValue(points[2].FromRhino(), topVal[2], bottomVal[2]));
            }
            else
            {
                throw new System.ArgumentException("Point list must contain 1 or 3 elements!");
            }


            Loads.LoadCase lc = null;
            if (!DA.GetData(4, ref lc))
            {
                return;
            }

            string comment = null;
            if (!DA.GetData(5, ref comment))
            {
                //pass;
            }

            if (brep == null || vals == null || lc == null)
            {
                return;
            }

            // convert geometry
            Geometry.Region region = brep.FromRhino();
            Geometry.Vector3d dir = direction.FromRhino();

            // create obj
            Loads.SurfaceTemperatureLoad obj = new Loads.SurfaceTemperatureLoad(region, dir, vals, lc, comment);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceTempLoad;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("86663E02-3711-4821-A5F6-99572F80925E"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}