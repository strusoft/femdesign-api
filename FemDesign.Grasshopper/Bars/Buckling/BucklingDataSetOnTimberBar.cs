// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class Bars_BucklingDataSetOnTimberBar: GH_Component
    {
        public Bars_BucklingDataSetOnTimberBar(): base("BucklingData.SetOnTimberBar", "SetOnTimberBar", "Set BucklingData on a Timber bar-element.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar. Timber bar element.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.FlexuralStiff", "FlexuralStiff", "BucklingLength definition in Flexural Stiff direction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.FlexuralWeak", "FlexuralWeak", "BucklingLength definition in Flexural Weak direction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("BucklingLength.LateralTorsional", "LateralTorsional", "BucklingLength definition for Lateral Torsional Buckling.", GH_ParamAccess.item);
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
            FemDesign.Bars.Buckling.BucklingLength lateralTorsional = null;
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
            if (!DA.GetData(3, ref lateralTorsional))
            {
                return;
            }

            //
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingData.SetOnTimberBar(bar, flexuralStiff, flexuralWeak, lateralTorsional));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BucklingDataSetOnTimberBar;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c7858a5d-08cf-46fc-9c8d-a1334e7a2a63"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}