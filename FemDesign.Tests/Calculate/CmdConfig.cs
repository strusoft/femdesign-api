using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.Calculate;

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

            var fdscript = new Calculate.FdScript(@"C:\temp\fdscript_test.xml", config);
            var path = @"C:\temp\fdscript_test.xml";
            fdscript.Serialize(path );


        }

    }
}
