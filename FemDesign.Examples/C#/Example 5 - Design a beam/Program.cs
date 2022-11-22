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
        static void Main(string[] args)
        {
            // EXAMPLE 5: Design a simple beam
            // This example will show you how to auto-design a beam in
            // FEM-Design. Before running, make sure you have a window
            // with FEM-Design open.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


            // Load a model
            // In this case a beam wich will be autodesigned.
            Model model = Model.DeserializeFromFilePath("my beam.struxml");
            

            // Set up the analysis
            var analysis = Calculate.Analysis.StaticAnalysis();
            var design = new Calculate.Design(autoDesign: true, applyChanges: true);

            var units = Results.UnitResults.Default();
            units.Displacement = Results.Displacement.mm;


            // Run analysis and design calculations
            using (var femDesign = new FemDesignConnection(keepOpen: true))
            {
                femDesign.Open(model);

                // First we run the analysis and the design calculations with auto-design
                femDesign.RunAnalysis(analysis);
                femDesign.RunDesign(Calculate.CmdUserModule.STEELDESIGN, design);

                // After using auto-design we need to re-calculate the model to update the distribution of the forces.
                femDesign.RunAnalysis(analysis);

                // Finally we can read and display the results
                var displacements = femDesign.GetResults<Results.BarDisplacement>(units);

                Console.WriteLine("Max nodal displacement per case/comb:");
                Console.WriteLine();
                Console.WriteLine("exbeam.struxml");
                foreach (var group in displacements.GroupBy(r => r.CaseIdentifier))
                {
                    double min = group.Min(r => r.Ez);
                    string caseOrCombName = group.Key;
                    Console.WriteLine($"{caseOrCombName}: {min:0.000}{units.Displacement}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
