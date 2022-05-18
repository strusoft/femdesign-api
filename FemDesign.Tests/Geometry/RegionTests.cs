using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Geometry.Tests
{
    [TestClass()]
    public class RegionTests
    {
        [TestMethod("SetEdgeConnection - All")]
        public void SetEdgeConnectionTest1()
        {
            string input = "Geometry/Slabs.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab = model.Entities.Slabs.First();

            var before = slab.SlabPart.GetEdgeConnections();
            foreach (var ec in before)
                Assert.IsNull(ec);


            // Set the edge connections
            slab.SlabPart.Region.SetEdgeConnections(Shells.EdgeConnection.GetHinged());

            CollectionAssert.AllItemsAreNotNull(slab.SlabPart.GetEdgeConnections(), "Should have edge connections");
        }

        [TestMethod("SetEdgeConnection - At index")]
        public void SetEdgeConnectionTest2()
        {
            string input = "Geometry/Slabs.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab = model.Entities.Slabs.First();

            var before = slab.SlabPart.GetEdgeConnections();
            foreach (var ec in before)
                Assert.IsNull(ec);


            // Set the edge connections
            slab.SlabPart.Region.SetEdgeConnection(Shells.EdgeConnection.GetHinged(), 0);
            slab.SlabPart.Region.SetEdgeConnection(Shells.EdgeConnection.GetHinged(), 1);

            Assert.IsTrue(slab.SlabPart.GetEdgeConnections().Where(ec => !(ec is null)).Count() == 2, "Should have edge connections");
        }

        [TestMethod("SetEdgeConnection - All by index")]
        public void SetEdgeConnectionTest3()
        {
            string input = "Geometry/Slabs.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab = model.Entities.Slabs.First();

            var before = slab.SlabPart.GetEdgeConnections();
            foreach (var ec in before)
                Assert.IsNull(ec);


            // Set the edge connections
            slab.SlabPart.Region.SetEdgeConnection(Shells.EdgeConnection.GetRigid(), 0);
            slab.SlabPart.Region.SetEdgeConnection(Shells.EdgeConnection.GetHinged(), 1);
            slab.SlabPart.Region.SetEdgeConnection(new Shells.EdgeConnection(Releases.Motions.Free(), Releases.Rotations.Free()), 2);
            slab.SlabPart.Region.SetEdgeConnection(new Shells.EdgeConnection(Releases.Motions.RigidLine(), Releases.Rotations.RigidLine()), 3);

            var edgeConnections = slab.SlabPart.GetEdgeConnections();
            Assert.IsNull(edgeConnections[0], $"Rigid edge connection 0 should be null");
            Assert.IsNotNull(edgeConnections[1], $"Edge connection 1 should not be null");
            Assert.IsNotNull(edgeConnections[2], $"Edge connection 2 should not be null");
            Assert.IsNotNull(edgeConnections[3], $"Edge connection 3 should not be null");
        }

        [TestMethod("SetEdgeConnection - Set hinge on slab")]
        public void SetEdgeConnectionTest4()
        {
            string input = "Geometry/Slabs.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab = model.Entities.Slabs.First();

            var before = slab.SlabPart.GetEdgeConnections();
            foreach (var ec in before)
                Assert.IsNull(ec);


            // Set the edge connections
            var newSlab = Shells.Slab.EdgeConnection(slab, Shells.EdgeConnection.GetHinged(), new List<int> { 1, 3 });

            Assert.IsTrue(newSlab.SlabPart.GetEdgeConnections().Where(ec => !(ec is null)).Count() == 2, "Should have edge connections");
            Assert.IsNotNull(newSlab.SlabPart.GetEdgeConnections()[1], "Should have edge connection");
            Assert.IsNotNull(newSlab.SlabPart.GetEdgeConnections()[3], "Should have edge connection");
        }

        [TestMethod("SetEdgeConnection - Set rigid on slab")]
        public void SetEdgeConnectionTest8()
        {
            string input = "Geometry/Slabs.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab = model.Entities.Slabs.First();

            var before = slab.SlabPart.GetEdgeConnections();
            foreach (var ec in before)
                Assert.IsNull(ec);


            // Set the edge connections
            var newSlab = Shells.Slab.EdgeConnection(slab, Shells.EdgeConnection.GetRigid(), new List<int> { 1, 3 });

            Assert.IsNull(newSlab.SlabPart.GetEdgeConnections()[1], "Should be null if the edge connection is default rigid");
            Assert.IsNull(newSlab.SlabPart.GetEdgeConnections()[3], "Should be null if the edge connection is default rigid");
            Assert.IsTrue(newSlab.SlabPart.GetEdgeConnections().Where(ec => !(ec is null)).Count() == 0, "Should have edge connections");
        }

        [TestMethod("SetEdgeConnection - Add to model and overwrite")]
        public void SetEdgeConnectionTest5()
        {
            string input = "Geometry/Slabs.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab = model.Entities.Slabs.First();

            var before = slab.SlabPart.GetEdgeConnections();
            foreach (var ec in before)
                Assert.IsNull(ec);


            // Set the edge connections
            var newSlab = Shells.Slab.EdgeConnection(slab, Shells.EdgeConnection.GetHinged(), new List<int> { 1, 3 });

            model.AddElements(new List<GenericClasses.IStructureElement> { newSlab });

            var added = model.Entities.Slabs.First(s => s.Guid == slab.Guid);

            Assert.IsNull(   added.SlabPart.GetEdgeConnections()[0]);
            Assert.IsNotNull(added.SlabPart.GetEdgeConnections()[1]);
            Assert.IsNull(   added.SlabPart.GetEdgeConnections()[2]);
            Assert.IsNotNull(added.SlabPart.GetEdgeConnections()[3]);
        }


        [TestMethod("SetEdgeConnection - serialized")]
        public void SetEdgeConnectionTest6()
        {
            string input = "Geometry/Slabs.struxml";
            string output = "Geometry/SlabsWithEdgeConnection.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab = model.Entities.Slabs.First();

            var before = slab.SlabPart.GetEdgeConnections();
            foreach (var ec in before)
                Assert.IsNull(ec);


            // Set the edge connections
            var newSlab = Shells.Slab.EdgeConnection(slab, Shells.EdgeConnection.GetHinged(), new List<int> { 1, 3 });

            model.AddElements(new List<GenericClasses.IStructureElement> { newSlab });

            model.SerializeModel(output);
        }

        [TestMethod("SetEdgeConnection - deserialize")]
        public void SetEdgeConnectionTest7()
        {
            string input = "Geometry/Slabs.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var slab1 = model.Entities.Slabs[0];
            var slab2 = model.Entities.Slabs[1];

            var ec1 = slab1.SlabPart.GetEdgeConnections();
            var ec2 = slab2.SlabPart.GetEdgeConnections();

            Assert.IsTrue(ec1.Count == 4);
            Assert.IsNull(ec1[0]);
            Assert.IsNull(ec1[1]);
            Assert.IsNull(ec1[2]);
            Assert.IsNull(ec1[3]);

            Assert.IsNull(ec2[0]);
            Assert.IsNotNull(ec2[1]);
            Assert.IsTrue(ec2[1].Rigidity.Motions.XNeg == 0.0);
            Assert.IsNotNull(ec2[2]);
            Assert.IsTrue(ec2[2].Rigidity.Motions.XNeg == 10000000);
            Assert.IsNotNull(ec2[3]);
            Assert.IsTrue(ec2[3].Rigidity.Motions.XNeg == 10000000);
        }

        [TestMethod("SetEdgeConnection - Deserialize-serialize")]
        public void SetEdgeConnectionTest9()
        {
            string input = "Geometry/Slabs.struxml";
            string output = "Geometry/Slabs-deserialize-serialize.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            model.SerializeModel(output);
        }
    }
}