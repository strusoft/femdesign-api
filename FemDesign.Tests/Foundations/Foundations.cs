using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FemDesign;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
namespace FemDesign.Foundations
{
    [TestClass]
    public class FoundationsTests
    {
        [TestMethod]
        public void CreateBoreholes()
        {
            var boreHole = new FemDesign.Foundations.BoreHole(10, 10, 0);

            var strata = new List<double> { 0.00, -10.00, -20.00 };
            var water = new List<double> { 0.00, -10.00, -20.00 };
            Console.WriteLine( SerializeToString(boreHole) );

            var levels = new FemDesign.Foundations.AllLevels(strata, water);
            boreHole = new FemDesign.Foundations.BoreHole(10, 10, 0, levels);
            Console.WriteLine(SerializeToString(boreHole));


            levels = new FemDesign.Foundations.AllLevels(strata, null);
            boreHole = new FemDesign.Foundations.BoreHole(10, 10, 0, levels);
            Console.WriteLine(SerializeToString(boreHole));

            boreHole = new FemDesign.Foundations.BoreHole(10, 10, 0);
            Console.WriteLine(SerializeToString(boreHole));
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