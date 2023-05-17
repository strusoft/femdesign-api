using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FemDesign;


namespace FemDesign.Results
{
    [TestClass]
    public class InteractionSurfaceTests
    {
        [TestMethod]
        public void ReadIntSrfFile()
        {
            var filepath = @"Results/InteractionSurface/output.txt";

            var intSrf = FemDesign.Results.InteractionSurface.ReadFromFile(filepath);

            Assert.IsTrue( intSrf.Vertices.Count == 322 );
            Assert.IsTrue( intSrf.Faces.Count == 640 );

        }
    }
}
