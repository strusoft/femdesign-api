using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Shells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Bars
{
    [TestClass()]
    public class CreateElementsTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("Create a Linear Beam,Bar,Truss and Deserialise")]
        public void CreateLineElement()
        {
            // Define geometry
            var p1 = new Geometry.Point3d(2.0, 2.0, 0);
            var p2 = new Geometry.Point3d(10, 2.0, 0);

            var p3 = new Geometry.Point3d(4.0, 0.0, 0);
            var p4 = new Geometry.Point3d(4.0, 0.0, 5.0);

            var p5 = new Geometry.Point3d(2.0, 2.0, 2);
            var p6 = new Geometry.Point3d(10, 2.0, 2);

            // Create elements
            //var edge = new Geometry.Edge(p1, p2, Geometry.FdVector3d.UnitZ());
            var edge = new Geometry.LineEdge(p1, p2, Geometry.Vector3d.UnitZ);
            var vertical = new Geometry.LineEdge(p3, p4, Geometry.Vector3d.UnitX);
            var edgeTruss = new Geometry.LineEdge(p5, p6, Geometry.Vector3d.UnitZ);


            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("Bars//materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("Bars//sections.struxml");

            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            var beam = new Bars.Beam(
                edge,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.Rigid },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                identifier: "B");

            var column = new Bars.Column(
                vertical,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.Rigid },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                identifier: "C");

            var truss = new Bars.Truss(
                edgeTruss,
                material,
                section,
                identifier: "T");

            var elements = new List<GenericClasses.IStructureElement>() { beam, column, truss };


            // Add to model
            Model model = new Model(Country.S);
            model.AddElements(elements);

            model.Open();
            var struxmlTxt = model.SerializeToString();
            Console.WriteLine(struxmlTxt);

            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var filePath = System.IO.Path.Combine(currentDirectory, "myModel.struxml");
            var myModel = FemDesign.Model.DeserializeFromFilePath(filePath);

            Assert.AreEqual(3, myModel.Entities.Bars.Count);

            Assert.IsTrue( myModel.Entities.Bars[0] is Bars.Bar );
            Assert.IsTrue( myModel.Entities.Bars[0] is Bars.Beam );

            Assert.IsTrue( myModel.Entities.Bars[1] is Bars.Bar );
            Assert.IsTrue( myModel.Entities.Bars[1] is Bars.Column );

            Assert.IsTrue( myModel.Entities.Bars[2] is Bars.Bar );
            Assert.IsTrue( myModel.Entities.Bars[2] is Bars.Truss );
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Create an Arc Beam,Bar,Truss and Deserialise")]
        public void CreateArcElement()
        {
            // arc by three points
            var p1 = new Geometry.Point3d(-1, 0, 0);
            var p2 = new Geometry.Point3d(0, 1, 0);
            var p3 = new Geometry.Point3d(1, 0, 0);
            var cs = Geometry.CoordinateSystem.Global();

            // Create elements
            var edge = new Geometry.ArcEdge(p1, p2, p3, cs);

            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("Bars//materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("Bars//sections.struxml");

            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            var beam = new Bars.Beam(
                edge,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.Rigid },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                identifier: "B");

			var elements = new List<GenericClasses.IStructureElement>() { beam };


            // Add to model
            Model model = new Model(Country.S);
            model.AddElements(elements);

            model.Open();
            var struxmlTxt = model.SerializeToString();
            Console.WriteLine(struxmlTxt);

            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var filePath = System.IO.Path.Combine(currentDirectory, "myModel.struxml");
            var myModel = FemDesign.Model.DeserializeFromFilePath(filePath);

            Assert.AreEqual(1, myModel.Entities.Bars.Count);

            Assert.IsTrue(myModel.Entities.Bars[0] is Bars.Bar);
            Assert.IsTrue(myModel.Entities.Bars[0] is Bars.Beam);
        }


    }
}