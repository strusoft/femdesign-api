using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.FiniteElements;
using FemDesign;

namespace FemDesign.Entities
{
    [TestClass()]
    public class PeakSmoothingRegionTests
    {
        [TestMethod("Create")]
        public void Create()
        {
            var rectangle = FemDesign.Geometry.Region.RectangleXY(Geometry.Point3d.Origin, 5, 5);
            var peakSmothingRegion = new PeakSmoothingRegion(rectangle);

            var objText = SerializeToString(peakSmothingRegion);
            Console.Write(objText);
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Open in Model")]
        public void OpenInModel()
        {
            var rectangle1 = FemDesign.Geometry.Region.RectangleXY(Geometry.Point3d.Origin, 5, 5);
            var rectangle2 = FemDesign.Geometry.Region.RectangleXY(new Geometry.Point3d(10, 0, 0), 5, 5);
            
            var peakSmothingRegion1 = new PeakSmoothingRegion(rectangle1);
            var peakSmothingRegion2 = new PeakSmoothingRegion(rectangle2, true);
            List<PeakSmoothingRegion> psrList = new List<PeakSmoothingRegion>() {peakSmothingRegion1, peakSmothingRegion2};
            
            List<GenericClasses.IStructureElement> elements = new List<GenericClasses.IStructureElement>();
            elements.AddRange(psrList);

            FemDesign.Model model = new FemDesign.Model(Country.S, elements);
            using (var connection = new FemDesign.FemDesignConnection())
            {
                connection.Open(model, true);
            }
        }

        public static string SerializeToString(PeakSmoothingRegion peakSmothingRegion)
        {
            // serialize
            XmlSerializer serializer = new XmlSerializer(typeof(PeakSmoothingRegion));
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, peakSmothingRegion);
                return writer.ToString();
            }
        }
    }
}
