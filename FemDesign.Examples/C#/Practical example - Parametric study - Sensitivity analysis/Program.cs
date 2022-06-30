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

            // This example was last updated 2022-05-03, using the ver. 21.1.0 FEM-Design API.


            // FILE PATH SETUP
            // Set the different paths and folders relevant to the example
            string struxmlPath = "Bridge Model.struxml";
            string bscPath = Path.GetFullPath("eigenfreq.bsc");
            List<string> bscPaths = new List<string>();
            bscPaths.Add(bscPath);


            // READ MODEL AND GET SUPPORTS
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            //Read point support number and its stiffness properties
            var support1 = model.Entities.Supports.PointSupport.FirstOrDefault(p => p.Name == "S.1");
            var support2 = model.Entities.Supports.PointSupport.FirstOrDefault(p => p.Name == "S.2");
            double alpha = 0.5;


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

                // Save struxml
                string outPathIndividual = Path.GetFullPath("Bridge Model_out" + Convert.ToString(alpha, System.Globalization.CultureInfo.InvariantCulture) + ".struxml");
                model.SerializeModel(outPathIndividual);


                // Run analysis
                var freq = new Calculate.Freq(5, 0, false, false, true, -0.01);
                Calculate.Analysis analysis = new Calculate.Analysis(null, null, freq, null, true, false, false, false, false, false, true, false, false, false, false, false, false);
                FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(outPathIndividual, analysis, bscPaths, "", true);
                Calculate.Application app = new Calculate.Application();
                app.RunFdScript(fdScript, false, true, true);

                // Read results from csv file (general method)
                {
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("Alpha: {0}", alpha));
                    string text = System.IO.File.ReadAllText(fdScript.CmdListGen[0].OutFile);
                    Console.WriteLine(text);
                }
                alpha = alpha + 0.5;
            }

            // ENDING THE PROGRAM
            Console.WriteLine("\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
