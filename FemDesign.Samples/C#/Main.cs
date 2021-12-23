using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FemDesign;


namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void Main(string[] args)
        {
            string samplesDir = System.IO.Directory.GetCurrentDirectory();
            if (samplesDir.EndsWith("bin\\Debug\\net5.0") || samplesDir.EndsWith("bin\\Release\\net5.0"))
                samplesDir = Path.GetFullPath(Path.Combine(samplesDir, "..\\..\\..\\"));
            System.IO.Directory.SetCurrentDirectory(samplesDir);

            Console.WriteLine("Running some sample code using the FEM-Design api!");

            //TestGetResults();
            //ChangeLengthOfBeam();
            //RunAnalysis();
            //CreateLoadGroups();
            LoadGroupsCombine();
        }
    }
}
