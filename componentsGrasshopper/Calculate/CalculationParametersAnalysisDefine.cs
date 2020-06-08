// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class CalculationParametersAnalysisDefine: GH_Component
    {
        public CalculationParametersAnalysisDefine(): base("Analysis.Define", "Define", "Set parameters for analysis.", "FemDesign", "Calculate")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Stage", "Stage", "Definition for construction stage calculation method. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Comb", "Comb", "Load combination calculation options. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;                        
            pManager.AddBooleanParameter("calcCase", "calcCase", "Load cases.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;            
            pManager.AddBooleanParameter("calcCstage", "calcCstage", "Construction stages.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcImpf", "calcImpf", "Imperfections", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcComb", "calcComb", "Load combinations", GH_ParamAccess.item, false);
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
            pManager.AddBooleanParameter("elemfine", "elemfine", "Fine or standard elements", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("diaphragm", "diaphragm", "Diaphragm calculation.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("peakSmoothing", "peakSmoothing", "Peak smoothing of internal forces", GH_ParamAccess.item, false);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Calculate.Stage stage = FemDesign.Calculate.Stage.Default();
            FemDesign.Calculate.Comb comb = FemDesign.Calculate.Comb.Default();
            bool calcCase = false;
            bool calcCstage = false;
            bool calcImpf = false;
            bool calcComb = false;
            bool calcGmax = false;
            bool calcStab = false;
            bool calcFreq = false;
            bool calcSeis = false;
            bool calcDesign = false;
            bool elemfine = false;
            bool diaphragm = false;
            bool peakSmoothing = false;

            if (!DA.GetData(0, ref stage))
            {
                // pass
            }
            if (!DA.GetData(1, ref comb))
            {
                // pass
            } 
            if (!DA.GetData(2, ref calcCase))
            {
                // pass
            }
            if (!DA.GetData(3, ref calcCstage))
            {
                // pass
            }
            if (!DA.GetData(4, ref calcImpf))
            {
                // pass
            }
            if (!DA.GetData(5, ref calcComb))
            {
                // pass
            }
            if (!DA.GetData(6, ref calcGmax))
            {
                // pass
            }
            if (!DA.GetData(7, ref calcStab))
            {
                // pass
            }
            if (!DA.GetData(8, ref calcFreq))
            {
                // pass
            }
            if (!DA.GetData(9, ref calcSeis))
            {
                // pass
            }
            if (!DA.GetData(10, ref calcDesign))
            {
                // pass
            }
            if (!DA.GetData(11, ref elemfine))
            {
                // pass
            }
            if (!DA.GetData(12, ref diaphragm))
            {
                // pass
            }
            if (!DA.GetData(13, ref peakSmoothing))
            {
                // pass
            }
            if (stage == null || comb == null)
            {
                // pass
            }

            //
            FemDesign.Calculate.Analysis obj = FemDesign.Calculate.Analysis.Define(stage, comb, calcCase, calcCstage, calcImpf, calcComb, calcGmax, calcStab, calcFreq, calcSeis, calcDesign, elemfine, diaphragm, peakSmoothing);

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
            get { return new Guid("96928633-7dca-4b63-9762-d1e6dc51806d"); }
        }
    }   
}