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
    public class CLTUtilizationTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"CLT panel, Utilization, Load comb.: ULS
Panel	Max.	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To
[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]
PP.1.1	91	26	-	16	-	21	39	91	15	91	91	-	-

Max. of load combinations, CLT panel, Utilization
Panel	Max.	Combination	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To
[-]	[%]	[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]
PP.1.1	91	ULS	26	-	16	-	21	39	91	15	91	91	-	-");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(CLTShellUtilization), "CLT Utilization should be parsed");
            Assert.IsTrue(results[1].GetType() == typeof(CLTShellUtilization), "CLT Utilization should be parsed");
            Assert.IsTrue(results.Count == 2, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Max. of load combinations, CLT panel, Utilization",
                "CLT panel, Utilization, Load comb.: ULS",
            };

            foreach (var header in headers)
            {
                var match = CLTShellUtilization.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(CLTShellUtilization).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "CLT panel, Utilization, Load comb.: ULS",
                "Panel	Max.	Combination	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To",
                "Panel	Max.	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To",
                "[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]",
                "[-]	[%]	[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]"
            };

            foreach (var header in headers)
            {
                var match = CLTShellUtilization.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
