using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 2: ANALYSING A MODEL
            // This example will show you how to run an analyse
            // with a given model from within a C# script.

            // This example was last updated 2022-05-03, using the ver. 21.1.0 FEM-Design API.

            // Loading up the model
            string struxmlPath = "exbeam.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);


            // Setup by load combination
            bool NLE = true;
            bool PL = true;
            bool NLS = false;
            bool Cr = false;
            bool _2nd = false;

            // SETTING UP LOAD COMBINATIONS
            // In this example, we use the same settings (CombItem)
            // for all load combinations, applied with a simple loop.
            var combItem = new FemDesign.Calculate.CombItem(0, 0, NLE, PL, NLS, Cr, _2nd);

            int numLoadCombs = model.Entities.Loads.LoadCombinations.Count;
            var combItems = new List<Calculate.CombItem>();
            for (int i = 0; i < numLoadCombs; i++)
            {
                combItems.Add(combItem);
            }

            Calculate.Comb comb = new Calculate.Comb();
            comb.CombItem = combItems.ToList();

            // Choosing the analysis settings
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

            // Running the analysis using an FdScript
            FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(Path.GetFullPath(struxmlPath), analysis, null, "", true);
            Calculate.Application app = new Calculate.Application();
            app.RunFdScript(fdScript, false, true, true);
        }
    }
}
