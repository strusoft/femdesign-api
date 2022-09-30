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

            // This example was last updated using the ver. 21.4.0 FEM-Design API.


            // LOADING UP THE MODEL
            string struxmlPath = "exbeam.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);


            // SETUP BY LOAD CALCULATION SETTINGS
            // These settings are found in the FEM-Design calculation window.
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


            // CHOOSING THE ANALYSIS SETTINGS
            // These dictate which calculations to run.
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


            // RUN THE ANALYSIS VIA AN FDSCRIPT
            var bscPath = new List<string> { @"C:\GitHub\femdesign-api\FemDesign.Examples\C#\Example 2 - Analysing a model\bin\Debug\pointsupportreactions.bsc" };

            FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(Path.GetFullPath(struxmlPath), analysis, bscPath, "", true);
            fdScript.CmdGlobalCfg = Calculate.CmdGlobalCfg.Default();
            Calculate.Application app = new Calculate.Application();
            app.RunFdScript(fdScript, false, true, true);
        }
    }
}
