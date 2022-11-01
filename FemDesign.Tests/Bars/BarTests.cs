using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Sections;
using FemDesign.Materials;

namespace FemDesign.Bars.Tests
{
    [TestClass()]
    public class BarTests
    {
        [TestMethod("Name, Identifier etc.")]
        public void BarTest()
        {
            var bar = GetTestBar();
            bar.Identifier = "TestName";
            
            Assert.AreEqual("TestName", bar.Identifier);
            Assert.AreEqual("TestName.1", bar.Name);
            Assert.AreEqual(1, bar.Instance);
        }

        [TestMethod("Name, Identifier etc. (BarPart)")]
        public void BarPartTest()
        {
            var bar = GetTestBar();
            bar.Identifier = "B";

            Assert.AreEqual("B", bar.Identifier);
            Assert.AreEqual("B.1", bar.Name);
            Assert.AreEqual(1, bar.Instance);

            Assert.AreEqual("B", bar.BarPart.Identifier, "Setting the identifier on Bar should also set on BarPart");
            Assert.AreEqual("B.1.1", bar.BarPart.Name);
            Assert.AreEqual(1, bar.BarPart.Instance);
        }

        [TestMethod("Name")]
        public void BarTest2()
        {
            var bar = GetTestBar();
            bar.Identifier = "AlexBeam";

            bar.Name = bar.Name;
            bar.Name = bar.Name;
            bar.Name = bar.Name;
            bar.Name = bar.Name;
            bar.Name = bar.Name;
            Assert.AreEqual("AlexBeam.1", bar.Name);
        }

        [TestMethod("Identifier")]
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

        [TestMethod("LockedIdentifier 1")]
        public void LockedIdentifierTest()
        {
            var bar = GetTestBar();
            bar.LockedIdentifier = true;

            Assert.IsTrue(bar.LockedIdentifier);
            Assert.IsTrue(bar._name.StartsWith("@"));
            Assert.IsFalse(bar.Name.StartsWith("@"));
        }

        [TestMethod("LockedIdentifier 2")]
        public void LockedIdentifierTest2()
        {
            Bar bar = GetTestBar();
            bar.LockedIdentifier = false;

            Assert.IsFalse(bar.LockedIdentifier);
            Assert.IsFalse(bar._name.StartsWith("@"));
            Assert.IsFalse(bar.Name.StartsWith("@"));
        }

        private static Bar GetTestBar()
        {
            var edge = new Geometry.LineEdge(new Geometry.Point3d(0, 0, 0), new Geometry.Point3d(1, 0, 0));
            Section section = GetTestSection();
            Material material = new Material();

            return new Bar(edge, material, section, "Test");
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