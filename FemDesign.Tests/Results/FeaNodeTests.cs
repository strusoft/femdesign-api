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
    public class FeaNodeTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Nodes
No.	x	y	z
[-]	[m]	[m]	[m]
1	-1.000	-0.000	9.000
2	-0.999	-0.054	4.000
3	-0.997	0.083	7.000
4	-0.996	0.088	2.000
5	-0.994	-0.107	1.000
6	-0.987	-0.158	6.000
7	-0.986	0.008	5.500
8	-0.985	0.175	5.000
9	-0.983	-0.079	7.500
10	-0.982	0.014	7.000
11	-0.982	0.015	4.000
12	-0.982	0.019	2.000
13	-0.982	-0.037	1.000
14	-0.980	0.068	9.000
15	-0.980	-0.068	9.000
16	-0.979	-0.091	2.500
17	-0.978	-0.088	6.000
18	-0.977	0.119	3.500
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(FeaNode), "FeaNode should be parsed");
            Assert.IsTrue(results.Count == 18, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Nodes",
            };

            foreach (var header in headers)
            {
                var match = FeaNode.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(FeaNode).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Nodes",
                "No.    x   y	z",
                "[-]	[m]	[m]	[m]"
            };

            foreach (var header in headers)
            {
                var match = FeaNode.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
