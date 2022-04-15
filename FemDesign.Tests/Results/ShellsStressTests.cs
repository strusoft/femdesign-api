using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results.Tests
{
    [TestClass()]
    public class ShellsStressTests
    {
        [TestMethod]
        public void Parse()
        {
            string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\312-shell-stresses\Stress_output_extractAndNot.txt";
            // string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\312-shell-stresses\Stress_output-Not Extract.txt";
            // string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\312-shell-stresses\Stress_output_extract.txt";

            var results = ResultsReader.Parse(file_path);

            Console.WriteLine(results);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shells, Stresses, top, Ultimate - Load case: deadload - for selected objects",
                "Shells, Stresses, bottom (Extract), Ultimate - Load case: deadload - for selected objects",
            };

            foreach (var header in headers)
            {
                var match = ShellStress.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(ShellStress).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shells, Stresses, top (Extract), Ultimate - Load case: deadload - for selected objects",
                "Shell	Max.	Elem	Node	Sigma x'	Sigma y'	Tau x'y'	Tau x'z'	Tau y'z'	Sigma vm	Sigma 1	Sigma 2	alpha	Case",
                "Shell	Elem	Node	Sigma x'	Sigma y'	Tau x'y'	Tau x'z'	Tau y'z'	Sigma vm	Sigma 1	Sigma 2	alpha",
                "[-]	[-]	[-]	[-]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[rad]	[-]"
            };

            foreach (var header in headers)
            {
                var match = ShellStress.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}