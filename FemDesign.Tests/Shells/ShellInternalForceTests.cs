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
    public class ShellInternalForceTests
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

Shells, Internal forces, Load comb.: lc
ID	Elem	Node	Mx'	My'	Mx'y'	Nx'	Ny'	Nx'y'	Tx'z'	Ty'z'	Comb.
[-]	[-]	[-]	[Nmm/mm]	[Nmm/mm]	[Nmm/mm]	[N/mm]	[N/mm]	[N/mm]	[N/mm]	[N/mm]	[-]
P.1.1	25	338	37193.091	12427.689	42016.373	-1.173	-0.290	0.783	75.687	-17.828	lc
P.1.1	25	404	29445.055	7583.917	37369.134	-2.086	-0.686	1.776	80.635	-14.914	lc
P.1.1	25	-	28267.081	19271.497	31212.947	-0.077	-2.064	1.893	32.338	-20.607	lc
P.1.1	25	310	33538.213	27206.502	35220.910	-0.402	-1.005	1.079	22.073	-20.143	lc
P.1.1	25	348	32207.382	16628.731	36099.203	-0.925	-1.027	1.390	43.483	-18.258	lc
P.1.1	26	448	16607.270	10398.495	29705.477	1.556	-4.430	2.635	45.835	-25.649	lc
P.1.1	26	536	-1621.296	1021.508	32683.787	4.579	-7.105	1.937	54.026	-40.257	lc
P.1.1	26	531	8588.596	13245.039	26754.187	4.951	-5.229	-1.076	39.052	-34.559	lc");

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
                "Shells, Internal forces, Load comb.: lc"
            };

            foreach (var header in headers)
            {
                var match = ShellInternalForce.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(ShellInternalForce).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shells, Internal forces, Ultimate - Load case: DL - for selected objects",
                "Shells, Internal forces, Load comb.: lc",
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