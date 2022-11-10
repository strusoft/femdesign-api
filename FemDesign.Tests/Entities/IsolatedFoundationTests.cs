using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Foundations;

namespace FemDesign.Entities
{
    [TestClass()]
    public class IsolatedFoundationTests
    {
        [TestMethod("Isolated constructor 1")]
        public void FoundationConstructorTest1()
        {
            var foundation = GetFoundationTest();

            Assert.AreEqual("F", foundation.Identifier);
            Assert.AreEqual(foundation.FoundationSystem, FoundationSystem.Simple);
            Assert.AreEqual(1, foundation.Instance);

            var foundation2 = GetFoundationTest();
            Assert.AreEqual(2, foundation2.Instance);
        }

        private static IsolatedFoundation GetFoundationTest()
        {
            var rectangle = FemDesign.Geometry.Region.RectangleXY(Geometry.Point3d.Origin, 5, 5);
            var point = new FemDesign.Geometry.Point3d(5, 0.123, -0.39);

            var extrudedSolid = new Foundations.ExtrudedSolid(0.3, rectangle);
            var materials = FemDesign.Materials.MaterialDatabase.DeserializeStruxml(@"Entities\materials.struxml");
            var material = materials.ByType().concrete[0];
            var isolatedFoundation = new FemDesign.Foundations.IsolatedFoundation(extrudedSolid, 3000, material, point);

            return isolatedFoundation;
        }


        public static string SerializeToString(IsolatedFoundation isolatedFoundation)
        {
            // serialize
            XmlSerializer serializer = new XmlSerializer(typeof(IsolatedFoundation));
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, isolatedFoundation);
                return writer.ToString();
            }
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Open a Model")]
        public void Open()
        {
            Model model = new Model(Country.S);

            var rectangle = FemDesign.Geometry.Region.RectangleXY(Geometry.Point3d.Origin, 5, 5);
            var point = new FemDesign.Geometry.Point3d(1, 1, 0);

            var extrudedSolid = new Foundations.ExtrudedSolid(0.3, rectangle, false);
            var materials = FemDesign.Materials.MaterialDatabase.GetDefault();
            var material = materials.ByType().concrete[0];
            var isolatedFoundation = new FemDesign.Foundations.IsolatedFoundation(extrudedSolid, 3000, material, point);

            var elements = new List<GenericClasses.IStructureElement>() { isolatedFoundation };

            model.AddElements(elements);
            Console.WriteLine( model.SerializeToString() );
            model.Open();
        }

    }
}
