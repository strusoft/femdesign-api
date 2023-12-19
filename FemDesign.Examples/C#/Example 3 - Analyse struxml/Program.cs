using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Calculate;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // EXAMPLE 2: ANALYSING A MODEL
            // This example will show you how to run an analysis
            // with a given model.

            // This example was last updated using the ver. 22.11.0 FEM-Design API.


            // LOADING UP THE MODEL
            // The file can be .struxml or .str
            string struxmlPath = "exbeam.struxml";


            // CHOOSING WHAT ANALYSIS TO RUN
            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(
                calcCase: true,
                calcComb: true
                );


            // If you want you may ask FEM-Design for results in different units
            var units = FemDesign.Results.UnitResults.Default();
            units.Displacement = Results.Displacement.mm;


            // RUN THE ANALYSIS
            using (var femDesign = new FemDesignConnection(outputDir: "My analyzed model", keepOpen: true))
            {
                // Run analysis and red some results
                femDesign.Open(struxmlPath);
                femDesign.RunAnalysis(analysis);
                var results = femDesign.GetResults<Results.NodalDisplacement>(units);
                
                // Display summary of results
                Console.WriteLine("Max nodal displacement per case/comb:");

                Console.WriteLine();
                Console.WriteLine(struxmlPath);
                foreach (var group in results.GroupBy(r => r.CaseIdentifier))
                {
                    double min = group.Min(r => r.Ez);
                    Console.WriteLine($"{group.Key}: {min:0.000}{units.Displacement}");
                }
            }
        }
    }
}
