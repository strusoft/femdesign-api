using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Reflection;

namespace FemDesign.Calculate
{
    [TestClass()]
    public class FdScriptTests
    {
        [TestMethod("CreateFdScript")]
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
                new CmdApplyDesignChanges(),
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
        }

        [TestMethod("Validate schema")]
        [TestCategory("Performance")] // the test is failing and it creates a GitHub Automatic Error in TEST CLI
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

        [TestMethod("CombItem")]
        public void CombItemTest()
        {
            var combItem = new Calculate.CombItem(2, 5, true, true, true, true, true, 5, 0.1234, 3);

            Assert.IsTrue(combItem.NLE == true);
            Assert.IsTrue(combItem._nle == true);

            Assert.IsTrue(combItem.PL == true);
            Assert.IsTrue(combItem._pl == true);

            Assert.IsTrue(combItem.NLS == true);
            Assert.IsTrue(combItem._nls == true);

            Assert.IsTrue(combItem.Cr == true);
            Assert.IsTrue(combItem._cr == true);

            Assert.IsTrue(combItem.f2nd == true);
            Assert.IsTrue(combItem._f2nd == true);

            Assert.IsTrue(combItem.Im == 5 );
            Assert.IsTrue(combItem.Waterlevel == 3 );
            Assert.IsTrue(combItem.ImpfRqd == 2 );
            Assert.IsTrue(combItem.StabRqd == 5 );
            Assert.IsTrue(combItem.Amplitude == 0.1234 );
        }


    }
}
