using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void RunAnalysis()
        {
            string struxmlPath = @"C:\Users\JohannaRiad\OneDrive - StruSoft AB\Desktop\Webinar - intro FEM design API\exbeam.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);


            // Setup for calculation of load combinations
            bool NLE = true;
            bool PL = false;
            bool NLS = false;
            bool Cr = false;
            bool _2nd = false;
            // Skapar inställningar för analysen
            var combItem = new FemDesign.Calculate.CombItem(0, 0, NLE, PL, NLS, Cr, _2nd);

            int numLoadCombs = model.Entities.Loads.LoadCombinations.Count;
            var combItems = new List<Calculate.CombItem>();
            for (int i = 0; i < numLoadCombs; i++)
            {
                combItems.Add(combItem);
            }

            // Behövs för datastrukturens skull
            Calculate.Comb comb = new Calculate.Comb();
            comb.CombItem = combItems.ToList();

            // Start analysis
            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(
                stage: null,
                comb: comb,
                freq: null,
                footfall: null,
                calcCase: true,
                calcCStage: false,
                calcImpf: false,
                calcComb: true,
                calcGMax: false,
                calcStab: false,
                calcFreq: false,
                calcSeis: false,
                calcDesign: false,
                calcFootfall: false,
                elemFine: false,
                diaphragm: false,
                peakSmoothing: false
                );

            FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(struxmlPath, analysis, null, "", true);
            Calculate.Application app = new Calculate.Application();
            app.RunFdScript(fdScript, false, true, true);
        }
    }
}
