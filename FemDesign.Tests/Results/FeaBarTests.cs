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
    public class FeaBarTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Bar elements
Bar	Elem	Node 1	Node 2
B.1.1	1	6038	5857
B.1.1	2	5857	5642
B.1.1	3	5642	5405
B.1.1	4	5405	5123
B.2.1	5	5123	4686");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(FeaBar), "FeaBar should be parsed");
            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Bar elements",
            };

            foreach (var header in headers)
            {
                var match = FeaBar.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(FeaBar).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Bar elements",
                "Bar	Elem	Node 1	Node 2",
            };

            foreach (var header in headers)
            {
                var match = FeaBar.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
