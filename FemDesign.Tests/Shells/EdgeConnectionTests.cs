using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Shells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Shells
{
    [TestClass()]
    public class EdgeConnectionTests
    {
        [TestMethod("Edge connection - Construct custom")]
        public void EdgeConnectionTest1()
        {
            var ec = new EdgeConnection(Releases.Motions.RigidLine(), Releases.Rotations.RigidLine());
            Assert.IsNotNull(ec);
            Assert.IsNull(ec.PredefRigidity);
            Assert.IsNotNull(ec.Rigidity);
        }

        [TestMethod("Edge connection - Construct predefined/library item")]
        public void EdgeConnectionTest2()
        {
            var ec = new EdgeConnection(Releases.Motions.RigidLine(), Releases.Rotations.RigidLine(), "My lib item");
            Assert.IsNotNull(ec);
            Assert.IsNotNull(ec.PredefRigidity);
            Assert.IsNull(ec.Rigidity);
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Edge connection - Deserialize")]
        public void EdgeConnectionTest3()
        {
            string input = "Shells/EdgeConnection-model.struxml";
            var deserialized = Model.DeserializeFromFilePath(input);

            var slabs = deserialized.Entities.Slabs;
            Assert.AreEqual(7, slabs.Count);
            Assert.IsTrue(slabs[0].SlabPart.GetEdgeConnections().Where(ec => ec != null).Count() == 1);
            Assert.IsTrue(slabs[0].SlabPart.GetEdgeConnections().First(ec => ec != null).PredefRigidity.Name == "No shear");
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Edge connection - Serialize custom")]
        public void EdgeConnectionTest4()
        {
            var ec = new EdgeConnection(
                Releases.Motions.Define(xNeg: 123.4), 
                Releases.Rotations.RigidLine()
                );

            Slab slab = CreateDummySlab();

            slab = Slab.EdgeConnection(slab, ec, 0);

            var model = new Model(Country.S, new List<GenericClasses.IStructureElement> { slab });

            string output = "Shells/EdgeConnection_Out.struxml";
            model.SerializeModel(output);

            var deserialized = Model.DeserializeFromFilePath(output);

            Assert.IsTrue(deserialized.Entities.Slabs.Count == 1);
            var actualEdgeConnections = deserialized.Entities.Slabs.First().SlabPart.GetEdgeConnections();
            Assert.IsTrue(actualEdgeConnections.Where(e => e != null).Count() == 1);
            var first = actualEdgeConnections.First();
            Assert.IsTrue(first.IsCustomItem);
            Assert.IsFalse(first.IsLibraryItem);
            Assert.IsNotNull(first.Rigidity);
            Assert.IsNull(first.PredefRigidity);
            Assert.AreEqual(123.4, first.Rigidity.Motions.XNeg, delta: 1e-6);
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Edge connection - Serialize library item")]
        public void EdgeConnectionTest5()
        {
            var ec = new EdgeConnection(
                Releases.Motions.Define(xNeg: 123.4),
                Releases.Rotations.RigidLine(),
                "My library connection"
                );

            Slab slab = CreateDummySlab();

            slab = Slab.EdgeConnection(slab, ec, 0);

            var model = new Model(Country.S, new List<GenericClasses.IStructureElement> { slab });

            string output = "Shells/EdgeConnection_Out.struxml";
            model.SerializeModel(output);

            var deserialized = Model.DeserializeFromFilePath(output);

            Assert.IsTrue(deserialized.Entities.Slabs.Count == 1);
            var actualEdgeConnections = deserialized.Entities.Slabs.First().SlabPart.GetEdgeConnections();
            Assert.IsTrue(actualEdgeConnections.Where(e => e != null).Count() == 1);
            var first = actualEdgeConnections.First();
            Assert.IsTrue(first.IsLibraryItem);
            Assert.IsFalse(first.IsCustomItem);
            Assert.AreEqual("My library connection", first.LibraryName);
            Assert.IsNull(first.Rigidity);
            Assert.IsNotNull(first.PredefRigidity);
            Assert.AreEqual(123.4, first.PredefRigidity.Rigidity.Motions.XNeg, delta: 1e-6);
        }

        private static Slab CreateDummySlab()
        {
            string input = "Shells/EdgeConnection-model.struxml";
            var template = Model.DeserializeFromFilePath(input);

            var p1 = new Geometry.Point3d(0, 0, 0);
            var p2 = new Geometry.Point3d(1, 0, 0);
            var p3 = new Geometry.Point3d(1, 1, 0);
            var p4 = new Geometry.Point3d(0, 1, 0);

            var edges = new List<Geometry.Edge> {
                new Geometry.Edge(p1, p2, Geometry.CoordinateSystem.Global()),
                new Geometry.Edge(p2, p3, Geometry.CoordinateSystem.Global()),
                new Geometry.Edge(p3, p4, Geometry.CoordinateSystem.Global()),
                new Geometry.Edge(p4, p1, Geometry.CoordinateSystem.Global())
            };
            var contour = new Geometry.Contour(edges);
            var region = new Geometry.Region(new List<Geometry.Contour> { contour }, Geometry.CoordinateSystem.Global());
            var slab = Slab.Plate("S", template.Materials.Material[0], region, EdgeConnection.Default, ShellEccentricity.Default, ShellOrthotropy.Default, new List<Thickness> { new Thickness(Geometry.Point3d.Origin, 0.2) });
            return slab;
        }
    }
}