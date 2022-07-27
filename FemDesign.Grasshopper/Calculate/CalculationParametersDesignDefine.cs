// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersDesignDefine: GH_Component
    {
        public CalculationParametersDesignDefine(): base("Design.Define", "Define", "Set parameters for design.", "FEM-Design", "Calculate")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("autoDesign", "autoDesign", "Auto-design elements.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("check", "check", "Check elements.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("BasedOn", "BasedOn", "Based on analysis of LoadCombination or LoadGroup.\nTrue: Load Combination\nFalse: Load Group", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool autoDesign = false, check = true; bool isLoadCombination = true;
            DA.GetData(0, ref autoDesign);
            DA.GetData(1, ref check);
            DA.GetData(2, ref isLoadCombination);

            FemDesign.Calculate.Design _obj = new FemDesign.Calculate.Design(autoDesign, check, isLoadCombination);

            DA.SetData(0, _obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.DesignDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("97749fff-76f9-4ff1-8f12-e8b5c5a7a471"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}