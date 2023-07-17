using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TenrecGeneratedTests
{
    [TestClass]
    public class AutoTest_TenrecTest
    {
        public string FilePath
        {
            get
            {
                string baseDirectory = Directory.GetCurrentDirectory();
                string fullPath = Path.Combine(baseDirectory, @"GhScript\TenrecTest.gh");
                return fullPath;
            }
        }

        private TestContext testContextInstance;
        public TestContext TestContext { get => testContextInstance; set => testContextInstance = value; }
        [TestMethod]
        public void PointDistance()
        {
            Tenrec.Runner.Initialize(TestContext);
            Tenrec.Runner.RunTenrecGroup(FilePath, new System.Guid("431f2ccd-058d-4d8d-88ef-718ccc7c4773"), TestContext);
        }
        [TestMethod]
        public void Test_true()
        {
            Tenrec.Runner.Initialize(TestContext);
            Tenrec.Runner.RunTenrecGroup(FilePath, new System.Guid("82fa3bf5-7339-4f16-b78a-27ba824d7988"), TestContext);
        }
        [TestMethod]
        public void Test_False()
        {
            Tenrec.Runner.Initialize(TestContext);
            Tenrec.Runner.RunTenrecGroup(FilePath, new System.Guid("23bd3d1b-1213-4ff2-8231-35dd66a3e985"), TestContext);
        }
        [TestMethod]
        public void asserting_false()
        {
            Tenrec.Runner.Initialize(TestContext);
            Tenrec.Runner.RunTenrecGroup(FilePath, new System.Guid("9679c999-b00f-459e-8403-9b9118e17215"), TestContext);
        }
        [TestMethod]
        public void RunLoadCaseAnalysis()
        {
            Tenrec.Runner.Initialize(TestContext);
            Tenrec.Runner.RunTenrecGroup(FilePath, new System.Guid("ed24620c-14a6-4236-9eba-9491279e86a1"), TestContext);
        }
    }

}
