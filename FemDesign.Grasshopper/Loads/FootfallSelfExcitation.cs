// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class FootfallSelfExcitation : GH_Component
    {
        public FootfallSelfExcitation() : base("Footfall.SelfExcitation", "SelfExcitation", "Create a footfall self excitation region.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Footfall", "Footfall", "Footfall.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep surface = null;
            string identifier = null;
            string comment = null;
            if (!DA.GetData(0, ref surface)) { return; }
            if (!DA.GetData(1, ref identifier)) { /* Pass */}
            if (!DA.GetData(2, ref comment)) { /* Pass */}

            // convert geometry
            Geometry.Region region = surface.FromRhino();

            // Create a Footfall self excitation
            var obj = new Loads.Footfall(region, identifier, comment);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.FootfallSelfExcitation;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("1a637184-84e6-4bdf-8cd6-b96f8c2e3d55"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

    }
}