using FemDesign.Bars;
using FemDesign.Materials;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FemDesign.Sections;
using FemDesign.GenericClasses;
using System.Linq;

namespace FemDesign.ModellingTools
{
    [TestClass]
    public class ModellingToolsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var bar = GetTestBar();
            var beam = GetTestBeam();
            var column= GetTestColumn();
            var truss= GetTestTruss();

            var listObjects = new List<Bar>() { bar, beam, column, truss };
            var cover = CoverReferenceList.FromObjectList(listObjects.ToList<object>());
            var coverReferenceList = new CoverReferenceList(listObjects.Select(x => x.BarPart.Guid).ToList());

            Assert.IsTrue(cover.RefGuid[0].Guid == coverReferenceList.RefGuid[0].Guid);
            Assert.IsTrue(cover.RefGuid[1].Guid == coverReferenceList.RefGuid[1].Guid);
            Assert.IsTrue(cover.RefGuid[2].Guid == coverReferenceList.RefGuid[2].Guid);
            Assert.IsTrue(cover.RefGuid[3].Guid == coverReferenceList.RefGuid[3].Guid);

        }

        private static Truss GetTestTruss()
        {
            var edge = new Geometry.LineEdge(new Geometry.Point3d(0, 0, 0), new Geometry.Point3d(1, 0, 0));
            Section section = GetTestSection();
            Material material = new Material();

            return new Truss(edge, material, section, identifier: "Test");
        }

        private static Bar GetTestBar()
        {
            var edge = new Geometry.LineEdge(new Geometry.Point3d(0, 0, 0), new Geometry.Point3d(1, 0, 0));
            Section section = GetTestSection();
            Material material = new Material();

            return new Bar(edge, material, section, identifier: "Test");
        }

        private static Beam GetTestBeam()
        {
            var edge = new Geometry.LineEdge(new Geometry.Point3d(0, 0, 0), new Geometry.Point3d(1, 0, 0));
            Section section = GetTestSection();
            Material material = new Material();

            return new Beam(edge, material, section, identifier: "Test");
        }

        private static Column GetTestColumn()
        {
            var edge = new Geometry.LineEdge(new Geometry.Point3d(0, 0, 0), new Geometry.Point3d(0, 0, 1));
            Section section = GetTestSection();
            Material material = new Material();

            return new Column(edge, material, section, identifier: "Test");
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
