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
        public void TestGetStabilityResults()
        {
            string struxmlPath = "Results\\Stability\\ReadBucklingShapesTest.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            var stability = new Calculate.Stability(new List<string> { "LC2ULS" }, new List<int> { 10 }, false, 5);
            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(stability: stability, calcComb : true, calcStab : true);


            using (var femDesign = new FemDesignConnection(fdInstallationDir : @"C:\Program Files\StruSoft\FEM-Design 23 Night Install\", outputDir: "My analyzed model", keepOpen: false))
            {
                femDesign.RunAnalysis(model, analysis);
                                
                var resultsBuckling = new List<Results.NodalBucklingShape>();
                var critParam = new List<Results.CriticalParameter>();

                List<string> allCombNames = model.Entities.Loads.LoadCombinations.Select(r => r.Name).ToList();
                var combName = new List<string>() { allCombNames[1]};
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

        //[TestCategory("FEM-Design required")]
        //[TestMethod]
        //public void TestGetEigenResults()
        //{
        //    // get model data
        //    string struxmlPath = "Results\\Stability\\ReadBucklingShapesTest.struxml";
        //    Model model = Model.DeserializeFromFilePath(struxmlPath);
        //    List<string> loadCombinations = model.Entities.Loads.LoadCombinations.Select(l => l.Name).ToList();

        //    // setup stability analysis
        //    List<List<string>> validCombos = new List<List<string>>
        //    {
        //        new List<string>(){ "LC1ULS" },
        //        new List<string>(){ "LC2ULS" },
        //        loadCombinations
        //    };
        //    List<List<string>> invalidCombos = new List<List<string>>
        //    {
        //        new List<string>(){ "Lc1uLs" },
        //        new List<string>(){ "LC5ULS" }
        //    };
        //    List<List<string>> combos = validCombos.Concat(invalidCombos).ToList();
        //    List<List<int>> reqShapes = new List<List<int>>
        //    {
        //        new List<int>(){ 8},
        //        new List<int>(){ 8},
        //        new List<int>(){ 10, 15 },
        //        new List<int>(){ 8 },
        //        new List<int>(){ 8 },
        //    };

        //    var stab = new List<Calculate.Stability>();
        //    var analysis = new List<Calculate.Analysis>();
        //    for (int i = 0; i < combos.Count; i++)
        //    {
        //        stab.Add(new Calculate.Stability(combos[i], reqShapes[i], false, 5));
        //        analysis.Add(new FemDesign.Calculate.Analysis(stability: stab[i], calcComb: true, calcStab: true));

        //    }


        //    List<List<string>> combos2 = invalidCombos.Concat(validCombos).ToList();
        //    List<List<int>> shapeIds = new List<List<int>>
        //    {
        //        new List<int>(){ 8, 4, 10 },
        //        new List<int>(){ 1, 3, 7, 8 },
        //        new List<int>(){ 10, 15 },
        //        new List<int>(){ 8 },
        //        new List<int>(){ 8 },
        //    };
        //    var bucklRes = new List<List<NodalBucklingShape>>();
        //    var bucklRes2 = new List<List<NodalBucklingShape>>();
        //    var critParams = new List<List<CriticalParameter>>();
        //    var critParams2 = new List<List<CriticalParameter>>();

        //    using (var femDesign = new FemDesignConnection(fdInstallationDir: @"C:\Program Files\StruSoft\FEM-Design 23\", outputDir: "StabilityResultsTest", keepOpen: false))
        //    {
        //        // open model
        //        femDesign.Open(model);


        //        for (int i = 0; i < combos.Count; i++)
        //        {
        //            // run analysis
        //            femDesign.RunAnalysis(analysis[i]);

        //            // get results
        //            bucklRes.Add(femDesign.GetEigenResults<Results.NodalBucklingShape>(combos[i], shapeIds[i]));
        //            bucklRes2.Add(femDesign.GetEigenResults<Results.NodalBucklingShape>(combos2[i], shapeIds[i]));
        //            critParams.Add(femDesign.GetEigenResults<Results.CriticalParameter>(combos[i], shapeIds[i]));
        //            critParams2.Add(femDesign.GetEigenResults<Results.CriticalParameter>(combos2[i], shapeIds[i]));
        //        }
        //    }

        //    // check results
        //    for (int i = 0; i < validCombos.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes[i].Count != 0);
        //    }
        //    for (int i = validCombos.Count; i < combos.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes[i].Count == 0);
        //    }
        //    for (int i = 0; i < invalidCombos.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes2[i].Count == 0);
        //    }
        //    for (int i = invalidCombos.Count + 1; i < combos2.Count; i++)
        //    {
        //        Assert.IsTrue(bucklRes2[i].Count == 0);
        //    }
        //}
    }
}