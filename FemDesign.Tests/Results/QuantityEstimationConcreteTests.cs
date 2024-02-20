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
