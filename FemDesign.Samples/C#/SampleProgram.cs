using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FemDesign;


namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        /// <summary>
        /// Change this to your path!
        /// </summary>
        static readonly string MaterialsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StruSoft", "FEM-Design Api", "materialLibrary", "materials_S.struxml");

        /// <summary>
        /// Change this to your path!
        /// </summary>
        static readonly string SectionsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StruSoft", "FEM-Design Api", "sectionLibrary", "sections.struxml");

        private static void Main(string[] args)
        {
            string samplesDir = System.IO.Directory.GetCurrentDirectory();
            if (samplesDir.EndsWith("bin\\Debug\\net5.0") || samplesDir.EndsWith("bin\\Release\\net5.0"))
                samplesDir = Path.GetFullPath(Path.Combine(samplesDir, "..\\..\\..\\"));
            System.IO.Directory.SetCurrentDirectory(samplesDir);

            Console.WriteLine("Running some sample code using the FEM-Design api!");

            //TestGetResults();
            CreatePostTensionedCable();
        }

        private static void TestGetResults()
        {
            // Create batch file to extract the results with
            string bscPath = "Local/Nodal displacements.bsc";
            var bsc = new FemDesign.Calculate.Bsc(Calculate.ResultType.NodalDisplacementsLoadCombination, bscPath);

            // Run FEM-Design script to extract results
            string modelPath = System.IO.Path.GetFullPath("Local/Sample.str");
            var fdscript = FemDesign.Calculate.FdScript.ReadStr(modelPath, new List<string>() { bsc });

            var app = new FemDesign.Calculate.Application();
            bool hasExited = app.RunFdScript(fdscript, false, true, true);

            // Read results from generated results files
            string resultsPath = fdscript.CmdListGen[0].OutFile;
            var results = Results.ResultsReader.Parse(resultsPath);

            // Print all results
            for (int i = 0; i < Math.Min(results.Count, 100); i++)
            {
                Console.WriteLine($"{i}: {results[i]}");
            }

            //var liveloads = results.Cast<FemDesign.Results.NodalDisplacement>().Where(r => r.CaseIdentifier == "Liveload" && r.Id == "S.1").ToList();
            var liveloads = results.Cast<FemDesign.Results.NodalDisplacement>().Where(r => r.CaseIdentifier == "SLS" && r.Id == "S.1").ToList();
            for (int i = 0; i < liveloads.Count; i++)
            {
                Console.WriteLine($"{i}: {liveloads[i]}");
            }
        }
    }
}
