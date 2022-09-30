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
    public class EigenFrequenciesTest
    {

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Eigenfrequencies
Shape	Frequency	Period	Modal mass	mx'	my'	mz'
[-]	[Hz]	[s]	[t]	[%]	[%]	[%]
1	1.491	0.671	10.260	0.000	0.000	81.302
2	6.041	0.166	4.163	0.000	0.000	0.104
3	19.250	0.052	4.102	0.000	0.000	2.653
4	21.010	0.048	3.524	0.000	0.000	8.512
5	25.865	0.039	8.156	0.424	68.956	0.000
6	32.680	0.031	1.594	0.000	0.000	0.000

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 6);
            Assert.IsTrue(results.First().GetType() == typeof(EigenFrequencies));
            Assert.IsTrue(results.Last().GetType() == typeof(EigenFrequencies));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Eigenfrequencies"
            };

            foreach (var header in headers)
            {
                var match = EigenFrequencies.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(EigenFrequencies).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Eigenfrequencies",
                "Shape	Frequency	Period	Modal	mass	mx'	my'	mz'",
                "[-]	[Hz]	[s]	[t]	[%]	[%]	[%]"
            };

            foreach (var header in headers)
            {
                var match = EigenFrequencies.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}