using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FemDesign.Results.Tests
{
    [TestClass()]
    public class ShellDerivedForceTests
    {
        [TestMethod]
        public void Parse()
        {
            string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\313-shell-derived-force\shell-internal-forces.txt";

            var results = ResultsReader.Parse(file_path);

            Assert.IsTrue(results[0].GetType() == typeof(ShellDerivedForce), "Shell derived force should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(ShellDerivedForce), "Shell derived force (extract) should be parsed");
            
            Console.WriteLine(results);
        }


        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Shells, Derived internal forces, Ultimate - Load case: deadload - for selected objects",
                "Shells, Derived internal forces (Extract), Ultimate - Load case: deadload - for selected objects",
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
                "Shells, Derived internal forces, Ultimate - Load case: deadload - for selected objects",
                "Shells, Derived internal forces (Extract), Ultimate - Load case: deadload - for selected objects",
                "ID	Elem	Node	M1	M2	alpha(M)	N1	N2	alpha(N)",
                "ID	Max.	Elem	Node	M1	M2	alpha(M)	N1	N2	alpha(N)	Case",
                "[-]	[-]	[-]	[-]	[kNm/m]	[kNm/m]	[rad]	[kN/m]	[kN/m]	[rad]	[-]"
            };

            foreach (var header in headers)
            {
                var match = ShellDerivedForce.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}