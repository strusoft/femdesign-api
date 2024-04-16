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
            string modelPath = "Results\\Assets\\General.str";

            var (resultLines, headers, results) = UtilTestMethods.GetCsvParseData<QuantityEstimationConcrete>(modelPath);


            // Check parsed data
            Assert.IsTrue(results.First().GetType() == typeof(QuantityEstimationConcrete), $"{typeof(QuantityEstimationConcrete).Name} should be parsed");
            Assert.IsTrue(results.Last().GetType() == typeof(QuantityEstimationConcrete), $"{typeof(QuantityEstimationConcrete).Name} should be parsed");
            Assert.IsTrue(results.Count == resultLines.Sum(), "Should read all results.");

            foreach (var header in headers)
            {
                // Check header
                foreach (var line in header)
                {
                    var headerMatch = QuantityEstimationConcrete.HeaderExpression.Match(line);
                    Assert.IsTrue(headerMatch.Success, $"Should identify \"{line}\" as header");
                }

                // Check identification
                var identifier = header[0];
                var match = QuantityEstimationConcrete.IdentificationExpression.Match(identifier);
                Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(QuantityEstimationConcrete).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
            }
        }

    }
}
