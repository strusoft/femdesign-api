using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using FemDesign.Calculate;

namespace FemDesign.Results
{
    [TestClass()]
    public class BarDisplacementTests
    {
        [TestMethod]
        public void Parse()
        {
            string modelPath = "Results\\Assets\\Model.str";

            var (resultLines, headers, results) = UtilTestMethods.GetCsvParseData<BarDisplacement>(modelPath);


            // Check parsed data
            Assert.IsTrue(results.First().GetType() == typeof(BarDisplacement), $"{typeof(BarDisplacement).Name} should be parsed");
            Assert.IsTrue(results.Last().GetType() == typeof(BarDisplacement), $"{typeof(BarDisplacement).Name} should be parsed");
            Assert.IsTrue(results.Count == resultLines.Sum(), "Should read all results.");

            foreach (var header in headers)
            {
                // Check header
                foreach (var line in header)
                {
                    var headerMatch = BarDisplacement.HeaderExpression.Match(line);
                    Assert.IsTrue(headerMatch.Success, $"Should identify \"{line}\" as header");
                }

                // Check identification
                var identifier = header[0];
                var match = BarDisplacement.IdentificationExpression.Match(identifier);
                Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(BarDisplacement).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
                Assert.IsTrue(match.Groups["casecomb"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
                if(match.Groups["casecomb"].Value == "case")
                    Assert.IsTrue(match.Groups["loadcasetype"].Success);
            }
        }
        
    }
}
