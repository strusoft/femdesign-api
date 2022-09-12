// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class Bars_BucklingDataSetOnSteelBar: GH_Component
    {
        public Bars_BucklingDataSetOnSteelBar(): base("BucklingData.SetOnSteelBar", "SetOnSteelBar", "Set BucklingData on a Steel bar-element.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar. Steel bar element.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.FlexuralStiff", "FlexuralStiff", "BucklingLength definition in Flexural Stiff direction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.FlexuralWeak", "FlexuralWeak", "BucklingLength definition in Flexural Weak direction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.PressuredTopFlange", "PressuredTopFlange", "BucklingLength definition for Pressured Top Flange.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.PressuredBottomFlange", "PressuredBottomFlange", "BucklingLength definition for Pressured Bottom Flange.", GH_ParamAccess.item);
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
            FemDesign.Bars.Buckling.BucklingLength pressuredTopFlange = null;
            FemDesign.Bars.Buckling.BucklingLength pressuredBottomFlange = null;
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
            if (!DA.GetData(3, ref pressuredTopFlange))
            {
                return;
            }
            if (!DA.GetData(4, ref pressuredBottomFlange))
            {
                return;
            }

            //
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingData.SetOnSteelBar(bar, flexuralStiff, flexuralWeak, pressuredTopFlange, pressuredBottomFlange));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BucklingDataSetOnSteelBar;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("5626a61b-2f13-4e7d-924c-de1b7955d8cf"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}