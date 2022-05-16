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
    public class ShellForceTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Internal forces, Ultimate - Load case: DL
ID	Elem	Node	Mx'	My'	Mx'y'	Nx'	Ny'	Nx'y'	Tx'z'	Ty'z'	Case
[-]	[-]	[-]	[Nmm/mm]	[Nmm/mm]	[Nmm/mm]	[N/mm]	[N/mm]	[N/mm]	[N/mm]	[N/mm]	[-]
P.1.1	1	128	-629.687	-456.883	-497.180	0.000	0.000	0.000	2.233	0.937	DL
P.1.1	1	176	-781.305	-677.811	-570.820	0.000	0.000	0.000	3.035	2.527	DL
P.1.1	1	201	-1083.194	-739.209	-759.231	0.000	0.000	0.000	4.107	0.876	DL
P.1.1	1	161	-1019.929	-562.715	-725.132	0.000	0.000	0.000	3.304	0.611	DL
P.1.1	1	-	-878.529	-609.154	-638.091	0.000	0.000	0.000	3.170	1.238	DL
P.1.1	2	659	-6966.868	-3545.224	-3316.979	0.000	0.000	0.000	16.566	-10.198	DL
P.1.1	2	656	-5959.095	-2876.377	-3023.274	0.000	0.000	0.000	4.302	-6.639	DL
P.1.1	2	620	-5617.413	-3007.125	-3329.400	0.000	0.000	0.000	6.773	-3.402	DL
P.1.1	2	614	-5477.713	-3065.220	-3201.075	0.000	0.000	0.000	13.549	-0.150	DL
P.1.1	2	-	-6005.272	-3123.486	-3217.682	0.000	0.000	0.000	10.298	-5.097	DL
P.1.1	3	632	-1748.579	-1326.290	-1415.639	0.000	0.000	0.000	2.434	7.569	DL
P.1.1	3	610	-1365.315	-884.253	-1013.528	0.000	0.000	0.000	2.474	5.153	DL
P.1.1	3	643	-1124.237	-931.518	-977.029	0.000	0.000	0.000	-3.394	-2.087	DL
P.1.1	3	664	-1344.560	-1396.120	-1348.207	0.000	0.000	0.000	-7.024	-6.525	DL
P.1.1	3	-	-1395.673	-1134.545	-1188.601	0.000	0.000	0.000	-1.377	1.028	DL
P.1.1	4	29	-45.168	263.682	-149.903	0.000	0.000	0.000	0.149	0.471	DL
P.1.1	4	1	-21.308	243.561	-128.030	0.000	0.000	0.000	0.228	3.880	DL
P.1.1	4	3	-3.482	311.694	-146.756	0.000	0.000	0.000	-0.213	3.198	DL
P.1.1	4	26	7.455	359.868	-177.807	0.000	0.000	0.000	-0.085	0.442	DL
P.1.1	4	-	-15.626	294.701	-150.624	0.000	0.000	0.000	0.020	1.998	DL
P.1.1	5	628	-5031.709	-3015.870	-2536.137	0.000	0.000	0.000	19.646	5.385	DL
P.1.1	5	636	-6036.244	-3820.865	-3220.296	0.000	0.000	0.000	20.052	-3.041	DL
P.1.1	5	597	-4281.033	-2890.708	-2664.453	0.000	0.000	0.000	13.193	5.103	DL
P.1.1	5	582	-3323.910	-2413.552	-1991.481	0.000	0.000	0.000	9.778	6.700	DL
P.1.1	5	-	-4668.224	-3035.249	-2603.092	0.000	0.000	0.000	15.667	3.537	DL
P.1.1	6	97	-308.636	-118.142	-246.443	0.000	0.000	0.000	1.344	1.829	DL
P.1.1	6	147	-360.955	-119.041	-275.081	0.000	0.000	0.000	1.162	2.337	DL
P.1.1	6	163	-534.911	-408.971	-381.399	0.000	0.000	0.000	2.095	2.351	DL
P.1.1	6	118	-447.365	-312.673	-348.227	0.000	0.000	0.000	1.965	1.405	DL
P.1.1	6	-	-412.967	-239.707	-312.787	0.000	0.000	0.000	1.641	1.981	DL
P.1.1	7	599	-2129.399	-1307.051	-1325.269	0.000	0.000	0.000	4.490	7.245	DL
P.1.1	7	550	-2017.732	-1063.485	-986.851	0.000	0.000	0.000	6.071	5.336	DL
P.1.1	7	533	-1484.821	-704.861	-723.136	0.000	0.000	0.000	4.397	4.253	DL");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(ShellInternalForce), "Shell internal force should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(ShellInternalForce), "Shell internal force (extract) should be parsed");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shells, Internal forces, Ultimate - Load case: DL",
                "Shells, Internal forces (Extract), Ultimate - Load case: DL",
            };

            foreach (var header in headers)
            {
                var match = ShellInternalForce.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(ShellInternalForce).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["LoadCaseType"].Success);
                Assert.IsTrue(match.Groups["casecomb"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shells, Internal forces, Ultimate - Load case: DL - for selected objects",
                "ID	Max.	Elem	Node	Mx'	My'	Mx'y'	Nx'	Ny'	Nx'y'	Tx'z'	Ty'z'	Case",
                "ID	Elem	Node	Mx'	My'	Mx'y'	Nx'	Ny'	Nx'y'	Tx'z'	Ty'z'",
                "[-]	[-]	[-]	[-]	[kNm/m]	[kNm/m]	[kNm/m]	[kN/m]	[kN/m]	[kN/m]	[kN/m]	[kN/m]	[-]"
            };

            foreach (var header in headers)
            {
                var match = ShellInternalForce.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");

            }
        }
    }
}