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
    public class QuantityEstimationConcreteTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Quantity estimation, Concrete
Storey	Struct.	Identifier	Quality	Section/	Height	Width	Subtotal	Volume	Total weight	Formwork	Reinforcement
	type			Thickness[m]	[m]	[m]	[m,m2]	[m3]	[t]	[m2]	[kg/m, kg/m2]
-	Plate	P.1	C40/50	0.200			4.000	0.800	2.039	5.600	0.000
-	Plate	P.11	C40/50	0.200			10.000	2.000	5.097	22.800	0.000
-	Wall	W.8	C40/50	0.200	3.00	2.00	6.000	1.200	3.058	13.600	0.000
TOTAL								4.000	10.194	42.000	

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 3, "Should read all results");
            Assert.IsTrue(results.All(r => r.GetType() == typeof(QuantityEstimationConcrete)));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Quantity estimation, Concrete",
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationConcrete.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(QuantityEstimationConcrete).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Quantity estimation, Concrete",
                "Storey	Struct.	Identifier	Quality	Section/	Height	Width	Subtotal	Volume	Total weight	Formwork	Reinforcement",
                "	type			Thickness[mm]	[mm]	[mm]	[mm,mm2]	[mm3]	[t]	[mm2]	[kg/m, kg/m2]"
            };

            foreach (var header in headers)
            {
                var match = QuantityEstimationConcrete.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
