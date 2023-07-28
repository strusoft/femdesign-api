using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.GenericClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using System.Diagnostics;
using System.IO;

namespace FemDesign.Examples
{
    [TestClass()]
    public class ExampleTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("Example_validation")]
        public void Examples()
        {
            int exitCode;

            // open an instance of FEM-Design
            var femDesign = new FemDesignConnection(minimized: true);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Example 1 - Creating a simple beam/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Example 2 - Analysing a model/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Example 3 - Analyse a beam/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Example 4 - Edit existing model/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Example 7 - Creating a wall/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Example10 - ConstructionStages/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Change length of beam/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Create post-tensioned cables/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Create Stiffness Point from a CSV/bin/Debug/net6.0");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Load groups/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Load groups combining/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Mesh size convergence study/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Parametric study - Reactions/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - Parametric study - Sensitivity analysis/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            exitCode = RunExample(@"../../../FemDesign.Examples/C#/Practical example - ResultsToJSON/bin/Debug");
            Assert.IsTrue(exitCode == 0);

            KillProcesses();

        }


        int RunExample(string path)
        {
            string fullPath;
            string exe;
            System.Diagnostics.Process process;
            int exitCode;

            var currentDir = System.IO.Directory.GetCurrentDirectory();
            fullPath = Path.GetFullPath(path);
            System.IO.Directory.SetCurrentDirectory(fullPath);

            exe = System.IO.Directory.GetFiles(fullPath, "*.exe")[0];
            process = Process.Start(exe);
            process.WaitForExit();
            exitCode = process.ExitCode;

            System.IO.Directory.SetCurrentDirectory(currentDir);

            return exitCode;
        }


        private void KillProcesses()
        {
            Process[] processes = Process.GetProcessesByName("fd3dstruct");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }


    }
}