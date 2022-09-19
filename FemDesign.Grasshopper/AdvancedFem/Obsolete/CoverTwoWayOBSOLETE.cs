// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class CoverTwoWayOBSOLETE: GH_Component
    {
        public CoverTwoWayOBSOLETE(): base("Cover.TwoWay", "TwoWay", "Create a two way cover.", "FEM-Design", "Cover")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface. Surface must be flat.", GH_ParamAccess.item);
            pManager.AddGenericParameter("SupportingBars", "SupportingBars", "Single bar element or list of bar elements. List cannot be nested.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("SupportingSlabs", "SupportingSlabs", "Single slab element or list of slab elements. List cannot be nested.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item, "CO");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Cover", "Cover", "Two way Cover.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep brep = null;
            List<FemDesign.Bars.Bar> bars = new List<FemDesign.Bars.Bar>();
            List<FemDesign.Shells.Slab> slabs = new List<FemDesign.Shells.Slab>();
            string identifier = "CO";
            if (!DA.GetData(0, ref brep))
            {
                return;
            }
            if (!DA.GetDataList(1, bars))
            {
                // pass
            }
            if (!DA.GetDataList(2, slabs))
            {
                // pass
            }
            if (!DA.GetData(3, ref identifier))
            {
                // pass
            }
            if (brep == null || bars == null || slabs == null || identifier == null)
            {
                return;
            }

            // create list of supporting structures
            List<object> structures = new List<object>();
            foreach (FemDesign.Bars.Bar bar in bars)
            {
                structures.Add(bar);
            }
            foreach(FemDesign.Shells.Slab slab in slabs)
            {
                structures.Add(slab);
            }

            // convert geometry
            FemDesign.Geometry.Region region = brep.FromRhino();

            //
            FemDesign.Cover obj = FemDesign.Cover.TwoWayCover(region, structures, identifier);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.CoverTwoWay;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("aa9d087e-14ce-48d5-abad-33f1364f390a"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}