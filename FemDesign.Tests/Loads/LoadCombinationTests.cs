using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Loads
{
    [TestClass()]
    public class LoadCombinationTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("Load combinations - Deserialize")]
        public void LoadCombinationTest1()
        {
            Model model = Model.DeserializeFromFilePath("Loads/Load combination case types.struxml");

            Assert.AreEqual(4, model.Entities.Loads.LoadCombinations.Count);
            var comb1 = model.Entities.Loads.LoadCombinations[0];
            var comb2 = model.Entities.Loads.LoadCombinations[1];
            var comb3 = model.Entities.Loads.LoadCombinations[2];
            var comb4 = model.Entities.Loads.LoadCombinations[3];

            // LoadCase and moving load load cases
            Assert.AreEqual(1, comb1.ModelLoadCase.Count);
            Assert.AreEqual(1, comb2.ModelLoadCase.Count);
            Assert.AreEqual(1, comb3.ModelLoadCase.Count);
            Assert.AreEqual(1, comb4.ModelLoadCase.Count);

            Assert.IsFalse(comb1.ModelLoadCase[0].IsMovingLoadLoadCase);
            Assert.IsFalse(comb2.ModelLoadCase[0].IsMovingLoadLoadCase);
            Assert.IsFalse(comb3.ModelLoadCase[0].IsMovingLoadLoadCase);
            Assert.IsTrue(comb4.ModelLoadCase[0].IsMovingLoadLoadCase);

            Assert.IsNotNull(comb1.ModelLoadCase[0].LoadCase);
            Assert.IsNotNull(comb2.ModelLoadCase[0].LoadCase);
            Assert.IsNotNull(comb3.ModelLoadCase[0].LoadCase);
            Assert.IsNull(comb4.ModelLoadCase[0].LoadCase);

            // Seismic
            Assert.IsNotNull(comb2.SeismicMax);
            Assert.IsNotNull(comb2.SeismicResFxMinusMx);
            Assert.IsNotNull(comb2.SeismicResFxPlusMx);
            Assert.IsNotNull(comb2.SeismicResFyMinusMy);
            Assert.IsNotNull(comb2.SeismicResFyPlusMy);
            Assert.IsNotNull(comb2.SeismicResFz);

            // PTC
            Assert.IsNotNull(comb3.PtcT0);
            Assert.IsNotNull(comb3.PtcT8);

            // Pile
            Assert.IsNotNull(comb3.PileLoadCase);

            // Construction stages
            Assert.AreEqual("cs.1", comb1.StageLoadCase._stageType);
            Assert.AreEqual(1, comb1.StageLoadCase.StageIndex);
            Assert.IsFalse(comb1.StageLoadCase.IsFinalStage);
            Assert.IsNotNull(comb1.StageLoadCase.Stage);

            Assert.AreEqual("cs.2", comb2.StageLoadCase._stageType);
            Assert.AreEqual(2, comb2.StageLoadCase.StageIndex);
            Assert.IsFalse(comb2.StageLoadCase.IsFinalStage);
            Assert.IsNotNull(comb2.StageLoadCase.Stage);

            Assert.AreEqual("final_cs", comb3.StageLoadCase._stageType);
            Assert.AreEqual(-1, comb3.StageLoadCase.StageIndex);
            Assert.IsTrue(comb3.StageLoadCase.IsFinalStage);
            Assert.IsNull(comb3.StageLoadCase.Stage);
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Load combinations - Serialize")]
        public void LoadCombinationTest2()
        {
            Model expected = Model.DeserializeFromFilePath("Loads/Load combination case types.struxml");
            expected.SerializeModel("Loads/out.struxml");
            Model actual = Model.DeserializeFromFilePath("Loads/out.struxml");

            Assert.AreEqual(expected.Entities.Loads.LoadCombinations.Count, actual.Entities.Loads.LoadCombinations.Count);
            var e = expected.Entities.Loads.LoadCombinations;
            var a = actual.Entities.Loads.LoadCombinations;

            for (int i = 0; i < a.Count; i++)
            {
                Assert.AreEqual(e[i].ModelLoadCase.Count, a[i].ModelLoadCase.Count);
                for (int j = 0; j < a[i].ModelLoadCase.Count; j++)
                    Assert.AreEqual(e[i].ModelLoadCase[j].Gamma, a[i].ModelLoadCase[j].Gamma);

                Assert.AreEqual(e[i].SeismicMax?.Gamma, a[i].SeismicMax?.Gamma);
                Assert.AreEqual(e[i].SeismicResFxPlusMx?.Gamma, a[i].SeismicResFxPlusMx?.Gamma);
                Assert.AreEqual(e[i].SeismicResFxMinusMx?.Gamma, a[i].SeismicResFxMinusMx?.Gamma);
                Assert.AreEqual(e[i].SeismicResFyPlusMy?.Gamma, a[i].SeismicResFyPlusMy?.Gamma);
                Assert.AreEqual(e[i].SeismicResFyMinusMy?.Gamma, a[i].SeismicResFyMinusMy?.Gamma);
                Assert.AreEqual(e[i].SeismicResFz?.Gamma, a[i].SeismicResFz?.Gamma);

                Assert.AreEqual(e[i].PtcT0?.Gamma, a[i].PtcT0?.Gamma);
                Assert.AreEqual(e[i].PtcT8?.Gamma, a[i].PtcT8?.Gamma);

                Assert.AreEqual(e[i].PileLoadCase?.Gamma, a[i].PileLoadCase?.Gamma);

                Assert.AreEqual(e[i].StageLoadCase?._stageType, a[i].StageLoadCase?._stageType);
            }
        }
    }
}