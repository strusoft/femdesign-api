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
    public class ShellCrackWidthTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Shell, Crack width, Load comb.: Kvasi-frekvent
ID	Elem	Face	Width 1	Direction 1	Width 2	Direction 2
[-]	[-]	[-]	[mm]	[rad]	[mm]	[rad]
P.1.1	1	bottom	0.751	3.168	0.000	4.739
		top	0.000	0.000	0.000	0.000
	2	bottom	1.763	3.135	0.000	4.706
		top	0.000	0.000	0.000	0.000
P.2.1	1	bottom	1.222	3.126	0.000	4.697
		top	0.000	0.000	0.000	0.000
	2	bottom	0.917	3.142	0.000	4.712
		top	0.000	0.000	0.000	0.000

Max. of load combinations, Shell, Crack width
ID	Elem	Face	Width 1	Direction 1	Width 2	Direction 2	Comb
[-]	[-]	[-]	[mm]	[rad]	[mm]	[rad]	[-]
P.1.1	1	bottom	0.751	3.168	0.000	4.739	Kvasi-frekvent
		top	0.000	0.000	0.000	0.000	Kvasi-frekvent
	2	bottom	1.763	3.135	0.000	4.706	Kvasi-frekvent
		top	0.000	0.000	0.000	0.000	Kvasi-frekvent

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 12);
            Assert.IsTrue(results.First().GetType() == typeof(RCShellCrackWidth));
            Assert.IsTrue(results.Last().GetType() == typeof(RCShellCrackWidth));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shell, Crack width, Load comb.: Kvasi-frekvent",
                "Max. of load combinations, Shell, Crack width",
            };

            foreach (var header in headers)
            {
                var match = RCShellCrackWidth.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(RCShellCrackWidth).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Shell, Crack width, Load comb.: Kvasi-frekvent",
                "ID	Elem	Face	Width 1	Direction 1	Width 2	Direction 2",
                "[-]	[-]	[-]	[mm]	[rad]	[mm]	[rad]",
                "Max. of load combinations, Shell, Crack width",
                "ID	Elem	Face	Width 1	Direction 1	Width 2	Direction 2	Comb",
                "[-]	[-]	[-]	[mm]	[rad]	[mm]	[rad]	[-]",
            };

            foreach (var header in headers)
            {
                var match = RCShellCrackWidth.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}