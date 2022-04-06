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
        /// NOTE: CHANGE THIS TO YOUR LOCAL MATERIALS
        /// </summary>
        static readonly string MaterialsPath = @"C:\Users\SamuelNyberg\OneDrive - StruSoft AB\Samuels arbetshörna\materialochsektioner\materials.struxml";

        /// <summary>
        /// NOTE: CHANGE THIS TO YOUR LOCAL SECTIONS
        /// </summary>
        static readonly string SectionsPath = @"C:\Users\SamuelNyberg\OneDrive - StruSoft AB\Samuels arbetshörna\materialochsektioner\sections.struxml";

        private static void Main(string[] args)
        {
            string samplesDir = System.IO.Directory.GetCurrentDirectory();
            if (samplesDir.EndsWith("bin\\Debug\\net5.0") || samplesDir.EndsWith("bin\\Release\\net5.0"))
                samplesDir = Path.GetFullPath(Path.Combine(samplesDir, "..\\..\\..\\"));
            System.IO.Directory.SetCurrentDirectory(samplesDir);

            Console.WriteLine("Running some sample code using the FEM-Design api!");

            // ChangeLengthOfBeam();
            // Example1CreateSimpleModel();
            Example4EditExistingModel();
            // RunAnalysis();
            // CreatePostTensionedCable();
            // CreateLoadGroups();
            // LoadGroupsCombine();
            // ParametricStudy();
            // CostOptimizationOfSlab();
        }
    }
}
