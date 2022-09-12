// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersFootfallDefine: GH_Component
    {
        public CalculationParametersFootfallDefine(): base("Footfall.Define", "Define", "Define calculation parameters for an footfall calculation.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Top", "Top", "Top of substructure. Masses on this level and below are not considered in Footfall calculation. [m]", GH_ParamAccess.item, -0.01);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Footfall", "Footfall", "Footfall.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double top = -0.01;
            if (!DA.GetData(0, ref top))
            {
                // pass
            }
            
            FemDesign.Calculate.Footfall obj = new Calculate.Footfall(top);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.CalculateFootfall;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("b0a30079-81b6-4a7c-b9b6-3c659a62a13f"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}