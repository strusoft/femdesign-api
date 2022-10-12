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
    public class LoadsTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("Moving loads deserialize")]
        public void MovingLoadsTest1()
        {
            Model model = Model.DeserializeFromFilePath("Loads/MovingLoads - No indexed Guid.struxml");
            Assert.IsNotNull(model, "Should read model with no IndexedGuid");

            Assert.IsNotNull(model.Entities.Loads.MovingLoads);
            Assert.AreEqual(1, model.Entities.Loads.MovingLoads.Count);
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Moving loads deserialize-serialize")]
        public void MovingLoadsTest2()
        {
            string path = "Loads/MovingLoads - No indexed Guid.struxml";
            string outPath = "Loads/out.struxml";
            Model actual = Model.DeserializeFromFilePath(path);

            actual.SerializeModel(outPath);
            Model expected = Model.DeserializeFromFilePath(outPath);

            Assert.AreEqual(expected.Entities.Loads.MovingLoads.Count, expected.Entities.Loads.MovingLoads.Count);
        }
    }
}