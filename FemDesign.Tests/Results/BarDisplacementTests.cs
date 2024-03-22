using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FemDesign.Results
{
    [TestClass()]
    public class BarDisplacementTests
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Bars, Displacements, Ultimate - Load case: IL
Bar	x	ex'	ey'	ez'	fix'	fiy'	fiz'
[-]	[m]	[mm]	[mm]	[mm]	[rad]	[rad]	[rad]
B.1.1	0.000	1.061	-0.130	-0.416	0.000	0.000	0.001
B.1.1	0.141	1.061	-0.019	-0.433	0.000	0.000	0.001
B.1.1	0.141	1.061	-0.019	-0.433	0.000	0.000	0.001
B.1.1	0.282	1.061	0.092	-0.434	0.000	0.000	0.001
B.1.1	0.282	1.061	0.092	-0.434	0.000	0.000	0.001
B.1.1	0.423	1.061	0.203	-0.433	0.000	0.000	0.001
B.1.1	0.423	1.061	0.203	-0.433	0.000	0.000	0.001
B.1.1	0.563	1.062	0.314	-0.448	0.000	0.000	0.001

");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results[0].GetType() == typeof(BarDisplacement), "FeaNode should be parsed");
            Assert.IsTrue(results.Count == 8, "Should read all results.");

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Bars, Displacements, Ultimate - Load case: IL",
                "Bars, Displacements, Quasi-Permanent - Load case: IL",
            };

            foreach (var header in headers)
            {
                var match = BarDisplacement.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(BarDisplacement).Name}");
                Assert.IsTrue(match.Groups["casename"].Success);
                Assert.IsTrue(match.Groups["type"].Success);
                Assert.IsTrue(match.Groups["result"].Success);
                Assert.IsTrue(match.Groups["loadcasetype"].Success);
                Assert.IsTrue(match.Groups["casecomb"].Success);
                Assert.IsTrue(match.Groups["casename"].Success);
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Bars, Displacements, Ultimate - Load case: IL",
                "Bar	x	ex'	ey'	ez'	fix'	fiy'	fiz'",
                "[-]	[m]	[mm]	[mm]	[mm]	[rad]	[rad]	[rad]"
            };

            foreach (var header in headers)
            {
                var match = BarDisplacement.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }

        [TestMethod]
        [TestCategory("FEM-Design required")]
        public void GetResultMethodTests()
        {
            string filepath = "Results\\Assets\\Model.str";

            using(var connection = new FemDesignConnection())
            {
                connection.Open(filepath);

                var model = connection.GetModel();
                var bars = model.Entities.Bars;
                var elements = new List<FemDesign.Bars.Bar> { bars[1], bars[2] };
                var elemIds = elements.Select(e => e.BarPart.Name).ToList();

                var loads = connection.GetLoads();
                var loadCases = loads.LoadCases.Select(c => c.Name).ToList();
                var loadCombs = loads.LoadCombinations.Select(c => c.Name).ToList();

                //------------------------------------------------------------------
                // Test GetResults() method
                //------------------------------------------------------------------

                // Get all of the BarDisplacement results
                var allRes = connection.GetResults<BarDisplacement>().OrderBy(r => r.Id).ToList();
                Assert.IsTrue(allRes.Count != 0);

                List<BarDisplacement> filteredAllRes = new List<BarDisplacement>();
                foreach(var id in elemIds)
                {
                    var filteredRes = allRes.Where(r => r.Id == id).ToList();
                    filteredAllRes.AddRange(filteredRes);
                }
                filteredAllRes = filteredAllRes.OrderBy(r => r.Pos).OrderBy(r => r.CaseIdentifier).OrderBy(r => r.Id).ToList();


                // Get all of the BarDisplacement results by structural elements
                var structElements = elements.Select(e => (FemDesign.GenericClasses.IStructureElement)e).ToList();
                var allResByElements = connection.GetResults<BarDisplacement>(elements: structElements).OrderBy(r => r.Pos).OrderBy(r => r.CaseIdentifier).OrderBy(r => r.Id).ToList();
                Assert.IsTrue(allRes.Count != 0);


                // Check
                Assert.AreEqual(allResByElements.Count, filteredAllRes.Count);
                for(int i = 0; i < allResByElements.Count; i++)
                {                    
                    PropertyInfo[] properties = typeof(BarDisplacement).GetProperties();
                    properties = properties.Where(p => p.Name != nameof(BarDisplacement.CaseIdentifier)).ToArray();

                    foreach(var prop in properties)
                    {
                        var item1 = prop.GetValue(allResByElements[i]);
                        var item2 = prop.GetValue(filteredAllRes[i]);
                        Assert.AreEqual(item1, item2);
                    }
                    string caseId1 = allResByElements[i].CaseIdentifier.Replace(" - selected objects", "");
                    string caseId2 = filteredAllRes[i].CaseIdentifier;
                    Assert.AreEqual(caseId1, caseId2);
                }




            }
        }
    }
}
