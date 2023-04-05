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
    public class ShellUtilizationTests
    {
        const double DOUBLE_TOLERANCE = 1e6;

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shell, Utilization, Load comb.: uls
Shell	Max.	RBX	RBY	RTX	RTY	BU	CWB	CWT
[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]
P.1.1	157	50	50	157	145	0	0	0

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.All(r => r.GetType() == typeof(RCShellUtilization)), "Shell utilization should be parsed");
            Assert.IsTrue(results.Count == 1, "Should read all results.");

            var shellUtilization = results.Cast<RCShellUtilization>().ToArray();
            
            Assert.IsTrue(shellUtilization[0].Id == "P.1.1");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shell, Utilization, Load comb.: Lyft",
                "Max. of load combinations, Shell, Utilization",
            };

            foreach (var header in headers)
            {
                var match = RCShellUtilization.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(RCShellUtilization).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shell, Utilization, Load comb.: Lyft",
                "Max. of load combinations, Shell, Utilization",
                "Shell	Max.	Combination	RBX	RBY	RTX	RTY	BU	CWB	CWT",
                "[-]	[%]	[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]"
            };

            foreach (var header in headers)
            {
                var match = RCShellUtilization.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
