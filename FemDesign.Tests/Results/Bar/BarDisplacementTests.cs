using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using FemDesign.Calculate;

namespace FemDesign.Results
{
    [TestClass()]
    public class BarDisplacementTests
    {
        [TestMethod]
        public void Parse()
        {
            string modelPath = "Results\\Assets\\Model.str";

            string outDir = $"Results\\{nameof(BarDisplacementTests)}";
            string outPath = OutputFileHelper.GetCsvPath(outDir, ListProc.BarsDisplacementsLoadCase.ToString());

            List<BarDisplacement> results = new List<BarDisplacement>();
            using (var connection = new FemDesignConnection(outputDir: outDir, tempOutputDir: false))
            {
                connection.Open(modelPath);
                results = connection.GetLoadCaseResults<BarDisplacement>();
            }

            // Calculate count of results from file
            string[] header = new string[3];
            int rowCount = 0;
            int emptyRow = 0;
            using (var reader = new StreamReader(outPath))
            {
                while(!reader.EndOfStream)
                {
                    var row = reader.ReadLine();

                    if (row == "")
                    {
                        emptyRow++;
                    }
                    else if (rowCount < 3)
                    {
                        header[rowCount] = row;
                    }

                    rowCount++;
                }
            }
            int resultRow = rowCount - emptyRow * 4;


            // Check parsed data
            Assert.IsTrue(results[0].GetType() == typeof(BarDisplacement), "BarDisplacement should be parsed");
            Assert.IsTrue(results.Count == resultRow, "Should read all results.");

            // Check header
            foreach (var line in header)
            {
                var match1 = BarDisplacement.HeaderExpression.Match(line);
                Assert.IsTrue(match1.Success, $"Should identify \"{line}\" as header");
            }

            // Check identification
            var identifier = header[0];
            var match = BarDisplacement.IdentificationExpression.Match(identifier);
            Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(BarDisplacement).Name}");
            Assert.IsTrue(match.Groups["casename"].Success);
            Assert.IsTrue(match.Groups["type"].Success);
            Assert.IsTrue(match.Groups["result"].Success);
            Assert.IsTrue(match.Groups["loadcasetype"].Success);
            Assert.IsTrue(match.Groups["casecomb"].Success);
            Assert.IsTrue(match.Groups["casename"].Success);

            // Remove test files
            Directory.Delete(outDir, true);
        }

        //[TestMethod]
        //public void Identification()
        //{
        //    var headers = new string[]
        //    {
        //        "Bars, Displacements, Ultimate - Load case: IL",
        //        "Bars, Displacements, Quasi-Permanent - Load case: IL",
        //    };

        //    foreach (var header in headers)
        //    {
        //        var match = BarDisplacement.IdentificationExpression.Match(header);
        //        Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(BarDisplacement).Name}");
        //        Assert.IsTrue(match.Groups["casename"].Success);
        //        Assert.IsTrue(match.Groups["type"].Success);
        //        Assert.IsTrue(match.Groups["result"].Success);
        //        Assert.IsTrue(match.Groups["loadcasetype"].Success);
        //        Assert.IsTrue(match.Groups["casecomb"].Success);
        //        Assert.IsTrue(match.Groups["casename"].Success);
        //    }
        //}

        //[TestMethod]
        //public void Headers()
        //{
        //    var headers = new string[]
        //    {
        //        "Bars, Displacements, Ultimate - Load case: IL",
        //        "Bar	x	ex'	ey'	ez'	fix'	fiy'	fiz'",
        //        "[-]	[m]	[mm]	[mm]	[mm]	[rad]	[rad]	[rad]"
        //    };

        //    foreach (var header in headers)
        //    {
        //        var match = BarDisplacement.HeaderExpression.Match(header);
        //        Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
        //    }
        //}

        
    }
}
