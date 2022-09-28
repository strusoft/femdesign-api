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
    public class QuantityEstimationTimberPanelTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Quantity estimation, Timber panel
Storey	Struct.	Identifier	Quality	Thickness	Panel type	Length	Width	Area	Weight	Pcs	Total weight
	type			[mm]	[-]	[mm]	[mm]	[mm2]	[t]	[-]	[t]
-	Plate	TP.3	L(T)60-3S	60.0	A.1	2000.000	1500.000	3000000.000	0.072	1	0.072
-	Plate	TP.3	L(T)60-3S	60.0	A.2	2000.000	480.000	960000.000	0.023	1	0.023
-	Wall	TP.10	L(T)60-3S	60.0	A.3	3000.000	480.000	1440000.000	0.035	1	0.035
-	Wall	TP.10	L(T)60-3S	60.0	A.4	3000.000	1500.000	4500000.000	0.108	1	0.108
-	Plate	TP.13	L(T)60-3S	60.0	A.5	5000.000	1500.000	7500000.000	0.180	1	0.180
-	Plate	TP.13	L(T)60-3S	60.0	A.6	5000.000	480.000	2400000.000	0.058	1	0.058
TOTAL										6	0.475

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 6, "Should read all results");
            Assert.IsTrue(results.All(r => r.GetType() == typeof(QuantityEstimationTimberPanel)));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Quantity estimation, Timber panel",
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationTimberPanel.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(QuantityEstimationTimberPanel).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Quantity estimation, Timber panel",
                "Storey	Struct.	Identifier	Quality	Thickness	Panel type	Length	Width	Area	Weight	Pcs	Total weight",
                "	type			[mm]	[-]	[mm]	[mm]	[mm2]	[t]	[-]	[t]"
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationTimberPanel.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
