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
            // This example will show you how to run an analysis
            // with a given model.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


            // LOADING UP THE MODEL
            string struxmlPath = "exbeam.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);


            // CHOOSING WHAT ANALYSIS TO RUN
            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(
                calcCase: true,
                calcComb: true
                );


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
            model.Entities.Loads.LoadCombinations.ForEach(lComb =>
            {
                lComb.CombItem = combItem;
            });

            analysis.SetLoadCombinationCalculationParameters(model);

            // If you want you may ask FEM-Design for results in different units
            var units = FemDesign.Results.UnitResults.Default();
            units.Displacement = Results.Displacement.mm;


            // RUN THE ANALYSIS
            using (var femDesign = new FemDesignConnection(outputDir: "My analyzed model", keepOpen: true))
            {
                // Run analysis and red some results
                femDesign.RunAnalysis(model, analysis);
                var results = femDesign.GetResults<Results.NodalDisplacement>(units);
                
                // Display summary of results
                Console.WriteLine("Max nodal displacement per case/comb:");

                Console.WriteLine();
                Console.WriteLine("exbeam.struxml");
                foreach (var group in results.GroupBy(r => r.CaseIdentifier))
                {
                    double min = group.Min(r => r.Ez);
                    Console.WriteLine($"{group.Key}: {min:0.000}{units.Displacement}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
        }
    }
}
