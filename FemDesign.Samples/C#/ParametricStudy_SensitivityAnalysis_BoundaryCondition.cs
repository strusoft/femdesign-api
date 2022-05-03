using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Paramatric study of a bridge
// The rotiational spring, cy, varies from free to rigid

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void ParametricStudy_SensitivityAnalysis_BoundaryCondition()
        {
            //Set the different paths and folders relevant to the example
            string struxmlPath = "ExampleModels/Bridge/Bridge Model.struxml";
            string outFolder = "ExampleModels/Bridge/";
            string bscPath = Path.GetFullPath("ExampleModels/Bridge/eigenfreq.bsc");
            List<string> bscPaths = new List<string>();
            bscPaths.Add(bscPath);

            //Read original struxml model
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            //Read point support number  and its stiffness properties
            var support1 = model.Entities.Supports.PointSupport.FirstOrDefault(p => p.Name == "S.1");
            var support2 = model.Entities.Supports.PointSupport.FirstOrDefault(p => p.Name == "S.2");
            double alpha = 0.5;

            //Iterate over model using different stiffness value for the the rotational sprig cy
            int N = 20;
            for (int i = 1; i <= N; i++)
            {
                //Change stiffness of support cy
                support1.Rotations.YPos = Math.Pow(10,alpha);
                support1.Rotations.YNeg = Math.Pow(10,alpha);

                support2.Rotations.YPos = Math.Pow(10,alpha);
                support2.Rotations.YNeg = Math.Pow(10,alpha);

                var supports = new List<Supports.PointSupport>() { support1, support2 };
                model.AddElements(supports);

                //Save struxml
                string outPathIndividual = Path.GetFullPath(outFolder + "Bridge Model_out" + Convert.ToString(alpha,System.Globalization.CultureInfo.InvariantCulture) + ".struxml");
                model.SerializeModel(outPathIndividual);


                //Run analysis
                var freq = new Calculate.Freq(5, 0, false, false, true, -0.01);
                Calculate.Analysis analysis = new Calculate.Analysis(null, null, freq, null, true, false, false, false, false, false, true, false, false, false, false, false, false);
                FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(outPathIndividual, analysis, bscPaths, "", true);
                Calculate.Application app = new Calculate.Application();
                app.RunFdScript(fdScript, false, true, true);

                //Read results from csv file (general method)
                int counter = 0;

                using (var printer = new StreamWriter("ExampleModels/Bridge/eigenfreq.txt", true))
                using (var reader = new StreamReader("ExampleModels/Bridge/eigenfreq.csv"))
                {
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("{0} {1}", "Alpha: ", alpha));

                    printer.WriteLine("");
                    printer.WriteLine(string.Format("{0} {1}", "Alpha: ", alpha));

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split('\t');
                        if (counter>0 & line!="")
                        {
                            Console.WriteLine(line);
                            printer.WriteLine(line);
                        }                       
                        counter++;
                    }                 
                }
                alpha = alpha + 0.5;            
            }
        }
    }
}
