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
            // PRACTICAL EXAMPLE: PARAMETRIC STUDY - REACTIONS
            // In this example, we will analyse how different E-modules will result
            // in different reaction forces in the supports holding a concrete plate.

            // This example was last updated 2022-05-03, using the ver. 21.1.0 FEM-Design API.


            // FILE PATH SETUP
            // Set the different paths and folders relevant to the example
            string struxmlPath = "sample_slab.struxml";
            string outFolder = "output/";
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);
            string bscPath = Path.GetFullPath("pointsupportreactions.bsc");
            List<string> bscPaths = new List<string>();
            bscPaths.Add(bscPath);

            // READ MODEL
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            // READ SLAB TO ANALYSE
            // In this example, the slab is card-coded to no. 5; if you make any personal applications,
            // it is probably better to look for a slab with a certain name, eg. P.1, to avoid confusion.
            Shells.Slab slab = model.Entities.Slabs[4];
            Materials.Material material = model.Entities.Slabs[4].Material;
            double Ecm = Convert.ToDouble(material.Concrete.Ecm);

            // ITERATION & ANALYSIS PROCESS
            // Iterate over model using different E-modulus for the slab
            for (int i = 1; i < 6; i++)
            {
                // Change E-modulus
                double new_Ecm = Math.Round(0.2 * i * Ecm);
                material.Concrete.Ecm = Convert.ToString(new_Ecm);

                // Save struxml
                string outPathIndividual = Path.GetFullPath(outFolder + "sample_slab_out" + Convert.ToString(new_Ecm) + ".struxml");
                model.SerializeModel(outPathIndividual);

                // Run analysis
                Calculate.Analysis analysis = new Calculate.Analysis(null, null, null, null, calcCase: true, false, false, false, false, false, false, false, false, false, false, false, false);
                FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(outPathIndividual, analysis, bscPaths, "", true);
                Calculate.Application app = new Calculate.Application();
                app.RunFdScript(fdScript, false, true, true);

                string pointSupportReactionsPath = Path.Combine(outFolder, "pointsupportreactions.csv");

                // Reading results (This method is only available for some result types as of now, but more will be added)
                var results = Results.ResultsReader.Parse(pointSupportReactionsPath);
                var pointSupportReactions = results.Cast<Results.PointSupportReaction>().ToList();

                // Print results
                Console.WriteLine();
                Console.WriteLine($"Emean: {new_Ecm}");
                Console.WriteLine("Id         | Reaction  ");
                foreach (var reaction in pointSupportReactions)
                {
                    Console.WriteLine($"{reaction.Id,10} | {reaction.Fz,10}");
                }
            }

            // ENDING THE PROGRAM
            Console.WriteLine("\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
