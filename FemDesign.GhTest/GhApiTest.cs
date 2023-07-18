using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace FemDesignGhTests
{
    [TestClass]
    public partial class AutoTest_ConstructModel
    {
        public string FilePath => GetFile("GhScript/ConstructModel.gh");
        private TestContext testContextInstance;
        public TestContext TestContext { get => testContextInstance; set => testContextInstance = value; }
        public static string GetFile(string filename)
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            return Path.Combine(baseDirectory, filename);
        }
        [TestMethod]
        public void ModelConstruct()
        {
            Tenrec.Runner.Initialize(TestContext);
            Tenrec.Runner.RunTenrecGroup(FilePath, new System.Guid("ed24620c-14a6-4236-9eba-9491279e86a1"), TestContext);
        }
    }

    [TestClass]
    public partial class AutoTest_RunAnalysis
    {
        public string FilePath => GetFile("GhScript/RunAnalysis.gh");
        private TestContext testContextInstance;
        public TestContext TestContext { get => testContextInstance; set => testContextInstance = value; }
        public static string GetFile(string filename)
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            return Path.Combine(baseDirectory, filename);
        }
        [TestMethod]
        public void RunLoadCase()
        {
            Tenrec.Runner.Initialize(TestContext);
            Tenrec.Runner.RunTenrecGroup(FilePath, new System.Guid("bcf473de-bb78-45fa-bbf4-c31d1ad011cd"), TestContext);
        }
    }

}
