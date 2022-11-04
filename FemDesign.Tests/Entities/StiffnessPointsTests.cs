using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Supports;

namespace FemDesign.Entities
{
    [TestClass()]
    public class StiffnessPointsTests
    {
        [TestMethod("Create")]
        public void Create()
        {
            var rectangle = FemDesign.Geometry.Region.RectangleXY(Geometry.Point3d.Origin, 5, 5);
            var surface = new SurfaceSupport(rectangle, Releases.Motions.RigidPoint());
            var point = new FemDesign.Geometry.Point3d(5, 0.123, -0.39);
            var stiffnessPoint = new FemDesign.Supports.StiffnessPoint(surface, point, Releases.Motions.RigidPoint(), new Releases.MotionsPlasticLimits(10,10,10,10,10,10));

            var objText = SerializeToString(stiffnessPoint);
            Console.Write(objText);
        }

        public static string SerializeToString(StiffnessPoint stiffPoint)
        {
            // serialize
            XmlSerializer serializer = new XmlSerializer(typeof(StiffnessPoint));
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, stiffPoint);
                return writer.ToString();
            }
        }
    }
}
