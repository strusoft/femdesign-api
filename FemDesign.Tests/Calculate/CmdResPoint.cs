using FemDesign.Loads;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using FemDesign.Bars;
using FemDesign.Sections;
using FemDesign.Materials;
using FemDesign.Utils;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Summary description for Stability
    /// </summary>
    [TestClass]
    public class CmdResPointTests
    {
        [TestMethod("Constructor")]
        public void Construct()
        {
            var pos = new FemDesign.Geometry.Point3d(10, 0, 15);

            var beam = GetTestBar();

            var identifier = "myResPoint";
            var cmdResPoint = new CmdResultPoint(pos, beam.BarPart.Guid, identifier);

            Assert.IsNotNull(cmdResPoint);
            Assert.AreEqual(cmdResPoint.Point, pos);
            Assert.AreEqual(cmdResPoint.Name, identifier);
            Assert.AreEqual(cmdResPoint.BaseEntity, beam.BarPart.Guid);

            Console.WriteLine(cmdResPoint.SerializeObjectToXml());
        }


        private void BarTest()
        {
            var bar = GetTestBar();
            bar.Identifier = "TestName";
            var guid = "{979386C8-FCA1-4F6D-BBC6-C09900878999}";
            bar.BarPart.Guid = new Guid(guid);
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
