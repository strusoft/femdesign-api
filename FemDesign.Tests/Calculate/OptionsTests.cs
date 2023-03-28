using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FemDesign.Calculate
{
    [TestClass()]
    public class OptionsTests
    {
        [TestMethod("Options")]
        public void TestOptions()
        {
            var options = new Options(BarResultPosition.OnlyNodes, ShellResultPosition.Center, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 0);
            Assert.IsTrue(options.SrfValues == 0);

            options = new Options(BarResultPosition.ByStep, ShellResultPosition.Vertices, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 1);
            Assert.IsTrue(options.SrfValues == 1);

            options = new Options(BarResultPosition.ByStep, ShellResultPosition.Vertices, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 1);
            Assert.IsTrue(options.SrfValues == 1);

            options = new Options(BarResultPosition.ResultPoints, ShellResultPosition.ResultPoints, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 2);
            Assert.IsTrue(options.SrfValues == 2);
        }


        [TestMethod("DeserialiseGlobalcfg")]
        public void DeserialiseGlobalcfg()
        {
            var globalCfg = Calculate.CmdGlobalCfg.DeserializeCmdGlobalCfgFromFilePath(@"Calculate\cmdglobalcfg.xml");
            Assert.IsNotNull(globalCfg);
        }
    }
}