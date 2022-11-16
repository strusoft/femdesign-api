using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using FemDesign.Calculate;

namespace FemDesign.Calculate
{
    [TestClass()]
    public class FdScriptTests
    {
        [TestMethod("CreateFdScript")]
        public void CreateFdScript()
        {
            var logfile = @"C:\temp\logfile.log";
            var filePath = @"C:\temp\tempFile.struxml";
            var script = new FdScript2(logfile, new CmdOpen(filePath));
            script.Serialize("script.fdscript");
        }
    }
}


