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
    public class ShellDerivedForceTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Derived internal forces, Ultimate - Load case: IL
ID	Elem	Node	M1	M2	alpha(M)	N1	N2	alpha(N)	Case
[-]	[-]	[-]	[Nmm/mm]	[Nmm/mm]	[rad]	[N/mm]	[N/mm]	[rad]	[-]
P.1.1	595	5773	256.256	92.485	-1.314	6.455	2.446	0.303	IL
P.1.1	595	5893	556.806	28.445	-0.187	8.037	0.632	0.180	IL
P.1.1	595	6074	745.102	-5.252	-0.161	12.700	1.269	0.225	IL
P.1.1	595	6054	-195.868	-1669.048	-1.224	13.862	3.748	-0.028	IL
P.1.1	595	-	157.880	-205.649	-0.810	10.144	2.144	0.147	IL
P.1.1	596	5431	618.335	77.168	-0.543	4.939	-1.074	0.259	IL
P.1.1	596	5767	2409.225	381.773	-0.401	6.812	-0.876	0.164	IL
P.1.1	596	5914	582.131	-312.793	-1.077	4.783	-0.964	0.172	IL
P.1.1	596	5696	501.837	-325.731	-1.150	5.374	-1.005	0.180	IL
P.1.1	596	-	911.020	71.967	-0.693	5.468	-0.971	0.192	IL
P.1.1	597	617	-123.324	-428.055	0.222	-1.549	-4.038	0.429	IL
P.1.1	597	606	136.395	-746.185	0.394	-0.666	-1.641	-1.365	IL
P.1.1	597	999	89.459	-327.459	0.348	-1.031	-1.676	1.332	IL
P.1.1	597	1054	103.865	-208.631	0.183	-1.205	-2.397	0.715	IL
P.1.1	597	-	48.120	-424.103	0.323	-1.402	-2.149	0.730	IL
P.1.1	598	481	155.363	-814.208	0.132	1.629	-3.533	1.407	IL
P.1.1	598	544	-297.169	-456.007	0.515	0.974	-5.212	0.913	IL

Shells, Derived internal forces (Extract), Ultimate - Load case: IL
ID	Max.	Elem	Node	M1	M2	alpha(M)	N1	N2	alpha(N)	Case
[-]	[-]	[-]	[-]	[Nmm/mm]	[Nmm/mm]	[rad]	[N/mm]	[N/mm]	[rad]	[-]
P.1.1	M1 (+)	701	6105	2731.907	57.585	0.427	12.556	-0.179	0.423	IL
P.1.1	M2 (+)	603	4227	523.044	458.486	1.245	2.642	-0.398	0.328	IL
P.1.1	N1 (+)	595	6054	-195.868	-1669.048	-1.224	13.862	3.748	-0.028	IL
P.1.1	N2 (+)	595	6054	-195.868	-1669.048	-1.224	13.862	3.748	-0.028	IL
P.1.1	M1 (-)	624	357	-1198.228	-3134.021	0.860	-3.257	-18.207	0.382	IL
P.1.1	M2 (-)	624	357	-1198.228	-3134.021	0.860	-3.257	-18.207	0.382	IL
P.1.1	N1 (-)	626	150	943.685	-3093.058	0.308	-5.624	-7.436	-1.430	IL
P.1.1	N2 (-)	624	357	-1198.228	-3134.021	0.860	-3.257	-18.207	0.382	IL
P.2.1	M1 (+)	875	1406	1802.832	-96.426	-1.350	2.037	-0.076	-0.264	IL
P.2.1	M2 (+)	775	3146	510.981	488.475	0.663	0.441	0.111	-1.558	IL
P.2.1	N1 (+)	793	4	1048.886	-1374.331	0.390	3.085	0.365	-1.301	IL
P.2.1	N2 (+)	909	1730	1594.594	-487.776	1.201	1.160	1.018	0.608	IL
P.2.1	M1 (-)	820	4380	-511.410	-1655.674	-0.688	0.219	-2.038	1.343	IL
P.2.1	M2 (-)	905	689	-217.661	-1755.619	0.172	0.085	-0.459	-1.539	IL
P.2.1	N1 (-)	762	5746	-488.269	-1669.396	-1.287	-0.436	-2.292	0.969	IL
P.2.1	N2 (-)	762	5746	-488.269	-1669.396	-1.287	-0.436	-2.292	0.969	IL

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(ShellDerivedForce), "Shell Derived Force force should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(ShellDerivedForce), "Shell Derived Force (extract) should be parsed");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shells, Derived internal forces, Quasi-permanent - Load case: IL",
                "Shells, Derived internal forces (Extract), Quasi-permanent - Load case: IL",
                "Shells, Derived internal forces (Extract), Quasi-permanent - Load comb.: IL",
            };

            foreach (var header in headers)
            {
                var match = ShellDerivedForce.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(ShellDerivedForce).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shells, Derived internal forces, Quasi-permanent - Load case: IL",
                "ID	Elem	Node	M1	M2	alpha(M)	N1	N2	alpha(N)",
                "[-]	[-]	[-]	[kNm/m]	[kNm/m]	[rad]	[kN/m]	[kN/m]	[rad]",
                "Shells, Derived internal forces (Extract), Quasi-permanent - Load case: IL",
            };

            foreach (var header in headers)
            {
                var match = ShellDerivedForce.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");

            }
        }
    }
}