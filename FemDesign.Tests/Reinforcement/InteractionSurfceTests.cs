using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FemDesign;

namespace FemDesign.Reinforcement
{
    [TestClass]
    public class InteractionSurface
    {
        [TestCategory("FEM-Design required")]
        [TestMethod]
        public void Deserialise()
        {
            var filePath = "Reinforcement\\intSrf-1.txt";
            var results = FemDesign.Results.InteractionSurface.ReadFromFile(filePath);


            var a = 2;

            filePath = "Reinforcement\\intSrf-2.txt";
            results = FemDesign.Results.InteractionSurface.ReadFromFile(filePath);

            a = 3;

            filePath = "Reinforcement\\intSrf.txt7cb0b0b6-96e9-4097-a8b6-bf5b8ae1b15c";
            results = FemDesign.Results.InteractionSurface.ReadFromFile(filePath);

            a = 3;
        }
    }
}
