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
    public class LoadCaseTests
    {
        [TestMethod("LoadCase - Constructor")]
        public void LoadCaseTest1()
        {
            var lc = new LoadCase("Name", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            Assert.IsNotNull(lc);
        }

        [TestMethod("LoadCase - Serialize indexed guid")]
        public void LoadCaseTest2()
        {
            LoadCase lc1 = new LoadCase("Load case 1", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase lc2 = new LoadCase("Load case 2 with indexed guid", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            lc2.IndexedGuid.Index = 1;
            var loadCases = new List<LoadCase> { lc1, lc2 };

            string path = "LoadCases.struxml";
            Model model = new Model(Country.COMMON);
            model.AddLoadCases(loadCases);
            model.SerializeModel(path);

            Model deserialized = Model.DeserializeFromFilePath(path);

            var deserializedCases = deserialized.Entities.Loads.LoadCases;

            Assert.IsTrue(loadCases[0].IndexedGuid == deserializedCases[0].IndexedGuid);
            Assert.IsTrue(loadCases[1].IndexedGuid == deserializedCases[1].IndexedGuid);
            
            Assert.IsFalse(deserializedCases[0].IndexedGuid.HasIndex);
            
            Assert.IsTrue(deserializedCases[1].IndexedGuid.HasIndex);
            Assert.IsTrue(deserializedCases[1].IndexedGuid.Index == 1);
        }
    }
}