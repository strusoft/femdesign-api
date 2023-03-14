using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FemDesign;

namespace FemDesign.Reinforcement
{
    [TestClass]
    public class ShearControlRegionTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod]
        public void Deserialise()
        {
            Model model = FemDesign.Model.DeserializeFromFilePath("Reinforcement\\ShearRegionTestFile.struxml");

            Assert.IsTrue( model.Entities.NoShearControlRegions.Count != 0 );
            Assert.IsTrue( model.Entities.NoShearControlRegions.Count == 2 );

            foreach(var obj in model.Entities.NoShearControlRegions)
            {
                Assert.IsTrue( obj.ReduceShearForces == true );
                Assert.IsTrue( obj.IgnoreShearCheck == true );
                Assert.IsTrue( obj.X == 1 );
            }
        }
    }
}
