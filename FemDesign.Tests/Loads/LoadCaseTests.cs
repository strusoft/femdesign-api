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
    public class LoadCaseTests
    {
        [TestMethod("LoadCase - Constructor")]
        public void LoadCaseTest1()
        {
            var lc = new LoadCase("Name", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            Assert.IsNotNull(lc);
        }
    }
}