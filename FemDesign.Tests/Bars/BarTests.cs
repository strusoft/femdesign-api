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
                                        Geometry.CoordinateSystem.Global()
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