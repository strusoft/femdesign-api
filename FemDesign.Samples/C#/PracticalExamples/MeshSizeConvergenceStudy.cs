using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        public static void MeshSizeConvergenceStudy()
        {
            // Input
            string modelPath = @"C:\Users\SamuelNyberg\Documents\GitHub\femdesign-api\FemDesign.Samples\C#\ExampleModels\Model with labelled sections.struxml";
            string outputDirectory = @"C:\Users\SamuelNyberg\Documents\GitHub\femdesign-api\FemDesign.Samples\C#\ExampleModels\Output";
            double maxAllowedDeviation = 0.01; // 1%
            List<double> meshSizes = new List<double> { 2.0, 1.0, 0.5, 0.25, 0.20, 0.15, 0.10 };

            // Preparations
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            string fileName = Path.GetFileName(modelPath);
            Calculate.Analysis analysisSettings = new Calculate.Analysis(null, null, null, null, calcCase: true, false, false, calcComb: true, false, false, false, false, false, false, false, false, false);
            Model model = Model.DeserializeFromFilePath(modelPath);

            // Run convergence study
            foreach (double size in meshSizes)
            {
                Console.WriteLine("Checkpoint 1");

                string currentPath = Path.Combine(outputDirectory, fileName.Replace(".struxml", $"_{size}.struxml"));
                model.SerializeModel(currentPath);
                Console.WriteLine("Checkpoint 2");

                string bscPath = Path.Combine(outputDirectory, fileName.Replace(".struxml", $"labelledSectionsInternalForcesLoadCombination.bsc"));
                var bsc = new Calculate.Bsc(Calculate.ListProc.LabelledSectionsInternalForcesLoadCombination, bscPath);
                Console.WriteLine("Checkpoint 3");

                Calculate.FdScript fdScript = Calculate.FdScript.Analysis(currentPath, analysisSettings, new List<string> { bsc }, null, true );
                var app = new Calculate.Application();
                app.RunFdScript(fdScript, false, true, true);
                Console.WriteLine("Checkpoint 4");

                // Read results
            }
        }
    }
}
