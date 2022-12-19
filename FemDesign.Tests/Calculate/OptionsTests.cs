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
            var options = new Options(BarResultPosition.OnlyNodes, ShellResultPosition.Vertices, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 0);
            Assert.IsTrue(options.SrfValues == 0);
            Assert.IsNull(options.Step);

            options = new Options(BarResultPosition.ByStep, ShellResultPosition.Center, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 1);
            Assert.IsTrue(options.SrfValues == 1);
            Assert.IsNotNull(options.Step);

            options = new Options(BarResultPosition.ByStep, ShellResultPosition.Vertices, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 1);
            Assert.IsTrue(options.SrfValues == 0);
            Assert.IsNotNull(options.Step);

            options = new Options(BarResultPosition.ResultPoint, ShellResultPosition.ResultPoint, 0.50);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Bar == 2);
            Assert.IsTrue(options.SrfValues == 2);
            Assert.IsNull(options.Step);

        }
    }
}