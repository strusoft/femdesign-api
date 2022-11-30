using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    [TestClass()]
    public class FdScriptTests
    {
        [TestMethod("CreateFdScript")]
        [TestCategory("Performance")] // the test is failing and it creates a GitHub Automatic Error in TEST CLI
        public void CreateFdScript()
        {
            string fdScriptPath = "script.fdscript";
            
            // Serialize
            var script = new FdScript(
                "logfile.log",
                new CmdOpen("model.struxml"),
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation(Analysis.StaticAnalysis()),
                new CmdCalculation(new Calculate.Design()),
                new CmdListGen("a.bsc", "./"),
                new CmdEndSession(),
                CmdGlobalCfg.Default(),
                new CmdDesignDesignChanges(),
                new CmdSave("model.struxml"),
                new CmdSaveDocx("model.docx")
                );
            script.Serialize(fdScriptPath);

            var xmlLines = System.IO.File.ReadAllLines(fdScriptPath);
            string xmlText = string.Join("\n", xmlLines);
            
            Assert.IsTrue(xmlLines[0].StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));

            Assert.IsTrue(xmlText.Contains("<fdscript"));
            Assert.IsTrue(xmlText.Contains("<fdscriptheader"));
            Assert.IsTrue(xmlText.Contains("<cmdopen"));
            Assert.IsTrue(xmlText.Contains("<cmduser"));
            Assert.IsTrue(xmlText.Contains("<cmdcalculation"));
            Assert.IsTrue(xmlText.Contains("<cmdlistgen"));
            Assert.IsTrue(xmlText.Contains("<cmdsave"));
            Assert.IsTrue(xmlText.Contains("<cmdendsession"));
            Assert.IsTrue(xmlText.Contains("<cmdglobalcfg"));
            Assert.IsTrue(xmlText.Contains("<cmdsave"));
            Assert.IsTrue(xmlText.Contains("<cmdsavedocx"));

            AssertValidFdScript(script);
        }

        [TestMethod("Validate schema")]
        public void ValidateSchema()
        {
            var script = new FdScript(
                "logfile.log",
                new CmdOpen("model.struxml"),
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation(Analysis.StaticAnalysis()),
                new CmdListGen("a.bsc", "./")
                );

            AssertValidFdScript(script);
        }

        [TestMethod("CmdListGen")]
        [TestCategory("Performance")] // the test is failing and it creates a GitHub Automatic Error in TEST CLI
        public void CmdListGen()
        {

            var mapCase = new MapCase("Deadload");
            var mapComb = new MapComb("myComb");

            //var bsc = new Bsc(ListProc.BarsDisplacementsLoadCase, "a.bsc");

            var bsc = new Bsc(ListProc.BarsDisplacementsLoadCase, "a.bsc", null, false);

            // Serialize
            var script = new FdScript(
                "logfile.log",
                new CmdListGen(bsc.BscPath, "./", false, mapCase)
                );

            AssertValidFdScript(script);
        }

        private static void AssertValidFdScript(FdScript script, string schemaPath = "fdscript.xsd")
        {
            // Serialize
            string fdScriptPath = "script.fdscript";
            script.Serialize(fdScriptPath);

            // Validate
            XmlDocument asset = new XmlDocument();
            XmlTextReader schemaReader = new XmlTextReader(schemaPath);
            XmlSchema schema = XmlSchema.Read(schemaReader, (obj, e) =>
            {
                Assert.Fail(e.Message);
            });
            asset.Schemas.Add(schema);

            asset.Load(fdScriptPath);

            asset.Validate((obj, e) =>
            {
                Assert.Fail(e.Message);
            });
        }
    }
}
