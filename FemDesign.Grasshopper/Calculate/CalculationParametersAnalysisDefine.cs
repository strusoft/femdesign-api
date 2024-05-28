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
            pManager.AddGenericParameter("Freq", "Freq", "Eigenfrequency calculation options.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Footfall", "Footfall", "Footfall calculation options.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;            
            pManager.AddGenericParameter("GroundAcc", "GroundAcc", "Ground acceleration calculation options.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ExForce", "ExForce", "Excitation force calculation options.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("PeriodicEx", "PeriodicEx", "Periodic excitation calculation options.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcCase", "calcCase", "Load cases.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcComb", "calcComb", "Load combinations", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcCstage", "calcCstage", "Construction stages.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcImpf", "calcImpf", "Imperfections", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcGmax", "calcGmax", "Maximum of load groups.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcStab", "calcStab", "Stability analysis.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcFreq", "calcFreq", "Eigenfrequencies", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcSeis", "calcSeis", "Seismic analysis.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcFootfall", "calcFootfall", "Footfall analysis", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcGroundAcc", "calcGroundAcc", "Time history, ground acceleration analysis.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcExForce", "calcExForce", "Time history, excitation force analysis.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcPeriodicEx", "calcPeriodicEx", "Periodic excitation analysis.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcDesign", "calcDesign", "Design calculations", GH_ParamAccess.item, false);
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
            if (!DA.GetData("StageSetting", ref stage))
            {
                // pass
            }

            FemDesign.Calculate.Comb _comb = FemDesign.Calculate.Comb.Default();
            if (!DA.GetData("Comb", ref _comb))
            {
                // pass
            }

            FemDesign.Calculate.Stability _stability = null;
            if (!DA.GetData("Stability", ref _stability))
            {
                // pass
            }

            FemDesign.Calculate.Imperfection _imperfection = null;
            if (!DA.GetData("Imperfection", ref _imperfection))
            {
                //pass
            }

            FemDesign.Calculate.Freq _freq = null;
            if (!DA.GetData("Freq", ref _freq))
            {
                // pass
            }

            FemDesign.Calculate.Footfall _footfall = null;
            if (!DA.GetData("Footfall", ref _footfall))
            {
                // pass
            }

            FemDesign.Calculate.GroundAcc _groundAcc = null;
            if (!DA.GetData("GroundAcc", ref _groundAcc))
            {
                // pass
            }

            FemDesign.Calculate.ExcitationForce _exForce = null;
            if (!DA.GetData("ExForce", ref _exForce))
            {
                // pass
            }

            FemDesign.Calculate.PeriodicExcitation _periodicExcitation = null;
            if (!DA.GetData("PeriodicEx", ref _periodicExcitation))
            {
                // pass
            }

            bool calcCase = true;
            if (!DA.GetData("calcCase", ref calcCase))
            {
                // pass
            }

            bool calcComb = true;
            if (!DA.GetData("calcComb", ref calcComb))
            {
                // pass
            }

            bool calcCstage = false;
            if (!DA.GetData("calcCstage", ref calcCstage))
            {
                // pass
            }

            bool calcImpf = false;
            if (!DA.GetData("calcImpf", ref calcImpf))
            {
                // pass
            }


            bool calcGMax = false;
            if (!DA.GetData("calcGmax", ref calcGMax))
            {
                // pass
            }

            bool calcStab = false;
            if (!DA.GetData("calcStab", ref calcStab))
            {
                // pass
            }

            bool calcFreq = false;
            if (!DA.GetData("calcFreq", ref calcFreq))
            {
                // pass
            }

            bool calcSeis = false; // not implemented
            if (!DA.GetData("calcSeis", ref calcSeis))
            {
                // pass
            }

            bool calcFootfall = false;
            if (!DA.GetData("calcFootfall", ref calcFootfall))
            {
                // pass
            }

            bool calcGroundAcc = false;
            if (!DA.GetData("calcGroundAcc", ref calcGroundAcc))
            {
                // pass
            }

            bool calcExForce = false;
            if (!DA.GetData("calcExForce", ref calcExForce))
            {
                // pass
            }

            bool calcPeriodicEx = false;
            if (!DA.GetData("calcPeriodicEx", ref calcPeriodicEx))
            {
                // pass
            }

            bool calcDesign = true;
            if (!DA.GetData("calcDesign", ref calcDesign))
            {
                // pass
            }

            bool elemFine = true;
            if (!DA.GetData("elemfine", ref elemFine))
            {
                // pass
            }

            int diaphragm = 0;
            if (!DA.GetData("diaphragm", ref diaphragm))
            {
                // pass
            }

            bool peakSmoothing = false;
            if (!DA.GetData("peakSmoothing", ref peakSmoothing))
            {
                // pass
            }


            // Create Analysis
            FemDesign.Calculate.Analysis obj = new FemDesign.Calculate.Analysis(stage: stage, stability: _stability.DeepClone(), imperfection: _imperfection.DeepClone(), comb: _comb.DeepClone(), freq: _freq.DeepClone(), footfall: _footfall.DeepClone(), groundAcc: _groundAcc.DeepClone(), exForce: _exForce.DeepClone(), periodicEx: _periodicExcitation.DeepClone(), calcCase: calcCase, calcCStage: calcCstage, calcImpf: calcImpf, calcComb: calcComb, calcGMax: calcGMax, calcStab: calcStab, calcFreq: calcFreq, calcSeis: calcSeis, calcFootfall: calcFootfall, calcGroundAcc: calcGroundAcc, calcExForce: calcExForce, calcPeriodicEx: calcPeriodicEx, calcDesign: calcDesign, elemFine: elemFine, diaphragm: diaphragm, peakSmoothing: peakSmoothing);

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
            get { return new Guid("{79605F1D-5311-4315-B320-A4C691A29BDE}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}