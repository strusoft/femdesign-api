using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FemDesign.Sections
{
    [TestClass()]
    public class SectionDatabaseTests
    {
        [TestMethod("SectionDatabase.GetDefault - Speed")]
        [TestCategory("Performance")]
        public void GetDefaultTest()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            SectionDatabase.GetDefault();
            sw.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds < 500, $"GetDefault once should be fast. {sw.ElapsedMilliseconds}");

            sw.Restart();
            for (int i = 0; i < 10; i++)
                SectionDatabase.GetDefault();
            sw.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds < 100, $"GetDefault many times after the first should be very fast. {sw.ElapsedMilliseconds}");
        }
    }
}