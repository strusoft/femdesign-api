// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersAnalysisDefine : FEM_Design_API_Component
    {
        public CalculationParametersAnalysisDefine() : base("Analysis.Define", "Analysis", "Set parameters for analysis.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("StageSetting", "StageSetting", "Definition for construction stage calculation method. Optional, if undefined default values will be used - for reference please see default values of Stage.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Comb", "Comb", "Load combination calculation options. Optional, if undefined default values will be used - for reference please see default values of Comb.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Stability", "Stability", "Stability calculation options.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Imperfection", "Imperfection", "Imperfection calculation options.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Freq", "Freq", "Eigienfrequency calculation options. Optional, if undefined default values will be used - for reference please see default values of Freq.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Footfall", "Footfall", "Footfall calculation options. Optional, if undefined default values will be used - for reference please see default values of Footfall.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcCase", "calcCase", "Load cases.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcComb", "calcComb", "Load combinations", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcCstage", "calcCstage", "Construction stages.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcImpf", "calcImpf", "Imperfections", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcGmax", "calcGmax", "Maximum of load groups.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcStab", "calcStab", "Stability analysis.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcFreq", "calcFreq", "Eigenfrequencies", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcSeis", "calcSeis", "Seismic analysis.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcDesign", "calcDesign", "Design calculations", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcFootfall", "calcFootfall", "Footfall analysis", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("elemfine", "elemfine", "Fine or standard elements", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("diaphragm", "diaphragm", "Diaphragm calculation. Connect 'ValueList' to get the options.\n\n'0'= None\n'1'= Rigid membrane\n'2'= Fully rigid", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("peakSmoothing", "peakSmoothing", "Peak smoothing of internal forces", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 17, new List<string> { "None", "Rigid membrane", "Fully rigid" }, new List<int> { 0, 1, 2 }, GH_ValueListMode.DropDown);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Calculate.Stage stage = null;
            if (!DA.GetData(0, ref stage))
            {
                // pass
            }


            FemDesign.Calculate.Comb _comb = FemDesign.Calculate.Comb.Default();
            if (!DA.GetData(1, ref _comb))
            {
                // pass
            }


            FemDesign.Calculate.Stability _stability = null;
            if (!DA.GetData(2, ref _stability))
            {
                // pass
            }


            FemDesign.Calculate.Imperfection _imperfection = null;
            if (!DA.GetData(3, ref _imperfection))
            {
                //pass
            }

            FemDesign.Calculate.Freq _freq = null;
            if (!DA.GetData(4, ref _freq))
            {
                // pass
            }

            FemDesign.Calculate.Footfall _footfall = null;
            if (!DA.GetData(5, ref _footfall))
            {
                // pass
            }

            bool calcCase = false;
            if (!DA.GetData("calcCase", ref calcCase))
            {
                // pass
            }

            bool calcComb = false;
            if (!DA.GetData("calcComb", ref calcComb))
            {
                // pass
            }

            bool calcCstage = false;
            if (!DA.GetData(8, ref calcCstage))
            {
                // pass
            }

            bool calcImpf = false;
            if (!DA.GetData(9, ref calcImpf))
            {
                // pass
            }


            bool calcGMax = false;
            if (!DA.GetData(10, ref calcGMax))
            {
                // pass
            }

            bool calcStab = false;
            if (!DA.GetData(11, ref calcStab))
            {
                // pass
            }

            bool calcFreq = false;
            if (!DA.GetData(12, ref calcFreq))
            {
                // pass
            }

            bool calcSeis = false;
            if (!DA.GetData(13, ref calcSeis))
            {
                // pass
            }

            bool calcDesign = false;
            if (!DA.GetData(14, ref calcDesign))
            {
                // pass
            }

            bool calcFootfall = false;
            if (!DA.GetData(15, ref calcFootfall))
            {
                // pass
            }

            bool elemFine = true;
            if (!DA.GetData(16, ref elemFine))
            {
                // pass
            }

            int diaphragm = 0;
            if (!DA.GetData(17, ref diaphragm))
            {
                // pass
            }

            bool peakSmoothing = false;
            if (!DA.GetData(18, ref peakSmoothing))
            {
                // pass
            }

            if (stage == null || _comb == null || _freq == null)
            {
                // pass
            }

            //
            FemDesign.Calculate.Analysis obj = new FemDesign.Calculate.Analysis(stage, _stability.DeepClone(), _imperfection.DeepClone(), _comb.DeepClone(), _freq.DeepClone(), _footfall.DeepClone(), calcCase, calcCstage, calcImpf, calcComb, calcGMax, calcStab, calcFreq, calcSeis, calcDesign, calcFootfall, elemFine, diaphragm, peakSmoothing);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.AnalysisDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{CC77B519-A3C2-44AC-BD6A-228E7000A836}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}