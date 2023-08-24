using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Composites
{
    [TestClass]
    public class CompositeBeamTest
    {
        [TestMethod]
        public void SerializeComposite()
        {
            // Load material and sections from .struxml files
            var materialsDB = Materials.MaterialDatabase.DeserializeStruxml(@"C:\Repos\femdesign-api\FemDesign.Tests\bin\Debug\Composites\materials.struxml");
            var steel = materialsDB.MaterialByName("S 275");
            var concrete = materialsDB.MaterialByName("C25/30");
            var matList = new List<Materials.Material>() { steel, concrete };

            var sectionsDB = Sections.SectionDatabase.DeserializeStruxml(@"C:\Repos\femdesign-api\FemDesign.Tests\bin\Debug\Composites\sections.struxml");
            var steelSection = sectionsDB.SectionByName("Steel sections, HE-B, 300");
            var concreteSection = sectionsDB.SectionByName("Unnamed, Concrete, 3");
            var secList = new List<Sections.Section>() { steelSection, concreteSection };


            // Create composite object
            var composite = new Composites();

            // Create composite section
            string name = "TestColumnA1";
            double cy = 80;     //mm
            double cz = 80;     //mm
            CompositeSection compositeSection = CompositeSection.ColumnA(matList, secList, name, cy, cz);
            composite.CompositeSection = new List<CompositeSection>() { compositeSection };

            // Create complex composite
            ComplexComposite complexComposite = new ComplexComposite(compositeSection);
            composite.ComplexComposite = new List<ComplexComposite>() { complexComposite };


            // Serialize struxml
            string fileName = "CompositeSerialization";
            string filePath = @"D:\Andi\API_Work\802_CompositeSections\tests\CSharp\" + fileName + ".struxml";
            this.SerializeComposite(filePath, composite);

            //Deserialize struxml
            Composites inData = DeserializeComposite(filePath);

            // Compare data
            Assert.AreEqual(composite, inData);
        }

        public void SerializeComposite(string filePath, Composites composite)
        {
            // Check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Serialization failed.");
            }

            // Serialize
            XmlSerializer serializer = new XmlSerializer(typeof(Composites));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, composite);
                writer.Close();
            }
        }
        public static Composites DeserializeComposite(string filePath)
        {
            // Check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Serialization failed.");
            }

            XmlSerializer deserializer = new XmlSerializer(typeof(Composites));
            TextReader reader = new StreamReader(filePath);
            var obj = deserializer.Deserialize(reader);
            Composites composite= (Composites)obj;
            reader.Close();
            return composite;
        }
    }
}
