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
    public class SoilElementsTests
    {
        [TestMethod]
        public void CreateBoreholes()
        {
            var boreHoles = GetBoreholes();

            boreHoles.ForEach(boreHole => Console.WriteLine(SerializeToString(boreHole)) );
        }


        [TestMethod]
        [TestCategory("FEM-Design required")]
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

        [TestMethod]
        public void CreateWater()
        {
            var water = new FemDesign.Soil.GroundWater("Water_1");

            Assert.IsNotNull(water.Color);
            Assert.IsNotNull(water.Name);

            Assert.IsTrue(water.Name == "Water_1");
        }

        [TestMethod]
        [TestCategory("FEM-Design required")]
        public void CreateStrata()
        {

            var strata = GetStrata();


            Assert.IsNotNull(strata);
            Assert.IsTrue(strata.Stratum.Count == 3);
            Assert.IsTrue(strata.GroundWater.Count == 2);
            Assert.IsTrue(strata.Name == "SOIL.1");
            Assert.IsTrue(strata.DepthLevelLimit == -25.0);

            Console.WriteLine(SerializeToString(strata));
        }

        [TestMethod]
        [TestCategory("FEM-Design required")]
        public void CreateModelWithSoil()
        {

            var strata = GetStrata();
            var borehole = GetBoreholes();
            var boreholes = borehole.Concat(OuterBoreholes()).ToList();

            var soilElements = new FemDesign.Soil.SoilElements(strata, boreholes);

            var model = new Model(Country.S, soil: soilElements);
            model.Open();

            Console.WriteLine(model.SerializeToString());
        }


        [TestMethod]
        [TestCategory("FEM-Design required")]
        public void CreateSoilElement()
        {
            var strata = GetStrata();
            var boreholes = GetBoreholes();

            var soilElements = new FemDesign.Soil.SoilElements(strata, boreholes);


            Assert.IsNotNull(soilElements.Strata);
            Assert.IsTrue(soilElements.BoreHoles.Count == 4);
            Console.WriteLine( SerializeToString(soilElements) );
        }

        private List<BoreHole> GetBoreholes()
        {
            var boreholes = new List<BoreHole>();

            var boreHole = new FemDesign.Soil.BoreHole(2, 2, 1);
            boreholes.Add(boreHole);

            var strata = new List<double> { 0.00, -10.00, -20.00 };
            var water = new List<double> { -12, -22.00 };

            var levels = new FemDesign.Soil.AllLevels(strata, water);
            boreHole = new FemDesign.Soil.BoreHole(4, 4, 0, levels);
            boreholes.Add(boreHole);


            levels = new FemDesign.Soil.AllLevels(strata, water);
            boreHole = new FemDesign.Soil.BoreHole(1, 8, 0, levels);
            boreholes.Add(boreHole);

            boreHole = new FemDesign.Soil.BoreHole(8, 8, 1);
            boreholes.Add(boreHole);

            return boreholes;
        }

        private List<BoreHole> OuterBoreholes()
        {
            var boreholes = new List<BoreHole>();

 
            var strata = new List<double> { 0.00, -10.00, -20.00 };
            var water = new List<double> { -12, -22.00 };
            var levels = new FemDesign.Soil.AllLevels(strata, water);

            var boreHole = new FemDesign.Soil.BoreHole(0, 0, 0, levels);
            boreholes.Add(boreHole);
            boreHole = new FemDesign.Soil.BoreHole(10, 0, 0, levels);
            boreholes.Add(boreHole);
            boreHole = new FemDesign.Soil.BoreHole(0, 10, 0, levels);
            boreholes.Add(boreHole);
            boreHole = new FemDesign.Soil.BoreHole(10, 10, 0, levels);
            boreholes.Add(boreHole);

            return boreholes;
        }

        private Strata GetStrata()
        {
            var database = FemDesign.Materials.MaterialDatabase.DeserializeStruxml(@"Soil\SoilDatabase.struxml");
            var soilMaterials = database.GetSoilMaterial();
            var soil1 = soilMaterials[0];
            var soil2 = soilMaterials[1];
            var soil3 = soilMaterials[2];

            var stratum1 = new FemDesign.Soil.Stratum(soil1);
            var stratum2 = new FemDesign.Soil.Stratum(soil2);
            var stratum3 = new FemDesign.Soil.Stratum(soil3);

            var stratums = new List<Stratum> { stratum1, stratum2, stratum3 };

            var water1 = new GroundWater("Water_1");
            var water2 = new GroundWater("Water_2");

            var waterLevels = new List<GroundWater> { water1, water2 };

            var contour = new List<Geometry.Point2d>();
            contour.Add( new Geometry.Point2d(0,0) );
            contour.Add( new Geometry.Point2d(10,0) );
            contour.Add( new Geometry.Point2d(10,10) );
            contour.Add( new Geometry.Point2d(0,10) );

            var strata = new FemDesign.Soil.Strata(stratums, waterLevels, contour, -25.0);

            return strata;
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
        public static string SerializeToString(SoilElements obj)
        {
            // serialize
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }
        private static string SerializeToString(Strata obj)
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