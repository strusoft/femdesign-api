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
            // PRACTICAL EXAMPLE: PARAMETRIC STUDY - SENSITIVITY ANALYSIS
            // In this example, we will analyse how different stiffness values on a support
            // will affect a bridge (modelled simply as a beam).

            // This example was last updated using the ver. 21.6.0 FEM-Design API.

            // FILE PATH SETUP
            // Set the different paths and folders relevant to the example
            string struxmlPath = "Bridge Model.struxml";

            // READ MODEL AND GET SUPPORTS
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            //Read point support number and its stiffness properties
            var support1 = model.Entities.Supports.PointSupport.FirstOrDefault(p => p.Name == "S.1");
            var support2 = model.Entities.Supports.PointSupport.FirstOrDefault(p => p.Name == "S.2");
            double alpha = 0.5;

            var analysis = Calculate.Analysis.Eigenfrequencies(5, 0, false, false, true, -0.01);
            using (var app = new FemDesign.FemDesignConnection())
            {
                // ITERATION AND ANALYSIS PROCESS
                // Iterate over model using different stiffness value for the the rotational spring cy
                int N = 20;
                for (int i = 1; i <= N; i++)
                {
                    // Change stiffness of support cy
                    support1.Rotations.YPos = Math.Pow(10, alpha);
                    support1.Rotations.YNeg = Math.Pow(10, alpha);

                    support2.Rotations.YPos = Math.Pow(10, alpha);
                    support2.Rotations.YNeg = Math.Pow(10, alpha);

                    var supports = new List<Supports.PointSupport>() { support1, support2 };
                    model.AddElements(supports);

                    // Run analysis
                    app.RunAnalysis(model, analysis);
                    var results = app.GetResults<Results.EigenFrequencies>();
                    // Read results from csv file (general method)
                    {
                        Console.WriteLine("");
                        Console.WriteLine(string.Format("Alpha: {0}", alpha));
                        Console.WriteLine(results[0]);
                    }
                    alpha = alpha + 0.5;
                }
            }


            // ENDING THE PROGRAM
            Console.WriteLine("\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
