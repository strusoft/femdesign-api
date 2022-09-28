using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FemDesign.Results
{
    [TestClass()]
    public class NodalResponseFactorTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Footfall analysis, Nodal response factors, SE.1 - Overall maximum - for selected objects
ID	Node	x	y	z
[-]	[-]	[-]	[-]	[-]
P.1.1	1	0.000	0.000	0.000
	2	0.000	0.000	0.000
	3	0.000	0.000	0.000
	4	0.000	0.000	0.000
	5	0.000	0.000	0.000
	6	0.000	0.000	0.000
	7	0.000	0.000	0.000
	8	0.000	0.000	0.000
	9	0.000	0.000	0.000
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(NodalResponseFactor), "Nodal Acceleration should be parsed");
            Assert.IsTrue(results.Count == 9, "Should read all results.");

            File.Delete(path);
        }


        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Footfall analysis, Nodal response factors, SE.1 - Overall maximum - for selected objects"
            };

            foreach (var header in headers)
            {
                var match = NodalResponseFactor.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(NodalResponseFactor).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Footfall analysis, Nodal response factors, SE.1 - Overall maximum - for selected objects",
                "ID	Node	x	y	z",
                "[-]	[-]	[-]	[-]	[-]"
            };

            foreach (var header in headers)
            {
                var match = NodalResponseFactor.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}