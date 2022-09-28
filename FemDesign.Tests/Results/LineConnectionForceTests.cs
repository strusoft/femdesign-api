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
    public class LineConnectionForceTests
    {

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Line connection forces, Ultimate - Load case: Omapaino
No.	Elem	Node	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Fr	Mr
[-]	[-]	[-]	[kN/m]	[kN/m]	[kN/m]	[kNm/m]	[kNm/m]	[kNm/m]	[kN/m]	[kNm/m]
PP.1.CE.1	649	385	-0.013	0.031	-41.232	0.000	0.000	0.000	41.232	0.000
PP.1.CE.1	649	1648	-0.015	-0.009	-6.096	0.000	0.000	0.000	6.096	0.000
PP.1.CE.1	649	1382	-0.014	0.009	-19.386	0.000	0.000	0.000	19.386	0.000
PP.1.CE.1	650	1648	-0.015	-0.009	-6.096	0.000	0.000	0.000	6.096	0.000

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 4);
            Assert.IsTrue(results.First().GetType() == typeof(LineConnectionForce));
            Assert.IsTrue(results.Last().GetType() == typeof(LineConnectionForce));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Line connection forces, Ultimate - Load case: dl",
                "Line connection forces, Load comb.: ll",
            };

            foreach (var header in headers)
            {
                var match = LineConnectionForce.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(LineConnectionForce).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Line connection forces, Ultimate - Load case: dl",
                "Line connection forces, Load comb.: ll",
                "No.	Elem	Node	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Fr	Mr",
                "[-]	[-]	[-]	[kN/m]	[kN/m]	[kN/m]	[kNm/m]	[kNm/m]	[kNm/m]	[kN/m]	[kNm/m]",
            };

            foreach (var header in headers)
            {
                var match = LineConnectionForce.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }

    }
}