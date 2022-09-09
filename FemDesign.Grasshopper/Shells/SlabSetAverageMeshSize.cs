// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SlabAverageMeshSize: GH_Component
    {
        public SlabAverageMeshSize(): base("Slab.AverageSurfaceElementSize", "AvgSrfElemSize", "Set average surface element size to slab.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
            pManager.AddNumberParameter("AverageSurfaceElementSize", "AvgSrfElemSize", "Average surface element size. [m]", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Passed slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Shells.Slab slab = null;
            double avgMeshSize = 0;
            if (!DA.GetData(0, ref slab))
            {
                return;
            }
            if (!DA.GetData(1, ref avgMeshSize))
            {
                return;
            }
            if (slab == null)
            {
                return;
            }

            //
            FemDesign.Shells.Slab obj = FemDesign.Shells.Slab.AverageSurfaceElementSize(slab, avgMeshSize);

            //
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SlabSetAverageElementSize;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("2408b02c-4aa8-4352-84c6-366f7c42f9bf"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}