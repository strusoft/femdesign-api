using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using FuzzySharp;


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


        [TestMethod("Fuzz")]
        public void test()
        {
            var db = SectionDatabase.GetDefault();

            var choices = db.Sections.Section.Select(s => s._sectionName).ToArray();
            var a = FuzzySharp.Process.ExtractOne("HEA100", choices);
            var b = FuzzySharp.Process.ExtractOne("CHS, 30-5.6", choices);
            var c = FuzzySharp.Process.ExtractOne("CHS 30/5.6", choices);
            var d = FuzzySharp.Process.ExtractOne("hea 100", choices);
            var e = FuzzySharp.Process.ExtractOne("150x300", choices);

            var f = FuzzySharp.Process.ExtractOne("150x20", choices);

            var test = 1;

            f = FuzzySharp.Process.ExtractOne("150", choices);

            test = 1;

        }
    }
}