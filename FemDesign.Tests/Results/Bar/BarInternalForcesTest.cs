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
    public class BarInternalForcesTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Bars, Internal forces, Quasi-permanent - Load case: IL
ID	x	N	Ty'	Tz'	Mt	My'	Mz'
[-]	[m]	[kN]	[kN]	[kN]	[kNm]	[kNm]	[kNm]
B.1.1	0.000	-0.202	0.703	-6.572	-0.109	1.786	0.117
B.1.1	0.141	0.122	0.462	-5.952	-0.113	0.837	0.023
B.1.1	0.141	0.122	0.462	-5.952	-0.113	0.837	0.023
B.1.1	0.282	0.714	0.198	-5.332	-0.092	0.044	-0.000
B.1.1	0.282	0.714	0.198	-5.332	-0.092	0.044	-0.000
B.1.1	0.423	1.254	0.393	-6.129	-0.032	-0.806	-0.018
B.1.1	0.423	1.254	0.393	-6.129	-0.032	-0.806	-0.018
B.1.1	0.563	1.526	0.611	-6.928	0.003	-1.863	-0.101
B.2.1	0.000	-0.786	0.535	-7.064	-0.140	1.876	0.091

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(BarInternalForce), "Bar Internal Force should be parsed");
            Assert.IsTrue(results.Count == 9, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Bars, Internal forces, Quasi-permanent - Load case: IL",
            };

            foreach (var header in headers)
            {
                var match = BarInternalForce.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(BarInternalForce).Name}");
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
                "Bars, Internal forces, Quasi-permanent - Load case: IL",
                "ID	x	N	Ty'	Tz'	Mt	My'	Mz'",
                "[-]	[m]	[kN]	[kN]	[kN]	[kNm]	[kNm]	[kNm]"
            };

            foreach (var header in headers)
            {
                var match = BarInternalForce.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
