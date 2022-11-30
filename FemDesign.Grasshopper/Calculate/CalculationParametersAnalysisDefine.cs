// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersAnalysisDefine: GH_Component
    {
        public CalculationParametersAnalysisDefine(): base("Analysis.Define", "Define", "Set parameters for analysis.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Stage", "Stage", "Definition for construction stage calculation method. Optional, if undefined default values will be used - for reference please see default values of Stage.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Comb", "Comb", "Load combination calculation options. Optional, if undefined default values will be used - for reference please see default values of Comb.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Freq", "Freq", "Eigienfrequency calculation options. Optional, if undefined default values will be used - for reference please see default values of Freq.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Footfall", "Footfall", "Footfall calculation options. Optional, if undefined default values will be used - for reference please see default values of Footfall.Define component.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;                         
            pManager.AddBooleanParameter("calcCase", "calcCase", "Load cases.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;            
            pManager.AddBooleanParameter("calcCstage", "calcCstage", "Construction stages.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcImpf", "calcImpf", "Imperfections", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("calcComb", "calcComb", "Load combinations", GH_ParamAccess.item, true);
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
            FemDesign.Calculate.Stage stage = null;
            if (!DA.GetData(0, ref stage))
            {
                // pass
            }

            
            FemDesign.Calculate.Comb comb = FemDesign.Calculate.Comb.Default();
            if (!DA.GetData(1, ref comb))
            {
                // pass
            } 

            FemDesign.Calculate.Freq freq = null;
            if (!DA.GetData(2, ref freq))
            {
                // pass
            }

            FemDesign.Calculate.Footfall footfall = null;
            if (!DA.GetData(3, ref footfall))
            {
                // pass
            }

            bool calcCase = false;
            if (!DA.GetData(4, ref calcCase))
            {
                // pass
            }

            
            bool calcCstage = false;
            if (!DA.GetData(5, ref calcCstage))
            {
                // pass
            }
            
            bool calcImpf = false;
            if (!DA.GetData(6, ref calcImpf))
            {
                // pass
            }
            
            bool calcComb = false;
            if (!DA.GetData(7, ref calcComb))
            {
                // pass
            }
            
            bool calcGMax = false;
            if (!DA.GetData(8, ref calcGMax))
            {
                // pass
            }
            
            bool calcStab = false;
            if (!DA.GetData(9, ref calcStab))
            {
                // pass
            }
            
            bool calcFreq = false;
            if (!DA.GetData(10, ref calcFreq))
            {
                // pass
            }
            
            bool calcSeis = false;
            if (!DA.GetData(11, ref calcSeis))
            {
                // pass
            }
            
            bool calcDesign = false;
            if (!DA.GetData(12, ref calcDesign))
            {
                // pass
            }

            bool calcFootfall = false;
            if (!DA.GetData(13, ref calcFootfall))
            {
                // pass
            }

            bool elemFine = false;
            if (!DA.GetData(14, ref elemFine))
            {
                // pass
            }
            
            bool diaphragm = false;
            if (!DA.GetData(15, ref diaphragm))
            {
                // pass
            }
            
            bool peakSmoothing = false;
            if (!DA.GetData(16, ref peakSmoothing))
            {
                // pass
            }

            if (stage == null || comb == null || freq == null)
            {
                // pass
            }

            //
            FemDesign.Calculate.Analysis obj = new FemDesign.Calculate.Analysis(stage, comb, freq, footfall, calcCase, calcCstage, calcImpf, calcComb, calcGMax, calcStab, calcFreq, calcSeis, calcDesign, calcFootfall, elemFine, diaphragm, peakSmoothing);

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

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }   
}