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
using System.Collections;

namespace FemDesign.Composites
{
    [TestClass]
    public class CompositeBeamTest
    {
        [TestMethod]
        public void CompositeBeamB()
        {
            // Geometry
            Point3d firstPt = new Point3d(0, 0, 0);
            Point3d secondPt = new Point3d(10, 0, 0);
            Edge line = new Edge(firstPt, secondPt);


            var materialsDB = Materials.MaterialDatabase.DeserializeStruxml(@"C:\Repository\femdesign-api\FemDesign.Tests\Composites\materials.struxml");
            var steel = materialsDB.MaterialByName("S 275");
            var concrete = materialsDB.MaterialByName("C25/30");
            

            // Create composite beam
            CompositeSection compositeSection = CompositeSection.BeamB(steel: steel, concrete: concrete, name: "beamB1", b: 200, bt: 700, o1: 400, o2: 150, h: 360, tw: 10, tfb: 50, tft: 20);
            Bars.Bar compositeBar = new Bars.Bar(line, Bars.BarType.Beam, compositeSection, null, null, "B");
            

            // Create output and input model
            Model modelIn = new Model(Country.S);
            Model modelOut = new Model(Country.S);
            modelOut.AddElements(compositeBar);

            using (var femDesign = new FemDesignConnection(
                fdInstallationDir: @"C:\Program Files\StruSoft\FEM-Design 22\", 
                outputDir: @"D:\Andi\API_Work\Github\802_CompositeSections\tests\CSharp\CompositeBeamB_out", 
                keepOpen: false))
            {
                femDesign.Open(modelOut, false);
                modelIn = femDesign.GetModel();
            }

            
            // check Composite obj.
            var compositesIn = modelIn.Composites;
            Assert.IsNotNull(compositesIn);
            
            var compositeSectionsOut = modelOut.Composites.CompositeSection.OrderBy(c => c.Guid).ToList();
            var compositeSectionsIn = modelIn.Composites.CompositeSection.OrderBy(c => c.Guid).ToList();
            Assert.IsNotNull(compositeSectionsIn);
            Assert.AreEqual(compositeSectionsIn.Count, compositeSectionsOut.Count);
            for (int i = 0; i < compositeSectionsIn.Count; i++)
            {
                var partsIn = compositeSectionsIn[i].Parts;
                var partsOut = compositeSectionsOut[i].Parts;
                Assert.AreEqual(partsIn.Count, partsOut.Count);

                var partsMaterialIn = partsIn.Select(p => p.Material.Family).ToList();
                var partsMaterialOut = partsOut.Select(p => p.Material.Family).ToList();
                Assert.AreEqual(partsMaterialIn.Count, partsMaterialOut.Count);
                for(int j = 0;  j < partsMaterialIn.Count; j++)
                {
                    Assert.AreEqual(partsMaterialIn[j], partsMaterialOut[j]);
                }

                var paramsIn = compositeSectionsIn[i].ParameterList;
                var paramsOut = compositeSectionsOut[i].ParameterList;
                Assert.AreEqual(paramsIn.Count, paramsOut.Count);
                for (int j = 0; j < paramsIn.Count; j++)
                {
                    Assert.AreEqual(paramsIn[j].Value, paramsOut[j].Value);
                }
            }

            var complexCompositesOut = modelOut.Composites.ComplexComposite;
            var complexCompositesIn = modelIn.Composites.ComplexComposite;
            Assert.IsNotNull(complexCompositesIn);
            Assert.AreEqual(complexCompositesOut.Count, complexCompositesIn.Count);
        }
        
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
            CompositeSection compositeSection = CompositeSection.ColumnA(steel, concrete, steelSection, name, cy, cz);

            composite.CompositeSection = new List<CompositeSection>() { compositeSection };

            //composite.CompositeSection[0].ParameterDictionary[CompositeParameterType.];
            //var sth = GetCompositeSectionParameters(compositeSection);


            // Create complex composite
            ComplexComposite complexComposite = new ComplexComposite(compositeSection);
            composite.ComplexComposite = new List<ComplexComposite>() { complexComposite };


            // Serialize struxml
            string fileName = "CompositeSerialization";
            string filePath = @"D:\Andi\API_Work\0_Github\802_CompositeSections\tests\CSharp\" + fileName + ".struxml";
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
