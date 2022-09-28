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
    public class NodalAccelerationTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Footfall analysis, Nodal accelerations, SE.1 - Overall maximum - for selected objects
ID	Node	ax	ay	az
[-]	[-]	[m/s2]	[m/s2]	[m/s2]
P.1.1	1	0.000	0.000	0.000
P.1.1	2	0.000	0.000	0.000
P.1.1	3	0.000	0.000	0.000
P.1.1	4	0.000	0.000	0.000
P.1.1	5	0.000	0.000	0.000
P.1.1	6	0.000	0.000	0.000
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(NodalAcceleration), "Nodal Acceleration should be parsed");
            Assert.IsTrue(results.Count == 6, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Footfall analysis, Nodal accelerations, SE.1",
            };

            foreach (var header in headers)
            {
                var match = NodalAcceleration.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(NodalAcceleration).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Footfall analysis, Nodal accelerations, SE.1",
                "ID	Node	ax	ay	az",
                "[-]	[-]	[m/s2]	[m/s2]	[m/s2]"
            };

            foreach (var header in headers)
            {
                var match = NodalAcceleration.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
