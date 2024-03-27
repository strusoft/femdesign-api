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
