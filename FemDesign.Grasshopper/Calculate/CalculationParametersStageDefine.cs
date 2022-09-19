// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersStageDefine: GH_Component
    {
        public CalculationParametersStageDefine(): base("StageSetting.Define", "Define", "Define construction stage method", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("ghost", "ghost", "Ghost construction method. True/false. If false incremental method is used.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stage", "Stage", "Stage.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool ghost = false;
            if (!DA.GetData(0, ref ghost))
            {
                // pass
            }

            //
            FemDesign.Calculate.Stage obj = FemDesign.Calculate.Stage.Define(ghost);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Stages;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("fb75062e-e1b0-4d68-a8d3-2597dc4712fe"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}