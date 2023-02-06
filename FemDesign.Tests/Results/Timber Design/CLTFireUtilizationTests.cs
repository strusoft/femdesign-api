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
    public class CLTFireUtilizationTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"CLT panel, Fire design, Utilization, Load comb.: fire
Panel	Duration	Fire protection	Max.	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To
[-]	[min.]	[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]
PP.1.1	40	Gypsum board, F, 2 layers, for other parts I.: 10 mm, O.: 10 mm, tch: 5 min, tf: 10 min	75	75	-	43	-	16	11	26	3	26	26	-	-

Max. of load combinations, CLT panel, Fire design, Utilization
Panel	Duration	Fire protection	Max.	Combination	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To
[-]	[min.]	[-]	[%]	[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]
PP.1.1	40	Gypsum board, F, 2 layers, for other parts I.: 10 mm, O.: 10 mm, tch: 5 min, tf: 10 min	75	fire	75	-	43	-	16	11	26	3	26	26	-	-

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(CLTFireUtilization), "CLT Utilization should be parsed");
            Assert.IsTrue(results[1].GetType() == typeof(CLTFireUtilization), "CLT Utilization should be parsed");
            Assert.IsTrue(results.Count == 2, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Max. of load combinations, CLT panel, Fire design, Utilization",
                "CLT panel, Fire design, Utilization, Load comb.: fire",
            };

            foreach (var header in headers)
            {
                var match = CLTFireUtilization.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(CLTFireUtilization).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Max. of load combinations, CLT panel, Fire design, Utilization",
                "CLT panel, Fire design, Utilization, Load comb.: fire",
                "Panel	Duration	Fire protection	Max.	Combination	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To",
                "Panel	Duration	Fire protection	Max.	Sx+	Sy+	Sx-	Sy-	Txy	Tx	Ty	SI	TS	CS	Bu	To",
                "[-]	[min.]	[-]	[%]	[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]",
                "[-]	[min.]	[-]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]	[%]"
            };

            foreach (var header in headers)
            {
                var match = CLTFireUtilization.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
