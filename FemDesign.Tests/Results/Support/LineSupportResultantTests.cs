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
    public class LineSupportResultantTests
    {

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Line support group, Resultants, Ultimate - Load case: Permanent plats
ID	l/2	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Case
	[mm]	[N]	[N]	[N]	[Nmm]	[Nmm]	[Nmm]	[-]
S.1	500.000	0.000	0.000	-6792.354	0.000	0.000	0.000	Permanent plats
S.2	500.000	0.000	0.000	-7015.836	0.000	0.000	0.000	Permanent plats
S.3	500.000	0.000	0.000	-7239.318	0.000	0.000	0.000	Permanent plats

Line support group, Resultants, Load comb.: 6.10a
ID	l/2	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Comb.
	[mm]	[N]	[N]	[N]	[Nmm]	[Nmm]	[Nmm]	[-]
S.1	500.000	0.000	0.000	-9169.677	0.000	0.000	0.000	6.10a
S.2	500.000	0.000	0.000	-9471.378	0.000	0.000	0.000	6.10a
S.3	500.000	0.000	0.000	-9773.079	0.000	0.000	0.000	6.10a

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 6);
            Assert.IsTrue(results.First().GetType() == typeof(LineSupportResultant));
            Assert.IsTrue(results.Last().GetType() == typeof(LineSupportResultant));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Line support group, Resultants, Ultimate - Load case: Permanent plats",
                "Line support group, Resultants, Load comb.: 6.10a",
            };

            foreach (var header in headers)
            {
                var match = LineSupportResultant.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(LineSupportResultant).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Line support group, Resultants, Ultimate - Load case: Permanent plats",
                "Line support group, Resultants, Load comb.: 6.10a",
                "ID	l/2	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Comb.",
                "	[mm]	[N]	[N]	[N]	[Nmm]	[Nmm]	[Nmm]	[-]",
            };

            foreach (var header in headers)
            {
                var match = LineSupportResultant.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }

    }
}