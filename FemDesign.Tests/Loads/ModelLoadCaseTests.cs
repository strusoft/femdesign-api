using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Loads.Tests
{
    [TestClass()]
    public class ModelLoadCaseTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("ModelLoadCase - No IndexedGuid")]
        public void ModelLoadCaseIndexedGuidTest1()
        {
            Model model = Model.DeserializeFromFilePath("Loads/MovingLoads - No indexed Guid.struxml");
            Assert.IsNotNull(model, "Should read model with no IndexedGuid");
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("ModelLoadCase - IndexedGuid")]
        public void ModelLoadCaseIndexedGuidTest2()
        {
            Model model = Model.DeserializeFromFilePath("Loads/MovingLoads - With indexed Guid.struxml");
            Assert.IsNotNull(model, "Should read model with IndexedGuid types");
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("ModelLoadCase - IndexedGuid - serialize-deserialize")]
        public void ModelLoadCaseIndexedGuidTest3()
        {
            string path = "Loads/MovingLoads - With indexed Guid.struxml";
            string outPath = "Loads/MovingLoads - With indexed Guid - Out.struxml";
            Model expected = Model.DeserializeFromFilePath(path);
            expected.SerializeModel(outPath);

            Model actual = Model.DeserializeFromFilePath(outPath);

            Assert.AreEqual(
                expected.Entities.Loads.LoadCases.Count,
                actual.Entities.Loads.LoadCases.Count);
            Assert.AreEqual(
                expected.Entities.Loads.LoadCombinations.Count,
                actual.Entities.Loads.LoadCombinations.Count);
            Assert.AreEqual(
                expected.Entities.Loads.LoadGroupTable.GeneralLoadGroups.Count,
                actual.Entities.Loads.LoadGroupTable.GeneralLoadGroups.Count);

            var expectedLoadComb = expected.Entities.Loads.LoadCombinations[0];
            var actualLoadComb = actual.Entities.Loads.LoadCombinations[0];

            Assert.AreEqual(
                expectedLoadComb.ModelLoadCase.Count,
                actualLoadComb.ModelLoadCase.Count);
            Assert.AreEqual(
                expectedLoadComb.ModelLoadCase[0].IndexedGuid,
                actualLoadComb.ModelLoadCase[0].IndexedGuid);
        }
    }
}