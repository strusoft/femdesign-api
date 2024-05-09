using FemDesign.Loads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Summary description for Stability
    /// </summary>
    [TestClass]
    public class StabilityTests
    {
        private static List<Loads.LoadCombination> LoadCombinationTests()
        {
            var lc_1 = new FemDesign.Loads.LoadCase("DL", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);

            var comb1 = new FemDesign.Loads.LoadCombination("combo_1", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb2 = new FemDesign.Loads.LoadCombination("combo_2", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb3 = new FemDesign.Loads.LoadCombination("combo_3", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb4 = new FemDesign.Loads.LoadCombination("combo_4", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb5 = new FemDesign.Loads.LoadCombination("combo_5", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb6 = new FemDesign.Loads.LoadCombination("combo_6", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb7 = new FemDesign.Loads.LoadCombination("combo_7", LoadCombType.UltimateOrdinary, (lc_1, 1.0));

            var loadCombinations = new List<Loads.LoadCombination> { comb1, comb2, comb3, comb4, comb5, comb6, comb7 };
            return loadCombinations;
        }
        private static FemDesign.Calculate.Stability Stability_1()
        {
            var lc_1 = new FemDesign.Loads.LoadCase("DL", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);

            var comb1 = new FemDesign.Loads.LoadCombination("combo_1", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb2 = new FemDesign.Loads.LoadCombination("combo_2", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb3 = new FemDesign.Loads.LoadCombination("combo_3", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb4 = new FemDesign.Loads.LoadCombination("combo_4", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb5 = new FemDesign.Loads.LoadCombination("combo_5", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb6 = new FemDesign.Loads.LoadCombination("combo_6", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb7 = new FemDesign.Loads.LoadCombination("combo_7", LoadCombType.UltimateOrdinary, (lc_1, 1.0));

            var loadCombinations = new List<Loads.LoadCombination> { comb1, comb2, comb3, comb4, comb5, comb6, comb7 };

            var numShapes = new List<int> { 1,2,3,4,5,6,7};
            var positiveOnly = false;
            var numberIteration = 5;

            return new FemDesign.Calculate.Stability(loadCombinations.Select(x => x.Name).ToList(), numShapes, positiveOnly, numberIteration);
        }


        private static FemDesign.Calculate.Stability Stability_2()
        {
            var lc_1 = new FemDesign.Loads.LoadCase("DL", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);

            var comb1 = new FemDesign.Loads.LoadCombination("combo_1", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb2 = new FemDesign.Loads.LoadCombination("combo_2", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb6 = new FemDesign.Loads.LoadCombination("combo_6", LoadCombType.UltimateOrdinary, (lc_1, 1.0));
            var comb7 = new FemDesign.Loads.LoadCombination("combo_7", LoadCombType.UltimateOrdinary, (lc_1, 1.0));

            var loadCombinations = new List<Loads.LoadCombination> { comb1, comb2, comb6, comb7 };

            var numShapes = new List<int> { 1, 2, 6, 7 };
            var positiveOnly = false;
            var numberIteration = 5;

            return new FemDesign.Calculate.Stability(loadCombinations.Select(x => x.Name).ToList(), numShapes, positiveOnly, numberIteration);
        }


        [TestMethod("Stability_2")]
        public void TestMethod2()
        {
            var stability = new Analysis(stability: Stability_2(), calcStab: true);

            var loadCombination = GetLoadCombinationsTests();
            stability._setStabilityAnalysis(loadCombination.Values.ToList());

            var path = Path.Combine(Directory.GetCurrentDirectory(), "FEM-Design API");
            string logfile = OutputFileHelper.GetLogfilePath(path);
            var script = new FdScript(
                logfile,
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation(stability)
            );

            var cmdCalculate = (FemDesign.Calculate.CmdCalculation)script.Commands[1];


            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[0].StabRqd == stability.Stability.NumShapes[0]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[1].StabRqd == stability.Stability.NumShapes[1]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[2].StabRqd == 0);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[3].StabRqd == 0);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[4].StabRqd == 0);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[5].StabRqd == stability.Stability.NumShapes[2]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[6].StabRqd == stability.Stability.NumShapes[3]);
        }


        [TestMethod("Stability_1")]
        public void TestMethod1()
        {
            var stability = new Analysis(stability: Stability_1(), calcStab: true);

            //stability.Comb.CombItem.Clear();
            var loadCombination = GetLoadCombinationsTests();
            stability._setStabilityAnalysis(loadCombination.Values.ToList());

            var path = Path.Combine(Directory.GetCurrentDirectory(), "FEM-Design API");
            string logfile = OutputFileHelper.GetLogfilePath(path);
            var script = new FdScript(
                logfile,
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation(stability)
            );

            var cmdCalculate = (FemDesign.Calculate.CmdCalculation)script.Commands[1];

            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[0].StabRqd == stability.Stability.NumShapes[0]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[1].StabRqd == stability.Stability.NumShapes[1]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[2].StabRqd == stability.Stability.NumShapes[2]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[3].StabRqd == stability.Stability.NumShapes[3]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[4].StabRqd == stability.Stability.NumShapes[4]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[5].StabRqd == stability.Stability.NumShapes[5]);
            Assert.IsTrue(cmdCalculate.Analysis.Comb.CombItem[6].StabRqd == stability.Stability.NumShapes[6]);
        }



        private Dictionary<int, Loads.LoadCombination> GetLoadCombinationsTests()
        {
            var loadCombinations = LoadCombinationTests();

            var dictLoadComb = new Dictionary<int, Loads.LoadCombination>();

            int index = 0;
            foreach (var comb in loadCombinations)
            {
                dictLoadComb.Add(index, comb);
                index++;
            }

            return dictLoadComb;
        }


    }
}
