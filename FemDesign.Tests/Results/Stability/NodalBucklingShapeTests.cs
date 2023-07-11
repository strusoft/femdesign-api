using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using static System.Net.Mime.MediaTypeNames;

namespace FemDesign.Results
{
    [TestClass]
    public class NodalBucklingShapeTest
    {
        [TestMethod]
        public void Parse()
        {
            string path = Path.GetTempFileName();

            using (var stream = new StreamWriter(path)) stream.Write(@"Nodal buckling shapes, ULS / 1 - for selected objects
ID	Node	ex	ey	ez	fix	fiy	fiz
[-]	[-]	[-]	[-]	[-]	[-]	[-]	[-]
B.1.1	32	-0.000	-0.005	-0.000	0.003	0.000	-0.010
B.1.1	35	-0.000	-0.007	-0.000	0.004	0.000	-0.009
B.1.1	41	-0.000	-0.011	-0.000	0.006	0.000	-0.009
B.1.1	26	-0.000	-0.002	-0.000	0.001	0.000	-0.010
B.1.1	44	-0.000	-0.012	-0.000	0.007	0.000	-0.009
B.1.1	47	-0.000	-0.014	-0.000	0.008	0.000	-0.009
");

            var results = ResultsReader.Parse(path);
            Assert.IsTrue(results.Count == 6);
            Assert.IsTrue(results.First().GetType() == typeof(NodalBucklingShape));
            Assert.IsTrue(results.Last().GetType() == typeof(NodalBucklingShape));

            File.Delete(path);
        }

        [TestMethod]
        public void Identification()
        {
            var headers = new string[]
            {
                "Nodal buckling shapes, LC1ULS / 1 - for selected objects"
            };

            foreach (var header in headers)
            {
                var match = NodalBucklingShape.IdentificationExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify type of \"{header}\" as {typeof(NodalBucklingShape).Name}");
            }
        }

        [TestMethod]
        public void Headers()
        {
            var headers = new string[]
            {
                "Nodal buckling shapes, LC1ULS / 1 - for selected objects",
                "ID\tNode\tex\tey\tez\tfix\tfiy\tfiz",
                "[-]\t[-]\t[-]\t[-]\t[-]\t[-]\t[-]\t[-]"
            };

            foreach (var header in headers)
            {
                var match = NodalBucklingShape.HeaderExpression.Match(header);
                Assert.IsTrue(match.Success, $"Should identify \"{header}\" as header");
            }
        }


        [TestCategory("FEM-Design required")]
        [TestMethod]
        public void TestReadStabilityResults()
        {
            string struxmlPath = "Results\\Stability\\ReadBucklingShapesTest.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(calcComb : true, calcStab : true);

            var combItem = new FemDesign.Calculate.CombItem(stabReq: 10);
            model.Entities.Loads.LoadCombinations.ForEach(lComb =>
            {
                lComb.CombItem = combItem;
            });

            analysis.SetLoadCombinationCalculationParameters(model);

            using (var femDesign = new FemDesignConnection(fdInstallationDir : @"C:\Program Files\StruSoft\FEM-Design 22\", outputDir: "My analyzed model", keepOpen: false))
            {
                femDesign.RunAnalysis(model, analysis);
                                
                var resultsBuckling = new List<Results.NodalBucklingShape>();
                var critParam = new List<Results.CriticalParameter>();

                List<string> allCombNames = model.Entities.Loads.LoadCombinations.Select(r => r.Name).ToList();
                var combName = new List<string>() { allCombNames[0]};
                List<int> id = new List<int>() { 5, 3};
                foreach(string c in combName)
                {
                    foreach (int i in id)
                    {
                        var res = femDesign.GetStabilityResults<Results.NodalBucklingShape>(c, i);
                        resultsBuckling.AddRange(res);
                        var crit = femDesign.GetStabilityResults<Results.CriticalParameter>(c, i);
                        critParam.AddRange(crit);
                    }
                }

                Assert.IsNotNull(resultsBuckling);
                Assert.IsNotNull(critParam);
            }
        }
    }
}