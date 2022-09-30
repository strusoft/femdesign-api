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
    public class ShellsDisplacementTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Displacements, Ultimate - Load case: DL
Shell	Elem	Node	ex'	ey'	ez'	fix'	fiy'	fiz'	Case
[-]	[-]	[-]	[mm]	[mm]	[mm]	[rad]	[rad]	[rad]	[-]
P.1.1	1859	771	0.000	0.000	-1.180	0.001	-0.001	0.000	DL
P.1.1	1859	828	0.000	0.000	-1.081	0.001	-0.001	0.000	DL
P.1.1	1859	858	0.000	0.000	-0.861	0.001	-0.001	0.000	DL
P.1.1	1859	811	0.000	0.000	-0.919	0.001	-0.001	0.000	DL
P.1.1	1859	-	0.000	0.000	-1.010	0.001	-0.001	0.000	DL
P.1.1	1860	1406	0.000	0.000	-0.045	0.000	-0.000	0.000	DL
P.1.1	1860	1402	0.000	0.000	-0.020	0.000	-0.000	0.000	DL
P.1.1	1860	1355	0.000	0.000	-0.069	0.000	-0.001	0.000	DL
P.1.1	1860	1345	0.000	0.000	-0.132	0.000	-0.001	0.000	DL
P.1.1	1860	-	0.000	0.000	-0.067	0.000	-0.001	0.000	DL
P.1.1	1861	1373	0.000	0.000	-0.511	0.001	-0.001	0.000	DL
P.1.1	1861	1339	0.000	0.000	-0.701	0.001	-0.001	0.000	DL
P.1.1	1861	1387	0.000	0.000	-0.622	0.001	-0.001	0.000	DL
P.1.1	1861	1414	0.000	0.000	-0.416	0.001	-0.001	0.000	DL
P.1.1	1861	-	0.000	0.000	-0.562	0.001	-0.001	0.000	DL
P.1.1	1862	639	0.000	0.000	-2.339	0.001	-0.001	0.000	DL
P.1.1	1862	588	0.000	0.000	-2.536	0.001	-0.001	0.000	DL
P.1.1	1862	590	0.000	0.000	-2.709	0.001	-0.002	0.000	DL
P.1.1	1862	632	0.000	0.000	-2.518	0.001	-0.002	0.000	DL
P.1.1	1862	-	0.000	0.000	-2.526	0.001	-0.002	0.000	DL
P.1.1	1863	1368	0.000	0.000	-0.252	0.001	-0.001	0.000	DL
P.1.1	1863	1377	0.000	0.000	-0.145	0.000	-0.001	0.000	DL
P.1.1	1863	1323	0.000	0.000	-0.247	0.001	-0.001	0.000	DL
P.1.1	1863	1306	0.000	0.000	-0.377	0.001	-0.001	0.000	DL

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
            Assert.IsTrue(results[0].GetType() == typeof(ShellDisplacement), "Shell displacements should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(ShellDisplacement), "Shell displacements (extract) should be parsed");

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
                var match = ShellDisplacement.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(ShellDisplacement).Name}");
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
                var match = ShellDisplacement.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}
