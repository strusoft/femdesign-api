using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results.Tests
{
    [TestClass()]
    public class ShellsDisplacementTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Displacements, Ultimate - Load case: LC1
Shell	Elem	Node	ex'	ey'	ez'	fix'	fiy'	fiz'
[-]	[-]	[-]	[mm]	[mm]	[mm]	[°]	[°]	[°]
P.1.1	1	564	0.000	0.000	-10.520	0.308	-0.049	0.000
P.1.1	2	55	0.000	0.000	-4.396	0.337	0.093	0.000

Shells, Displacements (Extract), Load comb.: LC1
Shell	Max.	Elem	Node	ex'	ey'	ez'	fix'	fiy'	fiz'	e	fi	Comb.
[-]	[-]	[-]	[-]	[mm]	[mm]	[mm]	[°]	[°]	[°]	[mm]	[°]	[-]
P.1.1	ex' (+)	91	172	-0.001	0.000	0.001	-0.000	0.007	0.000	0.001	0.007	LC1
P.1.1	ey' (+)	9	636	-0.001	0.000	0.031	0.000	-0.007	-0.000	0.031	0.007	LC1
P.1.1	ez' (+)	140	648	-0.001	0.000	0.081	0.001	-0.001	0.000	0.081	0.001	LC1
P.1.1	fix' (+)	140	648	-0.001	0.000	0.081	0.001	-0.001	0.000	0.081	0.001	LC1
P.1.1	fiy' (+)	98	632	-0.001	-0.000	0.001	0.000	0.007	-0.000	0.002	0.007	LC1
P.1.1	fiz' (+)	24	13	-0.001	-0.000	0.073	-0.001	-0.003	0.000	0.073	0.003	LC1
P.1.1	ex' (-)	18	282	-0.001	-0.000	0.077	-0.000	-0.001	-0.000	0.077	0.001	LC1
P.1.1	ey' (-)	136	662	-0.001	-0.000	0.031	0.000	0.007	-0.000	0.031	0.007	LC1
P.1.1	ez' (-)	30	351	-0.001	0.000	0.001	0.000	-0.007	0.000	0.001	0.007	LC1
P.1.1	fix' (-)	81	18	-0.001	0.000	0.081	-0.001	0.001	0.000	0.081	0.001	LC1
P.1.1	fiy' (-)	39	631	-0.001	0.000	0.001	0.000	-0.007	-0.000	0.002	0.007	LC1
P.1.1	fiz' (-)	101	575	-0.001	-0.000	0.071	0.001	0.003	-0.000	0.071	0.003	LC1
P.1.1	e	140	648	-0.001	0.000	0.081	0.001	-0.001	0.000	0.081	0.001	LC1
P.1.1	fi	39	631	-0.001	0.000	0.001	0.000	-0.007	-0.000	0.002	0.007	LC1

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(ShellsDisplacement), "Shell displacements should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(ShellsDisplacement), "Shell displacements (extract) should be parsed");
            Assert.IsTrue(results.Count == 16, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shells, Displacements, Ultimate - Load case: LC1",
                "Shells, Displacements (Extract), Load comb.: LC1",
            };

            foreach (var header in headers)
            {
                var match = ShellsDisplacement.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(ShellUtilization).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shells, Displacements, Ultimate - Load case: LC1",
                "Shells, Displacements (Extract), Load comb.: LC1",
                "Shell	Max.	Elem	Node	ex'	ey'	ez'	fix'	fiy'	fiz'	e	fi	Comb.",
                "[-] [-] [-] [-] [mm]    [mm]    [mm]    [°] [°] [°] [mm]    [°] [-]"
            };

            foreach (var header in headers)
            {
                var match = ShellsDisplacement.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
