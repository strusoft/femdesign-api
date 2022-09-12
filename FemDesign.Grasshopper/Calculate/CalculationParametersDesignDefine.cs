// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersDesignDefine : GH_Component
    {
        public CalculationParametersDesignDefine() : base("Design.Define", "Define", "Set parameters for design.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("autoDesign", "autoDesign", "Auto-design elements.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("check", "check", "Check elements.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("BasedOn", "BasedOn", "Based on analysis of LoadCombination or LoadGroup.\nTrue: Load Combination\nFalse: Load Group", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("applyChanges", "applyChanges", "Force FemDesign to apply the new cross sections to the model at the end of the design process.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool autoDesign = true;
            DA.GetData(0, ref autoDesign);

            bool check = true;
            DA.GetData(1, ref check);

            bool isLoadCombination = true;
            DA.GetData(2, ref isLoadCombination);

            bool applychanges = true;
            DA.GetData(3, ref applychanges); //uncomment the line when we get response from the developer.

            FemDesign.Calculate.Design _obj = new FemDesign.Calculate.Design(autoDesign, check, isLoadCombination, applychanges);

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
            get { return new Guid("{178C9BD7-9242-43EE-861C-F3E62E94DCB9}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}