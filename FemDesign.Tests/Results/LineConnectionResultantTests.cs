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
    public class LineConnectionResultantTests
    {

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Line connection, Resultants, Ultimate - Load case: Omapaino
ID	l/2	Fx'	Fy'	Fz'	Mx'	My'	Mz'
	[m]	[kN]	[kN]	[kN]	[kNm]	[kNm]	[kNm]
PP.1.CE.1	2.500	-0.020	0.001	-29.668	0.000	-3.966	0.002
PP.1.CE.2	1.250	0.094	2.144	-3.131	0.000	-0.041	1.857
PP.1.CE.3	2.500	2.125	-0.093	-29.701	0.000	3.862	1.006

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 3);
            Assert.IsTrue(results.First().GetType() == typeof(LineConnectionResultant));
            Assert.IsTrue(results.Last().GetType() == typeof(LineConnectionResultant));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Line connection, Resultants, Ultimate - Load case: Omapaino",
                "Line connection, Resultants, Load comb.: ULS EQU Y-",
            };

            foreach (var header in headers)
            {
                var match = LineConnectionResultant.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(LineConnectionResultant).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Line connection, Resultants, Ultimate - Load case: Omapaino",
                "Line connection, Resultants, Load comb.: ULS EQU Y-",
                "ID	l/2	Fx'	Fy'	Fz'	Mx'	My'	Mz'",
                "	[m]	[kN]	[kN]	[kN]	[kNm]	[kNm]	[kNm]",
            };

            foreach (var header in headers)
            {
                var match = LineConnectionResultant.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }

    }
}