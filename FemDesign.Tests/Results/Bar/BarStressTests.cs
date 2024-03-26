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
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Bars, Stresses, Ultimate - Load case: Liveload - for selected objects
ID	x	Sigma x'(max)	Sigma x'(min)	Sigma vm
[-]	[m]	[N/mm2]	[N/mm2]	[N/mm2]
Analyse_example_beam.1.1	0.000	0.000	0.000	0.000
Analyse_example_beam.1.1	1.230	0.908	-0.908	0.908
Analyse_example_beam.1.1	1.230	0.908	-0.908	0.908
Analyse_example_beam.1.1	2.460	3.632	-3.632	3.632
Analyse_example_beam.1.1	2.460	7.452	-7.452	7.452
Analyse_example_beam.1.1	4.980	3.972	-3.972	3.972
Analyse_example_beam.1.1	4.980	3.972	-3.972	3.972
Analyse_example_beam.1.1	7.500	10.239	-10.239	10.239
Analyse_example_beam.1.1	7.500	10.239	-10.239	10.239
Analyse_example_beam.1.1	11.250	4.183	-4.183	4.183
Analyse_example_beam.1.1	11.250	4.183	-4.183	4.183
Analyse_example_beam.1.1	15.000	0.000	0.000	0.132

Bars, Stresses, Ultimate - Load case: Deadload - for selected objects
ID	x	Sigma x'(max)	Sigma x'(min)	Sigma vm
[-]	[m]	[N/mm2]	[N/mm2]	[N/mm2]
Analyse_example_beam.1.1	0.000	0.000	0.000	0.000
Analyse_example_beam.1.1	1.230	0.681	-0.681	0.681
Analyse_example_beam.1.1	1.230	0.681	-0.681	0.681
Analyse_example_beam.1.1	2.460	2.724	-2.724	2.724
Analyse_example_beam.1.1	2.460	0.480	-0.480	0.480
Analyse_example_beam.1.1	4.980	0.209	-0.209	0.209
Analyse_example_beam.1.1	4.980	0.209	-0.209	0.209
Analyse_example_beam.1.1	7.500	4.816	-4.816	4.816
Analyse_example_beam.1.1	7.500	4.816	-4.816	4.816
Analyse_example_beam.1.1	11.250	3.920	-3.920	3.920
Analyse_example_beam.1.1	11.250	3.920	-3.920	3.920
Analyse_example_beam.1.1	15.000	0.000	0.000	0.591");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(BarStress), "Bar Stress should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(BarStress), "Bar Stress should be parsed");

            File.Delete(path);
            }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                    "Bars, Stresses, Ultimate - Load case: Liveload - for selected objects",
                    "Bars, Stresses, Ultimate - Load comb.: Liveload - for selected objects",
                    "Bars, Stresses, Quasi Permanent - Load comb.: Liveload - for selected objects",
                    "Bars, Stresses, Quasi-Permanent - Load comb.: Liveload - for selected objects",
            };

            foreach (var header in headers)
            {
                var match = BarStress.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(BarStress).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                    "Bars, Stresses, Ultimate - Load case: Liveload - for selected objects",
                    "Bars, Stresses, Ultimate - Load comb.: Liveload - for selected objects",
                    "ID	x	Sigma x'(max)	Sigma x'(min)	Sigma vm",
                    "[-]	[m]	[N/mm2]	[N/mm2]	[N/mm2]"
            };

            foreach (var header in headers)
            {
                var match = BarStress.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }

}