// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SurfaceReinforcementAddToSlab: GH_Component
    {
        public SurfaceReinforcementAddToSlab(): base("SurfaceReinforcement.AddToSlab", "AddToSlab", "Add surface reinforcement to slab.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
            pManager.AddGenericParameter("SurfaceReinforcement", "SurfaceReinforcement", "SurfaceReinforcment to add to slab. Item or list.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Passed slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            FemDesign.Shells.Slab slab = null;
            List<FemDesign.Reinforcement.SurfaceReinforcement> surfaceReinforcement = new List<FemDesign.Reinforcement.SurfaceReinforcement>();
            if (!DA.GetData(0, ref slab))
            {
                return;
            }
            if (!DA.GetDataList(1, surfaceReinforcement))
            {
                return;
            }
            if (slab == null || surfaceReinforcement == null)
            {
                return;
            }

            //
            FemDesign.Shells.Slab obj = FemDesign.Reinforcement.SurfaceReinforcement.AddReinforcementToSlab(slab, surfaceReinforcement);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.RebarAddToElement;
                ;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ab77cb64-5d15-4785-ba0d-ce5307efb873"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }   
}