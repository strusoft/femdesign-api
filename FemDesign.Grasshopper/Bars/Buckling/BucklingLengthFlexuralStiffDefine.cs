// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class BarsBucklingLengthFlexuralStiffDefine: GH_Component
    {
        public BarsBucklingLengthFlexuralStiffDefine(): base("BucklingLength.FlexuralStiffDefine", "FlexuralStiffDefine", "Define BucklingLength in Flexural Stiff direction.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Beta", "Beta", "Beta factor.", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Sway", "Sway", "Sway. True/false", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BucklingLength", "BucklingLength", "BucklingLength.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double beta = 1;
            bool sway = false;
            if (!DA.GetData(0, ref beta))
            {
                // pass
            }
            if (!DA.GetData(1, ref sway))
            {
                // pass
            }
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingLength.FlexuralStiff(beta, sway));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.FlexuralStiff;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("77425a82-a76c-4306-8de6-96dd2dc6fdeb"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}