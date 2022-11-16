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
        public void CreateFdScript()
        {
            // Serialize
            var script = new FdScript2(
                "logfile.log",
                new CmdOpen("model.struxml"),
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation(Analysis.StaticAnalysis()),
                new CmdListGen("a.bsc", "./")
                );
            script.Serialize("script.fdscript");

            var xmlLines = System.IO.File.ReadAllLines("script.fdscript");
            string xmlText = string.Join("\n", xmlLines);
            
            Assert.IsTrue(xmlLines[0].StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));

            Assert.IsTrue(xmlText.Contains("<fdscript"));
            Assert.IsTrue(xmlText.Contains("<fdscriptheader"));
            Assert.IsTrue(xmlText.Contains("<cmdopen"));
            Assert.IsTrue(xmlText.Contains("<cmduser"));
            Assert.IsTrue(xmlText.Contains("<cmdcalculation"));
            Assert.IsTrue(xmlText.Contains("<cmdlistgen"));
        }

        [TestMethod("Validate schema")]
        public void ValidateSchema()
        {
            // Serialize
            var script = new FdScript2(
                "logfile.log",
                new CmdOpen("model.struxml"),
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation(Analysis.StaticAnalysis()),
                new CmdListGen("a.bsc", "./")
                );
            script.Serialize("script.fdscript");

            // Validate
            XmlDocument asset = new XmlDocument();
            XmlTextReader schemaReader = new XmlTextReader(@"fdscript.xsd");
            XmlSchema schema = XmlSchema.Read(schemaReader, (obj, e) =>
            {
                Assert.Fail(e.Message);
            });
            asset.Schemas.Add(schema);

            asset.Load("script.fdscript");

            asset.Validate((obj, e) => {
                Assert.Fail(e.Message);
            });
        }
    }
}
