using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples.Tests
{
    [TestClass()]
    public class SampleProgramTests
    {
        [TestMethod()]
        public void Example_1_CreateSimpleModelTest()
        {
            var actual = SampleProgram.Example_1_CreateSimpleModel();
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Entities.Bars.Count, 1);
            Assert.AreEqual(actual.Entities.Supports.GetSupports().Count, 2);
            Assert.AreEqual(actual.Entities.Loads.GetLoads().Count, 3);
        }
    }
}