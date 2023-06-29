// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SlabPeakSmoothingRegion : GH_Component
    {
        public SlabPeakSmoothingRegion() : base("Slab.PeakSmoothingRegion", "PeakSmoothing", "Set peak smoothing regions.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Region", "Region", "Region", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Inactive", "Inactive", "Set the smoothing region to active or inactive. If true, the smoothing region is inactive.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SmoothingRegion", "SmRegion", "Peak smoothing region object. It should be added to the model as an element.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Brep region = null;
            if(!DA.GetData(0, ref region)) return;

            bool inactive = false;
            if(!DA.GetData(1, ref inactive))
            {
                // pass
            }

            if (region == null) return;

            FemDesign.Geometry.Region fdRegion = region.FromRhino();
            var output = new FemDesign.FiniteElements.PeakSmoothingRegion(fdRegion, inactive);

            // get output
            DA.SetData(0, output);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get { return FemDesign.Properties.Resources.SlabPeakSmoothingRegion; }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("35DCA882-2418-4AC5-AE9D-5048C1E1ADCA"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}
