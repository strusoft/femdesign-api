using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Collections.Generic;

using FemDesign;
using FemDesign.Geometry;
using FemDesign.Materials;
using FemDesign.Sections;
using FemDesign.GenericClasses;

namespace FemDesign.Performance
{
    [TestClass()]
    public class BeamGenerationTests
    {
        [TestMethod("GenerateBeam")]
        [TestCategory("FEM-Design required"), TestCategory("Performance")]
        [DataRow(800)]
        public void NumberOfBeam(int numberOfBeam)
        {

            var model = new Model(Country.S);
            var elements = new List<GenericClasses.IStructureElement>();


            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("Performance\\materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("Performance\\sections.struxml");
            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            // arc by 
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < numberOfBeam; i++)
            {
                var startPoint = new Point3d(0, i, 0);
                var endPoint = new Point3d(5 , i, 0);
                var edge = new LineEdge(startPoint, endPoint, Vector3d.UnitY);

                var bar = new Bars.Bar(
                    edge,
                    Bars.BarType.Beam,
                    material,
                    sections: new Sections.Section[] { section },
                    connectivities: new Bars.Connectivity[] { Bars.Connectivity.Hinged },
                    eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                    identifier: "B");
                bar.BarPart.LocalY = Geometry.Vector3d.UnitY;

                elements.Add(bar);
            }

            model.AddElements(elements);
            stopWatch.Stop();
            var time = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Time to create {numberOfBeam} elements is: {time}");

            stopWatch.Reset();
            stopWatch.Start();
            model.SerializeToString();
            stopWatch.Stop();
            time = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Time to Serialise {numberOfBeam} elements is: {time}");
            model.Open();
            Assert.IsTrue(time < 1500);
        }
    }
}
