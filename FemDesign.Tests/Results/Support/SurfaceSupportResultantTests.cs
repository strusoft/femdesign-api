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
    public class SurfaceSupportResultantTests
    {
        [TestMethod]
        [TestCategory("FEM-Design required")]
        public void Parse()
        {
            string modelPath = "Results\\Assets\\General.str";

            var (resultLines, headers, results) = UtilTestMethods.GetCsvParseData<SurfaceSupportResultant>(modelPath);


            // Check parsed data
            Assert.IsTrue(results.First().GetType() == typeof(SurfaceSupportResultant), $"{typeof(SurfaceSupportResultant).Name} should be parsed");
            Assert.IsTrue(results.Last().GetType() == typeof(SurfaceSupportResultant), $"{typeof(SurfaceSupportResultant).Name} should be parsed");
            Assert.IsTrue(results.Count == resultLines.Sum(), "Should read all results.");

            foreach (var header in headers)
            {
                // Check header
                foreach (var line in header)
                {
                    var headerMatch = SurfaceSupportResultant.HeaderExpression.Match(line);
                    Assert.IsTrue(headerMatch.Success, $"Should identify \"{line}\" as header");
                }

                // Check identification
                var identifier = header[0];
                var match = SurfaceSupportResultant.IdentificationExpression.Match(identifier);
                Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(SurfaceSupportResultant).Name}");
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

    }
}