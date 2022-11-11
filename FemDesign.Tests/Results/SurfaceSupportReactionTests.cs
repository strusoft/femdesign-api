using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class SurfaceSupportReactionTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Surface support, Reactions, Ultimate - Load case: Myload
No.	Elem	Node	Fx'	Fy'	Fz'	Fr	Case
[-]	[-]	[-]	[kN/m2]	[kN/m2]	[kN/m2]	[kN/m2]	[-]
F.1.3	70	186	0.000	0.000	0.057	0.057	Myload
F.1.3	70	150	0.000	0.000	0.061	0.061	Myload
F.1.3	70	137	0.000	0.000	0.066	0.066	Myload
F.1.3	70	168	0.000	0.000	0.060	0.060	Myload
F.1.3	70	-	0.000	0.000	0.061	0.061	Myload
F.1.3	71	148	0.000	0.000	0.057	0.057	Myload
F.1.3	71	168	0.000	0.000	0.060	0.060	Myload
F.1.3	71	137	0.000	0.000	0.066	0.066	Myload
F.1.3	71	121	0.000	0.000	0.061	0.061	Myload
F.1.3	71	-	0.000	0.000	0.061	0.061	Myload

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(SurfaceSupportReaction), "SurfaceSupport should be parsed");
            Assert.IsTrue(results.Count == 10, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Surface support, Reactions, Ultimate - Load case: Myload",
                "Surface support, Reactions, Load comb.: Combo1",
            };

            foreach (var header in headers)
            {
                var match = SurfaceSupportReaction.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(SurfaceSupportReaction).Name}");
                Assert.IsTrue(match.Groups["casename"].Success);
                Assert.IsTrue(match.Groups["type"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Surface support, Reactions, Ultimate - Load case: Myload",
                "No.	Elem	Node	Fx'	Fy'	Fz'	Fr	Case",
                "[-]	[-]	[-]	[kN/m2]	[kN/m2]	[kN/m2]	[kN/m2]	[-]"
            };

            foreach (var header in headers)
            {
                var match = SurfaceSupportReaction.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}