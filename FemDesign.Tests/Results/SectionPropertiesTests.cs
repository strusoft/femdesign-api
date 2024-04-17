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
    public class SectionPropertiesTests
    {
        [TestMethod()]
        public void Parse()
        {
            //string modelPath = "Results\\Assets\\General.str";
            string modelPath = "Results\\Assets\\SectionTest.str";

            var (resultLines, headers, results) = UtilTestMethods.GetCsvParseData<SectionProperties>(modelPath);


            // Check parsed data
            Assert.IsTrue(results.First().GetType() == typeof(SectionProperties), $"{typeof(SectionProperties).Name} should be parsed");
            Assert.IsTrue(results.Last().GetType() == typeof(SectionProperties), $"{typeof(SectionProperties).Name} should be parsed");
            Assert.IsTrue(results.Count == resultLines.Sum(), "Should read all results.");

            foreach (var header in headers)
            {
                // Check header
                foreach (var line in header)
                {
                    var headerMatch = SectionProperties.HeaderExpression.Match(line);
                    Assert.IsTrue(headerMatch.Success, $"Should identify \"{line}\" as header");
                }

                // Check identification
                var identifier = header[0];
                var match = SectionProperties.IdentificationExpression.Match(identifier);
                Assert.IsTrue(match.Success, $"Should identify type of \"{identifier}\" as {typeof(SectionProperties).Name}");
            }
        }
    }

}