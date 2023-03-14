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
    }
}