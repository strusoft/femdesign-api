using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.Calculate;
using FemDesign;
using FemDesign.Bars;
using FemDesign.Materials;
using FemDesign.Sections;

namespace FemDesign.Calculate
{
    [TestClass()]
    public partial class CmdListGenTest
    {
        [TestMethod("CreateCmdListGen")]
        public void CreateCmdListGen()
        {
            // Read model and results
            var strModel = Model.DeserializeFromFilePath(@"Calculate\pickElement.struxml");

            var myElement = strModel.Entities.Bars.Skip(2).Take(4);

            var elements = new List<FemDesign.GenericClasses.IStructureElement>();

            foreach(FemDesign.GenericClasses.IStructureElement element in myElement)
                elements.Add(element);

            using (var femDesign = new FemDesignConnection(keepOpen: true))
            {
                // Run analysis and red some results
                var myFile = @"C:\GitHub\femdesign-api\FemDesign.Tests\Calculate\pickElement.str";
                femDesign.Open(myFile);
                var results = femDesign.GetResults<Results.NodalDisplacement>(elements: elements);

                var a = 9;
            }
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