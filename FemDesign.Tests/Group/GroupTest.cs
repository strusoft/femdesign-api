using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using Microsoft.XmlDiffPatch;

using FemDesign.GenericClasses;

namespace FemDesign.Groups
{
    [TestClass]
    public class GroupTest
    {
        [TestMethod]
        public void OpenModelWithGroup()
        {
            var filePath = @"C:\Users\MarcoPellegrinoKonsu\OneDrive - StruSoft AB\FemDesign_API_Development\21.4.0\463-RigidityGroup\supportGroup.struxml";
            var model = FemDesign.Model.DeserializeFromFilePath(filePath);
            model.Open(@"C:\Users\MarcoPellegrinoKonsu\OneDrive - StruSoft AB\FemDesign_API_Development\21.4.0\463-RigidityGroup\supportGroupOUT.struxml");
        }
    }
}
