// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class Bars_BucklingDataSetOnConcreteBar: GH_Component
    {
        public Bars_BucklingDataSetOnConcreteBar(): base("BucklingData.SetOnConcreteBar", "SetOnConcreteBar", "Set BucklingData on a Concrete bar-element.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar. Concrete bar element.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.FlexuralStiff", "FlexuralStiff", "BucklingLength definition in Flexural Stiff direction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.FlexuralWeak", "FlexuralWeak", "BucklingLength definition in Flexural Weak direction.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Bars.Bar bar = null;
            FemDesign.Bars.Buckling.BucklingLength flexuralStiff = null;
            FemDesign.Bars.Buckling.BucklingLength flexuralWeak = null;
            if (!DA.GetData(0, ref bar))
            {
                return;
            }
            if (!DA.GetData(1, ref flexuralStiff))
            {
                return;
            }
            if (!DA.GetData(2, ref flexuralWeak))
            {
                return;
            }

            //
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingData.SetOnConcreteBar(bar, flexuralStiff, flexuralWeak));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BucklingDataSetOnConcreteBar;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("38ce2ef2-d4b6-4b0b-b2e1-369529cf5d75"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}