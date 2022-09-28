using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FemDesign.Materials
{
    [TestClass()]
    public class MaterialDatabaseTests
    {
        [TestMethod("MaterialDatabase.GetDefault - Speed")]
        [TestCategory("Performance")]
        public void GetDefaultTest()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            MaterialDatabase.GetDefault();
            sw.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds < 500, $"GetDefault once should be fast. {sw.ElapsedMilliseconds}");

            sw.Restart();
            for (int i = 0; i < 10; i++)
                MaterialDatabase.GetDefault();
            sw.Stop();
            Assert.IsTrue(sw.ElapsedMilliseconds < 100, $"GetDefault many times after the first should be very fast. {sw.ElapsedMilliseconds}");
        }
    }
}