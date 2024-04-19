using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using static System.Net.Mime.MediaTypeNames;

namespace FemDesign.Results
{
    [TestClass]
    public class NodalBucklingShapeTest
    {
        [TestMethod]
        public void Parse()
        {
            string modelPath = "Results\\Assets\\General.str";

            var (resultLines, headers, results) = UtilTestMethods.GetCsvParseData<NodalBucklingShape>(modelPath);


            // Check parsed data
            Assert.IsTrue(results.First().GetType() == typeof(NodalBucklingShape), $"{typeof(NodalBucklingShape).Name} should be parsed");
            Assert.IsTrue(results.Last().GetType() == typeof(NodalBucklingShape), $"{typeof(NodalBucklingShape).Name} should be parsed");
            Assert.IsTrue(results.Count == resultLines.Sum(), "Should read all results.");

            foreach (var header in headers)
            {
                // Check header
                foreach (var line in header)
                {
                    var headerMatch = NodalBucklingShape.HeaderExpression.Match(line);
                    Assert.IsTrue(headerMatch.Success, $"Should identify \"{line}\" as header");
                }

                // Check identification
                var identifier = header[0];
                var match = NodalBucklingShape.IdentificationExpression.Match(identifier);
                Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(NodalBucklingShape).Name}");
                Assert.IsTrue(match.Groups["casename"].Success);
                Assert.IsTrue(match.Groups["shape"].Success);
            }
        }

    }
}