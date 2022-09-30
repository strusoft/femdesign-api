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
    public class ShellsStressTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shells, Stresses, bottom, Ultimate - Load case: DL
Shell	Elem	Node	Sigma x'	Sigma y'	Tau x'y'	Tau x'z'	Tau y'z'	Sigma vm	Sigma 1	Sigma 2	alpha	Case
[-]	[-]	[-]	[Pa]	[Pa]	[Pa]	[Pa]	[Pa]	[Pa]	[Pa]	[Pa]	[rad]	[-]
P.1.1	1	128	-377812.420	-274129.630	-298308.046	0.000	0.000	617480.988	-23191.863	-628750.186	-0.871	DL
P.1.1	1	176	-468783.277	-406686.530	-342492.094	0.000	0.000	739193.042	-93838.355	-781631.452	-0.831	DL
P.1.1	1	201	-649916.472	-443525.213	-455538.875	0.000	0.000	976421.371	-79639.474	-1013802.211	-0.897	DL
P.1.1	1	161	-611957.560	-337629.061	-435079.083	0.000	0.000	921820.051	-18604.929	-930981.692	-0.938	DL
P.1.1	1	-	-527117.432	-365492.608	-382854.525	0.000	0.000	811488.059	-55014.549	-837595.492	-0.889	DL
P.1.1	2	659	-4180120.640	-2127134.540	-1990187.428	0.000	0.000	4998896.771	-914312.074	-5392943.106	-1.023	DL
P.1.1	2	656	-3575457.150	-1725826.119	-1813964.210	0.000	0.000	4411706.104	-614530.171	-4686753.098	-1.021	DL
P.1.1	2	620	-3370447.772	-1804274.936	-1997640.161	0.000	0.000	4528334.179	-441716.800	-4733005.908	-0.972	DL
P.1.1	2	614	-3286627.676	-1839131.834	-1920644.988	0.000	0.000	4382512.670	-510396.328	-4615363.183	-0.966	DL
P.1.1	2	-	-3603163.310	-1874091.857	-1930609.197	0.000	0.000	4574287.127	-623285.084	-4853970.083	-0.996	DL
P.1.1	3	632	-1049147.327	-795773.885	-849383.454	0.000	0.000	1750268.237	-63681.369	-1781239.843	-0.859	DL
P.1.1	3	610	-819189.069	-530551.866	-608116.688	0.000	0.000	1275676.856	-49863.456	-1299877.479	-0.902	DL
P.1.1	3	643	-674541.927	-558910.915	-586217.521	0.000	0.000	1192196.428	-27664.782	-1205788.060	-0.835	DL
P.1.1	3	664	-806735.742	-837671.858	-808924.050	0.000	0.000	1624749.589	-13131.875	-1631275.725	-0.776	DL
P.1.1	3	-	-837403.516	-680727.131	-713160.428	0.000	0.000	1456153.897	-41615.208	-1476515.440	-0.840	DL");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(ShellStress), "Shell Stress should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(ShellStress), "Shell Stress (extract) should be parsed");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shells, Stresses, bottom, Ultimate - Load case: DL",
                "Shells, Stresses, bottom (Extract), Ultimate - Load case: deadload - for selected objects",
            };

            foreach (var header in headers)
            {
                var match = ShellStress.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(ShellStress).Name}");
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
                Assert.IsTrue(match.Groups["side"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shells, Stresses, top (Extract), Ultimate - Load case: deadload - for selected objects",
                "Shell	Max.	Elem	Node	Sigma x'	Sigma y'	Tau x'y'	Tau x'z'	Tau y'z'	Sigma vm	Sigma 1	Sigma 2	alpha	Case",
                "Shell	Elem	Node	Sigma x'	Sigma y'	Tau x'y'	Tau x'z'	Tau y'z'	Sigma vm	Sigma 1	Sigma 2	alpha",
                "[-]	[-]	[-]	[-]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[N/mm2]	[rad]	[-]"
            };

            foreach (var header in headers)
            {
                var match = ShellStress.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
                
            }
        }
    }
}