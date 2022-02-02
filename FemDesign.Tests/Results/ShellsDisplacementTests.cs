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
        [TestMethod("Parse combined")]
        public void ParseCombinedTest()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Displacements, Ultimate - Load case: LC1
Shell	Elem	Node	ex'	ey'	ez'	fix'	fiy'	fiz'
[-]	[-]	[-]	[mm]	[mm]	[mm]	[°]	[°]	[°]
P.1.1	1	564	0.000	0.000	-10.520	0.308	-0.049	0.000
P.1.1	2	55	0.000	0.000	-4.396	0.337	0.093	0.000
P.1.1	3	458	0.000	0.000	-5.955	0.311	-0.064	0.000
P.1.1	4	105	0.000	0.000	-5.243	0.323	0.085	0.000
P.1.1	5	452	0.000	0.000	-13.715	0.231	-0.028	0.000
P.1.1	6	594	0.000	0.000	-13.131	0.258	-0.029	0.000
P.1.1	7	46	0.000	0.000	-10.520	0.308	0.049	0.000
P.1.1	8	176	0.000	0.000	-16.870	0.109	0.013	0.000
P.1.1	9	592	0.000	0.000	-16.661	0.120	-0.005	0.000

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
            Assert.IsTrue(results[0].GetType() == typeof(ShellsDisplacement), "Shell displacements (extract) should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(ShellsDisplacement), "Shell displacements should be parsed");
            Assert.IsTrue(results.Count == 23, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod("Parse")]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Displacements, Ultimate - Load case: LC1
Shell	Elem	Node	ex'	ey'	ez'	fix'	fiy'	fiz'
[-]	[-]	[-]	[mm]	[mm]	[mm]	[°]	[°]	[°]
P.1.1	1	564	0.000	0.000	-10.520	0.308	-0.049	0.000
P.1.1	2	55	0.000	0.000	-4.396	0.337	0.093	0.000
P.1.1	3	458	0.000	0.000	-5.955	0.311	-0.064	0.000
P.1.1	4	105	0.000	0.000	-5.243	0.323	0.085	0.000
P.1.1	5	452	0.000	0.000	-13.715	0.231	-0.028	0.000
P.1.1	6	594	0.000	0.000	-13.131	0.258	-0.029	0.000
P.1.1	7	46	0.000	0.000	-10.520	0.308	0.049	0.000
P.1.1	8	176	0.000	0.000	-16.870	0.109	0.013	0.000
P.1.1	9	592	0.000	0.000	-16.661	0.120	-0.005	0.000
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.All(r => r.GetType() == typeof(ShellsDisplacement)), "Shell displacements should be parsed");
            Assert.IsTrue(results.Count == 9, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod("Parse extract")]
        public void ParseExtractTest()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Displacements (Extract), Load comb.: LC1
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
            Assert.IsTrue(results.All(r => r.GetType() == typeof(ShellsDisplacement)), "Shell displacements (extract) should be parsed");
            Assert.IsTrue(results.Count == 14, "Should read all results.");

            File.Delete(path);
        }
    }
}
