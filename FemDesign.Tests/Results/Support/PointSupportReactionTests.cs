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
    public class PointSupportReactionTests
    {

        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Max. of load combinations, Point support group, Reactions, MinMax, Ultimate
Max.	ID	x	y	z	Node	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Fr	Mr	Comb
[-]	[-]	[m]	[m]	[m]	[-]	[kN]	[kN]	[kN]	[kNm]	[kNm]	[kNm]	[kN]	[kNm]	[-]
Fx' (+)	S.1	2.000	6.000	0.000	1	5.000	5.555	2.565	2.500	-5.346	6.664	7.902	8.902	ULS3
Fy' (+)	S.1	2.000	6.000	0.000	1	5.000	5.555	2.565	2.500	-5.346	6.664	7.902	8.902	ULS3
Fz' (+)	S.2	8.000	6.000	0.000	9	2.500	1.945	9.784	5.000	12.003	-3.336	10.284	13.424	ULS3
Mx' (+)	S.2	8.000	6.000	0.000	9	2.500	1.945	9.784	5.000	12.003	-3.336	10.284	13.424	ULS3
My' (+)	S.2	8.000	6.000	0.000	9	2.500	1.945	9.784	5.000	12.003	-3.336	10.284	13.424	ULS3
Mz' (+)	S.1	2.000	6.000	0.000	1	5.000	5.555	2.565	2.500	-5.346	6.664	7.902	8.902	ULS3
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 6);
            Assert.IsTrue(results.First().GetType() == typeof(PointSupportReactionMinMax));
            Assert.IsTrue(results.Last().GetType() == typeof(PointSupportReactionMinMax));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Max. of load combinations, Point support group, Reactions, MinMax, Ultimate",
                "Max. of load combinations, Point support group, Reactions, MinMax, Accidental",
            };

            foreach (var header in headers)
            {
                var match = PointSupportReactionMinMax.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(PointSupportReactionMinMax).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Max. of load combinations, Point support group, Reactions, MinMax, Ultimate",
                "Max. of load combinations, Point support group, Reactions, MinMax, Accidental",
                "Max.	ID	x	y	z	Node	Fx'	Fy'	Fz'	Mx'	My'	Mz'	Fr	Mr	Comb",
                "[-]	[-]	[m]	[m]	[m]	[-]	[kN]	[kN]	[kN]	[kNm]	[kNm]	[kNm]	[kN]	[kNm]	[-]"
            };

            foreach (var header in headers)
            {
                var match = PointSupportReactionMinMax.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }

        }

    }
}

