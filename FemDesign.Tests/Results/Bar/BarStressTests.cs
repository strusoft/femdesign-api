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
    public class BarStressTests
    {
        [TestMethod()]
        public void Parse()
        {
            string modelPath = "Results\\Assets\\General.str";

            var (resultLines, headers, results) = UtilTestMethods.GetCsvParseData<BarStress>(modelPath);


            // Check parsed data
            Assert.IsTrue(results.First().GetType() == typeof(BarStress), $"{typeof(BarStress).Name} should be parsed");
            Assert.IsTrue(results.Last().GetType() == typeof(BarStress), $"{typeof(BarStress).Name} should be parsed");
            Assert.IsTrue(results.Count == resultLines.Sum(), "Should read all results.");

            foreach (var header in headers)
            {
                // Check header
                foreach (var line in header)
                {
                    var headerMatch = BarStress.HeaderExpression.Match(line);
                    Assert.IsTrue(headerMatch.Success, $"Should identify \"{line}\" as header");
                }

                // Check identification
                var identifier = header[0];
                var match = BarStress.IdentificationExpression.Match(identifier);
                Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(BarStress).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
                Assert.IsTrue(match.Groups["casecomb"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
                if (match.Groups["casecomb"].Value == "case")
                    Assert.IsTrue(match.Groups["loadcasetype"].Success);
            }
        }
    }

}