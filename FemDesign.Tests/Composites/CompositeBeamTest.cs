using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

using FemDesign;
using FemDesign.Materials;
using FemDesign.Sections;
using FemDesign.Geometry;

namespace FemDesign.Composites
{
    [TestClass]
    public class CompositeBeamTest
    {
        [TestMethod]
        public void CompositeColumnA()
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

            //composite.CompositeSection[0].ParameterDictionary[CompositeParameterType.];
            //var sth = GetCompositeSectionParameters(compositeSection);


            // Create complex composite
            ComplexComposite complexComposite = new ComplexComposite(compositeSection);
            composite.ComplexComposite = new List<ComplexComposite>() { complexComposite };


            // Serialize struxml
            string fileName = "CompositeSerialization";
            string filePath = @"D:\Andi\API_Work\Github\802_CompositeSections\tests\CSharp\" + fileName + ".struxml";
            this.SerializeComposite(filePath, composite);

            //Deserialize struxml
            Composites inData = DeserializeComposite(filePath);

            // Compare data
            Assert.AreEqual(composite, inData);
        }

        [TestMethod]
        public void CompositeBeamB()
        {
            // Geometry
            Point3d firstPt = new Point3d(0, 0, 0);
            Point3d secondPt = new Point3d(10, 0, 0);
            Edge line = new Edge(firstPt, secondPt);


            var materialsDB = Materials.MaterialDatabase.DeserializeStruxml(@"C:\Repos\femdesign-api\FemDesign.Tests\Composites\materials.struxml");
            var steel = materialsDB.MaterialByName("S 275");
            var concrete = materialsDB.MaterialByName("C25/30");
            

            // Create composite beam
            CompositeSection compositeSection = CompositeSection.BeamB(steel, concrete, "beamB1", 200, 700, 400, 150, 360, 10, 50, 20);
            Bars.Bar compositeBar = new Bars.Bar(line, Bars.BarType.Beam, compositeSection, null, null, "B");
            
            
            // Create model
            Model model = new Model(Country.S);
            model.AddElements(compositeBar);

            using (var femDesign = new FemDesignConnection(
                fdInstallationDir: @"C:\Program Files\StruSoft\FEM-Design 22\", 
                outputDir: @"D:\Andi\API_Work\Github\802_CompositeSections\CodeTest\Output", 
                keepOpen: false))
            {
                femDesign.Open(model, false);
            }

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
        public void SerializeSection(string filePath, ModelSections sections)
        {
            // Check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Serialization failed.");
            }

            // Serialize
            XmlSerializer serializer = new XmlSerializer(typeof(ModelSections));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, sections);
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
