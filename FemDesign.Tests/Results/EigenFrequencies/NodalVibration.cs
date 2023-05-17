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
    public class NodalVibrationTests
    {

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Nodal vibration shapes, 1
ID	Node	ex	ey	ez	fix	fiy	fiz
[-]	[-]	[-]	[-]	[-]	[-]	[-]	[-]
B.1.1	5405	-0.045	0.034	-0.021	-0.014	0.002	0.013
B.1.1	5123	-0.047	0.033	-0.023	-0.021	-0.004	0.013
B.1.1	5642	-0.044	0.035	-0.020	-0.012	0.004	0.013
B.1.1	5857	-0.042	0.036	-0.019	-0.014	0.001	0.013
B.1.1	6038	-0.041	0.037	-0.018	-0.020	-0.008	0.014
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 5);
            Assert.IsTrue(results.First().GetType() == typeof(NodalVibration));
            Assert.IsTrue(results.Last().GetType() == typeof(NodalVibration));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Nodal vibration shapes, 42"
            };

            foreach (var header in headers)
            {
                var match = NodalVibration.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(NodalVibration).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Nodal vibration shapes, 42",
                "ID	Node	ex	ey	ez	fix	fiy	fiz",
                "[-]	[-]	[-]	[-]	[-]	[-]	[-]	[-]"
            };

            foreach (var header in headers)
            {
                var match = NodalVibration.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}