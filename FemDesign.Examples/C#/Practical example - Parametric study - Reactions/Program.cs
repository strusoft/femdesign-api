using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Calculate;
using FemDesign.Materials;
using FemDesign.Results;
using FemDesign.Shells;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // PRACTICAL EXAMPLE: PARAMETRIC STUDY - REACTIONS
            // In this example, we will analyse how different E-modules will result
            // in different reaction forces in the supports holding a concrete plate.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


            // READ MODEL
            string struxmlPath = "sample_slab.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            // READ SLAB TO ANALYSE
            Slab slab = model.Entities.Slabs.Find(s => s.Name == "P.1");
            Material material = slab.Material;
            double Ecm = double.Parse(material.Concrete.Ecm);

            // ITERATION & ANALYSIS PROCESS
            Analysis analysis = new Analysis(calcCase: true);

            using (var femDesign = new FemDesignConnection(minimized: true))
                for (int i = 1; i < 6; i++)
                {
                    // Change E-modulus
                    double new_Ecm = Math.Round(0.2 * i * Ecm);
                    material.Concrete.Ecm = new_Ecm.ToString();

                    // Run analysis and get point support reactions
                    femDesign.Open(model);
                    femDesign.RunAnalysis(analysis);
                    var pointSupportReactions = femDesign.GetResults<PointSupportReaction>();

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
