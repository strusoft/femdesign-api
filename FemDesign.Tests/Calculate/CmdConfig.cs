using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.Calculate;
using System.Xml.Serialization;
using System.IO;

namespace FemDesign.Calculate
{
    [TestClass()]
    public partial class CmdConfigTest
    {
        [TestMethod("CreateConfig")]
        public void CreateConfig()
        {

            var cmsConfig = new Calculate.CcmsConfig(true, 100);
            var coCoConfig = new Calculate.CcCoConfig();

            var ecst = new EcstConfig(true);
            var config = new FemDesign.Calculate.CmdConfig(cmsConfig, coCoConfig, ecst);
        }

        [TestMethod("DesignParameter")]
        public void DesignParameter()
        {

            var databse = Sections.SectionDatabase.GetDefault();

            var mySection = databse.Sections.Section[0];
            var mySection1 = databse.Sections.Section[1];

            var sections = new List<Sections.Section> { mySection, mySection1};

            var cmsConfig = new Calculate.DesParamBarSteel(0.90, sections);

            var config = new FemDesign.Calculate.CmdConfig(cmsConfig);

            var serializer = new XmlSerializer(typeof(CmdConfig));
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, config);

            string serializedPerson = stringWriter.ToString();
            Console.WriteLine(serializedPerson);
        }
    }
}
