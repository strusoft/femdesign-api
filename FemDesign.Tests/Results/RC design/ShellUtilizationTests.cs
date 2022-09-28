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

            using (var stream = new StreamWriter(path)) stream.Write(@"Shell, Utilization, Load comb.: Lyft
Shell	Max.	RBX	RBY	RTX	RTY	BU	SC	CWB	CWT
[-]	[%]	[%]	[%]	[%]	[%]	[%]	[-]	[%]	[%]
P.1.1	1000	1000	1000	1000	1000	0	OK	0	0
P.2.1	78	78	31	71	66	0	OK	0	0
P.3.1	1000	1000	71	1000	71	0	OK	0	0

Max. of load combinations, Shell, Utilization
Shell	Max.	Combination	RBX	RBY	RTX	RTY	BU	SC	CWB	CWT
[-]	[%]	[-]	[%]	[%]	[%]	[%]	[%]	[-]	[%]	[%]
P.1.1	1000	Lyft	1000	1000	1000	1000	0	OK	0	0
P.2.1	78	Lyft	78	31	71	66	0	OK	0	0
P.3.1	1000	Lyft	1000	71	1000	71	0	OK	0	0

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.All(r => r.GetType() == typeof(RCShellUtilization)), "Shell utilization should be parsed");
            Assert.IsTrue(results.Count == 6, "Should read all results.");

            var shellUtilization = results.Cast<RCShellUtilization>().ToArray();
            
            Assert.IsTrue(shellUtilization[0].Id == "P.1.1");
            Assert.IsTrue(shellUtilization[1].Id == "P.2.1");
            Assert.IsTrue(shellUtilization[2].Id == "P.3.1");

            Assert.IsTrue(Math.Abs(shellUtilization[0].Max - 1000.0) < DOUBLE_TOLERANCE);
            Assert.IsTrue(Math.Abs(shellUtilization[1].Max - 78.0) < DOUBLE_TOLERANCE);
            Assert.IsTrue(Math.Abs(shellUtilization[2].Max - 1000.0) < DOUBLE_TOLERANCE);

            Assert.IsTrue(Math.Abs(shellUtilization[1].RBX - 78) < DOUBLE_TOLERANCE);
            Assert.IsTrue(Math.Abs(shellUtilization[1].RBY - 31) < DOUBLE_TOLERANCE);
            Assert.IsTrue(Math.Abs(shellUtilization[1].RTX - 71) < DOUBLE_TOLERANCE);
            Assert.IsTrue(Math.Abs(shellUtilization[1].RTY - 66) < DOUBLE_TOLERANCE);
            Assert.IsTrue(Math.Abs(shellUtilization[1].BU  -  0) < DOUBLE_TOLERANCE);
            Assert.IsTrue(         shellUtilization[1].SC == true);
            Assert.IsTrue(Math.Abs(shellUtilization[1].CWB -  0) < DOUBLE_TOLERANCE);
            Assert.IsTrue(Math.Abs(shellUtilization[1].CWT -  0) < DOUBLE_TOLERANCE);

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
                "Shell	Max.	Combination	RBX	RBY	RTX	RTY	BU	SC	CWB	CWT",
                "[-]	[%]	[-]	[%]	[%]	[%]	[%]	[%]	[-]	[%]	[%]"
            };

            foreach (var header in headers)
            {
                var match = RCShellUtilization.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
