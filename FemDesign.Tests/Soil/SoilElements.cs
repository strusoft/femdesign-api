using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FemDesign;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace FemDesign.Soil
{
    [TestClass]
    public class FoundationsTests
    {
        [TestMethod]
        public void CreateBoreholes()
        {
            var boreHole = new FemDesign.Soil.BoreHole(10, 10, 0);

            var strata = new List<double> { 0.00, -10.00, -20.00 };
            var water = new List<double> { 0.00, -10.00, -20.00 };
            Console.WriteLine( SerializeToString(boreHole) );

            var levels = new FemDesign.Soil.AllLevels(strata, water);
            boreHole = new FemDesign.Soil.BoreHole(10, 10, 0, levels);
            Console.WriteLine(SerializeToString(boreHole));


            levels = new FemDesign.Soil.AllLevels(strata, null);
            boreHole = new FemDesign.Soil.BoreHole(10, 10, 0, levels);
            Console.WriteLine(SerializeToString(boreHole));

            boreHole = new FemDesign.Soil.BoreHole(10, 10, 0);
            Console.WriteLine(SerializeToString(boreHole));
        }


        [TestMethod]
        public void CreateStratum()
        {
            var database = FemDesign.Materials.MaterialDatabase.DeserializeStruxml(@"Soil\SoilDatabase.struxml");
            var soilMaterials = database.GetSoilMaterial();
            var soil = soilMaterials[0];
            var stratum = new FemDesign.Soil.Stratum(soil);

            Assert.IsNotNull(stratum.Color);
            Assert.IsNotNull(stratum.Guid);

            Assert.IsTrue(stratum.Guid == soil.Guid);
        }



        public static string SerializeToString(BoreHole obj)
        {
            // serialize
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }
    }
}