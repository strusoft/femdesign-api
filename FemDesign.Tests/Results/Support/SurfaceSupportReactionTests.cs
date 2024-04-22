using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace FemDesign.Results
{
    [TestClass()]
    public class SurfaceSupportReactionTests
    {
        [TestMethod]
        public void Parse()
        {
            string modelPath = "Results\\Assets\\General.str";

            var (resultLines, headers, results) = UtilTestMethods.GetCsvParseData<SurfaceSupportReaction>(modelPath);


            // Check parsed data
            Assert.IsTrue(results.First().GetType() == typeof(SurfaceSupportReaction), $"{typeof(SurfaceSupportReaction).Name} should be parsed");
            Assert.IsTrue(results.Last().GetType() == typeof(SurfaceSupportReaction), $"{typeof(SurfaceSupportReaction).Name} should be parsed");
            Assert.IsTrue(results.Count == resultLines.Sum(), "Should read all results.");

            foreach (var header in headers)
            {
                // Check header
                foreach (var line in header)
                {
                    var headerMatch = SurfaceSupportReaction.HeaderExpression.Match(line);
                    Assert.IsTrue(headerMatch.Success, $"Should identify \"{line}\" as header");
                }

                // Check identification
                var identifier = header[0];
                var match = SurfaceSupportReaction.IdentificationExpression.Match(identifier);
                Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(SurfaceSupportReaction).Name}");
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

    }
}