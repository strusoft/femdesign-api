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


        [TestMethod("Case Name")]
        public void LoadCaseName()
        {
            var raiseErrorText = new List<string>
            {
                new String('a', 81),
                "\n case",
                "\n",
                "\n\n",
            };

            var validText = new List<string>
            {
                "wind",
                "wind snow",
                "åö - ö"
            };


            foreach (var text in raiseErrorText)
                Assert.ThrowsException<ArgumentException>(() => new LoadCase(text, LoadCaseType.Static, LoadCaseDuration.Permanent));


            foreach (var text in validText)
                new LoadCase(text, LoadCaseType.Static, LoadCaseDuration.Permanent);
        }

    }
}