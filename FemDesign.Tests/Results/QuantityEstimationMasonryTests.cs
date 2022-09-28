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
    public class QuantityEstimationMasonryTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Quantity estimation, Masonry
Storey	Struct.	Identifier	Quality	Thickness	Unit weight	Subtotal	Total weight
	type			[mm]	[t/m, t/m2]	[m,m2]	[t]
-	Wall	W.1.1	Brick (sample)	200	0.160	49.281	7.885
-	Wall	W.2.1	Masonry (sample)	200	0.160	26.163	4.186
-	Wall	W.3.1	Masonry (sample)	200	0.160	10.765	1.722
-	Wall	W.4.1	Masonry (sample)	200	0.160	19.998	3.200
TOTAL							16.993

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 4, "Should read all results");
            Assert.IsTrue(results.All(r => r.GetType() == typeof(QuantityEstimationMasonry)));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Quantity estimation, Masonry",
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationMasonry.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(QuantityEstimationMasonry).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Quantity estimation, Masonry",
                "Storey	Struct.	Identifier	Quality	Thickness	Unit weight	Subtotal	Total weight",
                "	type			[mm]	[t/m, t/m2]	[m,m2]	[t]"
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationMasonry.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
