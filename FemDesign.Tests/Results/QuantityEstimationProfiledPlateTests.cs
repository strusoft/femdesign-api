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
    public class QuantityEstimationProfiledPlateTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Quantity estimation, Profiled panel
Storey	Struct.	Identifier	Quality	Section	Thickness	Panel type	Length	Width	Area	Weight	Pcs	Total weight
	type				[mm]	[-]	[mm]	[mm]	[mm2]	[t]	[-]	[t]
-	Plate	PP.2	C40/50	HD-F 120-20	0.200	A.1	2000.000	1197.000	2394000.000	0.587	1	0.587
-	Plate	PP.2	C40/50	HD-F 120-20	0.200	A.2	2000.000	797.000	1594000.000	0.391	1	0.391
-	Plate	PP.6	S 355	HE-B 260	0.260	A.7	2000.000	260.000	520000.000	0.186	7	1.302
-	Plate	PP.6	S 355	HE-B 260	0.260	A.8	2000.000	156.000	312000.000	0.112	1	0.112
-	Plate	PP.7	D30	Sawn lumber 150x150	0.150	A.9	2000.000	150.000	300000.000	0.029	13	0.374
-	Plate	PP.7	D30	Sawn lumber 150x150	0.150	A.10	2000.000	8.000	16000.000	0.002	1	0.002
-	Wall	PP.9	C40/50	HD-F 120-20	0.200	A.3	3000.000	797.000	2391000.000	0.586	1	0.586
-	Wall	PP.9	C40/50	HD-F 120-20	0.200	A.4	3000.000	1197.000	3591000.000	0.881	1	0.881
-	Plate	PP.12	C40/50	HD-F 120-20	0.200	A.5	5000.000	1197.000	5985000.000	1.468	1	1.468
-	Plate	PP.12	C40/50	HD-F 120-20	0.200	A.6	5000.000	797.000	3985000.000	0.977	1	0.977
TOTAL											28	6.679

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 10, "Should read all results");
            Assert.IsTrue(results.All(r => r.GetType() == typeof(QuantityEstimationProfiledPlate)));

            var _results = results.Cast<QuantityEstimationProfiledPlate>().ToList();
            Assert.AreEqual(_results.First().Area, 2394000, 1e-6);
            
            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Quantity estimation, Profiled panel",
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationProfiledPlate.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(QuantityEstimationProfiledPlate).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Quantity estimation, Profiled panel",
                "Storey	Struct.	Identifier	Quality	Section	Thickness	Panel type	Length	Width	Area	Weight	Pcs	Total weight",
                "	type				[mm]	[-]	[mm]	[mm]	[mm2]	[t]	[-]	[t]"
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationProfiledPlate.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
