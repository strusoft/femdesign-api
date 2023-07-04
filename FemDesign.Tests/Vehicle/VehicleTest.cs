using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.LibraryItems;

using StruSoft.Interop.StruXml.Data;
namespace FemDesign.Vehicle
{
    [TestClass()]
    public class VehicleTests
    {
        [TestMethod("DeserialiseVehicle")]
        public void DeserialiseResources()
        {
            var vehicles = VehicleDatabase.DeserializeFromResource();
            Assert.IsTrue( vehicles.Count == 18 );
        }
    }
}
