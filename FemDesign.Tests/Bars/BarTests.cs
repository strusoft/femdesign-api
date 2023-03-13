using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Sections;
using FemDesign.Materials;

namespace FemDesign.Bars
{
    [TestClass()]
    public class BarTests
    {
        [TestInitialize]
        public void ResetInstanceCounters()
        {
            // Reset all instance counters before each tests
            PrivateType barPartType = new PrivateType(typeof(BarPart));
            barPartType.SetStaticFieldOrProperty("_barInstance", 0);
            barPartType.SetStaticFieldOrProperty("_columnInstance", 0);
            barPartType.SetStaticFieldOrProperty("_trussInstance", 0);
        }

        [TestMethod("Bar constructor 1")]
        public void BarConstructorTest1()
        {
            var edge = new Geometry.LineEdge(new Geometry.Point3d(0, 0, 0), new Geometry.Point3d(1, 0, 0));
            Bar bar = new Bar(edge, new Material(), GetTestSection(), "Truss");

            Assert.AreEqual("Truss", bar.Identifier);
            Assert.AreEqual(BarType.Truss, bar.Type);
            Assert.AreEqual(1, bar.Instance);

            Bar bar2 = new Bar(edge, new Material(), GetTestSection(), "Truss");
            Assert.AreEqual(2, bar2.Instance);
        }

        [TestMethod("Name, Identifier etc.")]
        public void BarTest()
        {
            var bar = GetTestBar();
            bar.Identifier = "TestName";

            Assert.AreEqual("TestName", bar.Identifier);
            Assert.AreEqual("TestName.2", bar.Name);
            Assert.AreEqual(2, bar.Instance);
        }

        [TestMethod("Name, Identifier etc. (BarPart)")]
        public void BarPartTest()
        {
            var bar = GetTestBar();
            bar.Identifier = "BP";

            Assert.AreEqual("BP", bar.Identifier);
            Assert.AreEqual("BP.2", bar.Name);
            Assert.AreEqual(2, bar.Instance);

            Assert.AreEqual("BP", bar.BarPart.Identifier);
            Assert.AreEqual("BP.2.1", bar.BarPart.Name);
            Assert.AreEqual(2, bar.BarPart.Instance);
        }

        [TestMethod("Identifier 1")]
        public void BarTest3()
        {
            var bar = GetTestBar();
            bar.Identifier = "BeamOfLight";
            bar.Identifier = bar.Identifier;
            bar.Identifier = bar.Identifier;
            bar.Identifier = bar.Identifier;
            bar.Identifier = bar.Identifier;
            bar.Identifier = bar.Identifier;

            Assert.AreEqual("BeamOfLight", bar.Identifier);
        }

        [TestMethod("Identifier 2")]
        public void BarTest4()
        {
            var bar = GetTestBar();
            bar.Identifier = "Repeat";
            bar.Identifier = "Repeat";
            bar.Identifier = "Repeat";
            bar.Identifier = "Repeat";
            bar.Identifier = "Repeat";

            Assert.AreEqual("Repeat", bar.Identifier);
        }

        [TestMethod("Identifier validation")]
        public void BarTest5()
        {
            var bar = GetTestBar();
            
            bar.Identifier = "Valid";
            Assert.AreEqual("Valid", bar.Identifier);
            
            bar.Identifier = "Valid_1";
            Assert.AreEqual("Valid_1", bar.Identifier);

            bar.Identifier = "Valid.1.2.3";
            Assert.AreEqual("Valid.1.2.3", bar.Identifier);

            bar.Identifier = "Valid_åäöÅÄÖ~!£\"%^*()";
            Assert.AreEqual("Valid_åäöÅÄÖ~!£\"%^*()", bar.Identifier);

            Assert.ThrowsException<ArgumentException>(() => bar.Identifier = null);
            Assert.ThrowsException<ArgumentException>(() => bar.Identifier = "");
            Assert.ThrowsException<ArgumentException>(() => bar.Identifier = "invalid char >");
            Assert.ThrowsException<ArgumentException>(() => bar.Identifier = "invalid char &");
            Assert.ThrowsException<ArgumentException>(() => bar.Identifier = "invalid char $");
        }

        [TestMethod("LockedIdentifier 1")]
        public void LockedIdentifierTest()
        {
            var bar = GetTestBar();
            bar.LockedIdentifier = true;

            Assert.IsTrue(bar.LockedIdentifier);
            Assert.IsFalse(bar.Name.StartsWith("@"));

            Assert.IsTrue(bar.BarPart.LockedIdentifier);
            Assert.IsFalse(bar.BarPart.Name.StartsWith("@"));
        }

        [TestMethod("LockedIdentifier 2")]
        public void LockedIdentifierTest2()
        {
            Bar bar = GetTestBar();
            bar.LockedIdentifier = false;

            Assert.IsFalse(bar.LockedIdentifier);
            Assert.IsFalse(bar.Name.StartsWith("@"));

            Assert.IsFalse(bar.BarPart.LockedIdentifier);
            Assert.IsFalse(bar.BarPart.Name.StartsWith("@"));
        }


        [TestMethod("UpdateSection")]
        [TestCategory("FEM-Design required")]
        public void UpdateSection()
        {

            // Define geometry
            Model model = FemDesign.Model.DeserializeFromFilePath("Bars//myModel.struxml");

            var beam = model.Entities.Bars.Where(x => x.BarPart.HasComplexSectionRef && !x.BarPart.HasDeltaBeamComplexSectionRef).ToList().First();
            var truss = model.Entities.Bars.Where(x => x.Type == BarType.Truss).ToList().First();
            var deltaBeam = model.Entities.Bars.Where(x => x.BarPart.HasDeltaBeamComplexSectionRef).ToList().First();
            var compositeBeam = model.Entities.Bars.Where(x => x.BarPart.HasComplexCompositeRef).ToList().First();

            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("Bars//materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("Bars//sections.struxml");

            var section1 = sectionsDB.SectionByName("Concrete sections, Rectangle, 120x150");
            var section2 = sectionsDB.SectionByName("Concrete sections, Rectangle, 200x550");
            var steelSection = sectionsDB.SectionByName("Steel sections, IPE, 80");


            beam.UpdateSection(new Section[] { section1, section2 });
            truss.UpdateSection(steelSection);

            Assert.ThrowsException<NotImplementedException>(() => deltaBeam.UpdateSection(new Section[] { section1, section2 }));
            Assert.ThrowsException<NotImplementedException>(() => compositeBeam.UpdateSection(new Section[] { section1, section2 }));
        }

        [TestMethod("UpdateMaterial")]
        [TestCategory("FEM-Design required")]
        public void UpdateMaterial()
        {
            // Define geometry
            Model model = FemDesign.Model.DeserializeFromFilePath("Bars//myModel.struxml");

            var beam = model.Entities.Bars.Where(x => x.BarPart.HasComplexSectionRef && !x.BarPart.HasDeltaBeamComplexSectionRef).ToList().First();
            var truss = model.Entities.Bars.Where(x => x.Type == BarType.Truss).ToList().First();
            var deltaBeam = model.Entities.Bars.Where(x => x.BarPart.HasDeltaBeamComplexSectionRef).ToList().First();
            var compositeBeam = model.Entities.Bars.Where(x => x.BarPart.HasComplexCompositeRef).ToList().First();

            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("Bars//materials.struxml");

            var c1620 = materialsDB.MaterialByName("C16/20");
            var s235 = materialsDB.MaterialByName("S 235");

            Console.WriteLine(beam.BarPart.ComplexMaterialObj.Name);
            beam.UpdateMaterial(c1620);
            Console.WriteLine(beam.BarPart.ComplexMaterialObj.Name);

            Console.WriteLine(truss.BarPart.ComplexMaterialObj.Name);
            truss.UpdateMaterial(s235);
            Console.WriteLine(truss.BarPart.ComplexMaterialObj.Name);

            Assert.IsTrue(beam.BarPart.ComplexMaterialObj.Equals(c1620));
            Assert.IsTrue(truss.BarPart.ComplexMaterialObj.Equals(s235));

            Assert.ThrowsException<NotImplementedException>(() => deltaBeam.UpdateMaterial(c1620));
            Assert.ThrowsException<NotImplementedException>(() => compositeBeam.UpdateMaterial(c1620));
        }



        private static Bar GetTestBar()
        {
            var edge = new Geometry.LineEdge(new Geometry.Point3d(0, 0, 0), new Geometry.Point3d(1, 0, 0));
            Section section = GetTestSection();
            Material material = new Material();

            return new Beam(edge, material, section, identifier: "Test");
        }

        private static Section GetTestSection()
        {
            return new FemDesign.Sections.Section(
                            new Geometry.RegionGroup(
                                new Geometry.Region(
                                    new List<Geometry.Contour> {
                            new Geometry.Contour(
                                new List<Geometry.Edge> {
                                    new Geometry.CircleEdge(
                                        0.010,
                                        Geometry.Point3d.Origin,
                                        Geometry.Plane.XY
                                        )
                                }
                                )
                                    }
                                    )
                                ),
                            "custom",
                            Materials.MaterialTypeEnum.Undefined,
                            "TestGroup",
                            "TestType",
                            "TestSize"
                            );
        }
    }
}