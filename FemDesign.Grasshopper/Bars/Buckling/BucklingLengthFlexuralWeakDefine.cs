// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class BarsBucklingLengthFlexuralWeakDefine: GH_Component
    {
        public BarsBucklingLengthFlexuralWeakDefine(): base("BucklingLength.FlexuralWeakDefine", "FlexuralWeakDefine", "Define BucklingLength in Flexural Weak direction.", CategoryName.Name(), SubCategoryName.Cat2a())
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
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingLength.FlexuralWeak(beta, sway));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.FlexuralWeak;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("814cc655-dd47-433f-8b0c-5e87c74022b3"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}