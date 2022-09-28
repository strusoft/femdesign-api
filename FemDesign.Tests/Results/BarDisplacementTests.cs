using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    [TestClass()]
    public class BarDisplacementTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Bars, Displacements, Ultimate - Load case: IL
Bar	x	ex'	ey'	ez'	fix'	fiy'	fiz'
[-]	[m]	[mm]	[mm]	[mm]	[rad]	[rad]	[rad]
B.1.1	0.000	1.061	-0.130	-0.416	0.000	0.000	0.001
B.1.1	0.141	1.061	-0.019	-0.433	0.000	0.000	0.001
B.1.1	0.141	1.061	-0.019	-0.433	0.000	0.000	0.001
B.1.1	0.282	1.061	0.092	-0.434	0.000	0.000	0.001
B.1.1	0.282	1.061	0.092	-0.434	0.000	0.000	0.001
B.1.1	0.423	1.061	0.203	-0.433	0.000	0.000	0.001
B.1.1	0.423	1.061	0.203	-0.433	0.000	0.000	0.001
B.1.1	0.563	1.062	0.314	-0.448	0.000	0.000	0.001

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(BarDisplacement), "FeaNode should be parsed");
            Assert.IsTrue(results.Count == 8, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Bars, Displacements, Ultimate - Load case: IL",
                "Bars, Displacements, Quasi-Permanent - Load case: IL",
            };

            foreach (var header in headers)
            {
                var match = BarDisplacement.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(BarDisplacement).Name}");
                Assert.IsTrue(match.Groups["casename"].Success);
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
                Assert.IsTrue(match.Groups["loadcasetype"].Success);
                Assert.IsTrue(match.Groups["casecomb"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Bars, Displacements, Ultimate - Load case: IL",
                "Bar	x	ex'	ey'	ez'	fix'	fiy'	fiz'",
                "[-]	[m]	[mm]	[mm]	[mm]	[rad]	[rad]	[rad]"
            };

            foreach (var header in headers)
            {
                var match = BarDisplacement.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
