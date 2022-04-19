using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FemDesign.Results.Tests
{
    [TestClass()]
    public class NodalAccelerationTests
    {
        [TestMethod]
        public void Parse()
        {
            string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\314-footfall\nodal-acceleration.txt";

            var results = ResultsReader.Parse(file_path);

            Assert.IsTrue(results[0].GetType() == typeof(NodalAcceleration), "Nodal Accelerations should be parsed");
            Assert.IsTrue(results[results.Count - 1].GetType() == typeof(NodalAcceleration), "Nodal Accelerations should be parsed");

            Console.WriteLine(results);
        }


        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Footfall analysis, Nodal accelerations, SE.1 - Overall maximum - for selected objects"
            };

            foreach (var header in headers)
            {
                var match = NodalAcceleration.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(NodalAcceleration).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Footfall analysis, Nodal accelerations, SE.1 - Overall maximum - for selected objects",
                "ID	Node	ax	ay	az",
                "[-]	[-]	[-]	[-]	[-]"
            };

            foreach (var header in headers)
            {
                var match = NodalAcceleration.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }
    }
}