using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace MeshSizeConvergenceStudy
{
    internal class Program
    {
        static void Main()
        {
            // Input
            string modelPath = "Model with labelled sections.struxml";
            string outputDirectory = "Output";
            double maxAllowedDeviation = 0.01; // 1%
            string labeledSectionIdentifier = "LS.1";
            List<double> meshSizes = new List<double> { 2.0, 1.0, 0.5, 0.25, 0.20, 0.15, 0.10 };

            // Preparations
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            string fileName = Path.GetFileName(modelPath);
            FemDesign.Calculate.Analysis analysisSettings = new FemDesign.Calculate.Analysis(null, null, null, null, calcCase: true, false, false, calcComb: true, false, false, false, false, false, false, false, false, false);
            Model model = Model.DeserializeFromFilePath(modelPath);

            Dictionary<double, List<double>> allForces = new Dictionary<double, List<double>>() {};

            // Run convergence study
            foreach (double size in meshSizes)
            {
                model.Entities.Slabs[0].SlabPart.MeshSize = size;

                // Serializing new model
                string currentPath = Path.GetFullPath(Path.Combine(outputDirectory, fileName.Replace(".struxml", $"_{size}.struxml")));
                model.SerializeModel(currentPath);

                // Readying the .bsc script
                string bscPath = Path.Combine(outputDirectory, fileName.Replace(".struxml", $" - LabelledSectionsInternalForcesLoadCombination.bsc"));
                var bsc = new FemDesign.Calculate.Bsc(FemDesign.Calculate.ListProc.LabelledSectionsInternalForcesLoadCombination, bscPath);

                // Running the analysis
                FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(currentPath, analysisSettings, new List<string> { bsc }, null, true);
                var app = new FemDesign.Calculate.Application();
                app.RunFdScript(fdScript, false, true, true);

                // Preprarations
                string csvPath = bscPath.Replace(".bsc", ".csv");
                List<double> currentForces = new List<double>();

                // Reading results

                // DEt är något fel med den här biten: den hämtar samma lista på värden varje gång
                using (var reader = new StreamReader(csvPath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split('\t');
                        if (values[0] == labeledSectionIdentifier)
                        {
                            currentForces.Add(double.Parse(values[8], System.Globalization.CultureInfo.InvariantCulture));
                        }
                    }
                }
                Console.WriteLine("\nSize: {0}\nTx'z':", size);
                foreach (double value in currentForces) { Console.WriteLine(value); }
                allForces.Add(size, currentForces);

            }

            // Calculation preparation
            int counter = 0;
            List<double> totalForce = new List<double>();
            List<double> previousForces = new List<double>();
            List<double> totalDeviation = new List<double>();
            List<double> deviationCalculation = new List<double>();
            List<double> deviationPercentage = new List<double>();

            // Calculation loop

            // TODO: The calculations aren't correct. Revisit!
            foreach (var forcesList in allForces)
            {
                if (counter > 0)
                {
                    for (int i = 0; i < forcesList.Value.Count; i++)
                    {
                        deviationCalculation.Add(Math.Abs(forcesList.Value[i]-previousForces[i]));
                    }
                    totalDeviation.Add(deviationCalculation.Sum());
                    deviationCalculation.Clear();
                }
                else
                {
                    totalDeviation.Add(0);
                }
                totalForce.Add(forcesList.Value.Sum());
                deviationPercentage.Add(totalDeviation[counter] / totalForce[counter]);

                Console.WriteLine("{0}\t{1}\t{2}\t{3}", forcesList.Key, totalForce[counter], totalDeviation[counter], deviationPercentage[counter]);

                previousForces.AddRange(forcesList.Value);
                counter++;
            }


            Console.ReadKey();
        }
    }
}
