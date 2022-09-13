// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceTemperatureLoad: GH_Component
    {
        public SurfaceTemperatureLoad(): base("SurfaceTemperatureLoad.Construct", "Construct", "Construct a surface temperature load. Direction of surface load will be defined by surface normal.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("TopBotLocationValue", "TopBotLocVal", "Temperature at top and bottom of surface. Either 1 value (uniform) or 3 values (variable). [\x00B0C]", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceTemperatureLoad", "SrfTmpLoad", "SurfaceTemperatureLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep brep = null;
            if (!DA.GetData(0, ref brep))
            {
                return;
            }

            Vector3d direction = Vector3d.Zero;
            if (!DA.GetData(1, ref direction))
            {
                return;
            }

            List<Loads.TopBotLocationValue> vals = new List<Loads.TopBotLocationValue>();
            if (!DA.GetDataList(2, vals))
            {
                return;
            }

            Loads.LoadCase lc = null;
            if (!DA.GetData(3, ref lc))
            {
                return;
            }

            string comment = null;
            if (!DA.GetData(4, ref comment))
            {
                // pass;
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
            get { return new Guid("7117d19c-af79-4d1f-8a7e-5c414716f5c3"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}