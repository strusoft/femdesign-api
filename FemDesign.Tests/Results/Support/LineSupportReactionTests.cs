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
    public class LineSupportReactionTests
    {

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Line support group, Reactions, Ultimate - Load case: dl
ID	Elem	Node	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Fr	Mr	Case
[-]	[-]	[-]	[N/mm]	[N/mm]	[N/mm]	[Nmm/mm]	[Nmm/mm]	[Nmm/mm]	[N/mm]	[Nmm/mm]	[-]
S.6	276	572	-46.577	-85.270	-726.331	43240.721	9.585	-1131.244	732.801	43255.517	dl
S.6	276	665	-2.163	-34.037	-373.593	25773.945	28.756	5949.836	375.147	26451.798	dl
S.6	276	620	-23.152	-60.665	-548.451	34973.785	-4.793	3367.687	552.281	35135.551	dl
S.6	277	665	-2.163	-34.037	-373.593	25773.945	28.756	5949.836	375.147	26451.798	dl
S.6	277	755	8.596	0.221	-55.415	6898.951	162.953	3308.945	56.078	7653.182	dl
S.6	277	710	2.044	-15.523	-215.709	15998.297	-23.964	3771.990	216.276	16436.970	dl
S.6	278	755	8.596	0.221	-55.415	6898.951	162.953	3308.945	56.078	7653.182	dl
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 7);
            Assert.IsTrue(results.First().GetType() == typeof(LineSupportReaction));
            Assert.IsTrue(results.Last().GetType() == typeof(LineSupportReaction));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Line support group, Reactions, Ultimate - Load case: dl",
                "Line support group, Reactions, Load comb.: lc",
            };

            foreach (var header in headers)
            {
                var match = LineSupportReaction.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(LineSupportReaction).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Line support group, Reactions, Ultimate - Load case: dl",
                "Line support group, Reactions, Load comb.: lc",
                "ID	Elem	Node	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Fr	Mr	Comb.",
                "[-]	[-]	[-]	[N/mm]	[N/mm]	[N/mm]	[Nmm/mm]	[Nmm/mm]	[Nmm/mm]	[N/mm]	[Nmm/mm]	[-]",
                "ID	Elem	Node	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Fr	Mr	Case"
            };

            foreach (var header in headers)
            {
                var match = LineSupportReaction.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }

    }
}