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
            // In this case a beam will be auto-designed.
            Model model = Model.DeserializeFromFilePath("my beam.struxml");

            // Set up the analysis
            var analysis = Calculate.Analysis.StaticAnalysis();
            var design = new Calculate.Design(autoDesign: true, applyChanges: true);

            var units = Results.UnitResults.Default();
            units.Displacement = Results.Displacement.mm;

            var outputLog = new List<string>();
            List<Results.BarDisplacement> displacements;
            Model newModel;

            // Run analysis and design calculations
            using (var femDesign = new FemDesignConnection(keepOpen: true))
            {
                // In this example we will also read the log output from FEM-Design.
                // We do so by adding an event handler that saves the output messages from FEM-Design
                femDesign.SetVerbosity(Verbosity.InputOnly);
                femDesign.OnOutput += (message) => outputLog.Add(message);

                femDesign.Open(model);

                // First we run the analysis and the design calculations with auto-design
                femDesign.RunAnalysis(analysis);
                femDesign.RunDesign(Calculate.CmdUserModule.STEELDESIGN, design);

                // After using auto-design we need to re-calculate the model to update the distribution of the forces
                femDesign.RunAnalysis(analysis);

                // Finally we read the updated model and get some results
                newModel = femDesign.GetModel();
                displacements = femDesign.GetResults<Results.BarDisplacement>(units);

                // If we want to save the results we must save the file as a .str
                femDesign.Save("my auto-designed beam.str");
            }

            // Compare the results
            string originalSection = model.Entities.Bars[0].BarPart.ComplexSectionObj.Sections[0].Name;
            string newSection = newModel.Entities.Bars[0].BarPart.ComplexSectionObj.Sections[0].Name;

            Console.WriteLine($"Original section:    '{originalSection}'");
            Console.WriteLine($"Auto-design section: '{newSection}'");
            Console.WriteLine();
            Console.WriteLine("Max nodal displacement per case/comb:");
            Console.WriteLine();
            foreach (var group in displacements.GroupBy(r => r.CaseIdentifier))
            {
                double min = group.Min(r => r.Ez);
                string caseOrCombName = group.Key;
                Console.WriteLine($"{caseOrCombName}: {min:0.000}{units.Displacement}");
            }

            // Here, we simply output the log messages to the user, but we could just as well save it to a file if we want
            Console.WriteLine();
            Console.WriteLine("Log:");
            foreach (var line in outputLog) Console.WriteLine(line);

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
