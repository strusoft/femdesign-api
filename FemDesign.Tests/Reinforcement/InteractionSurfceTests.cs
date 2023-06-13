using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FemDesign;

namespace FemDesign.Reinforcement
{
    [TestClass]
    public class InteractionSurfaceTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod]
        public void Deserialise()
        {
            var filePath = "Reinforcement\\intSrf-1.txt";
            var results = FemDesign.Results.InteractionSurface.ReadFromFile(filePath);

            Assert.IsTrue(results.Vertices.Count == 322);
            Assert.IsTrue(results.Faces.Count == 640);
        }
    }
}
