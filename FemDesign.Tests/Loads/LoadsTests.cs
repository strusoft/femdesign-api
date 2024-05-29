using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Loads
{
    [TestClass()]
    public class LoadsTests
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("Moving loads deserialize")]
        public void MovingLoadsTest1()
        {
            Model model = Model.DeserializeFromFilePath("Loads/MovingLoads - No indexed Guid.struxml");
            Assert.IsNotNull(model, "Should read model with no IndexedGuid");

            Assert.IsNotNull(model.Entities.Loads.MovingLoads);
            Assert.AreEqual(1, model.Entities.Loads.MovingLoads.Count);
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Moving loads deserialize-serialize")]
        public void MovingLoadsTest2()
        {
            string path = "Loads/MovingLoads - No indexed Guid.struxml";
            string outPath = "Loads/out.struxml";
            Model actual = Model.DeserializeFromFilePath(path);

            actual.SerializeModel(outPath);
            Model expected = Model.DeserializeFromFilePath(outPath);

            Assert.AreEqual(expected.Entities.Loads.MovingLoads.Count, expected.Entities.Loads.MovingLoads.Count);
        }

        [TestMethod("Mass")]
        public void MassTest()
        {
            // create mass
            Geometry.Point3d position = new Geometry.Point3d(0, 0, 0);
            double value = 1.0;
            bool applyEcc = true;
            Mass mass = new Mass(position, value, applyEcc);

            // check properties
            Assert.AreEqual(position, mass.Position);
            Assert.AreEqual(value, mass.Value);
            Assert.AreEqual("", mass.Comment);
            Assert.AreEqual(applyEcc, mass.ApplyOnEcc);

            Assert.IsNotNull(mass.Guid);

        }

        [TestMethod("ExcitationLoad")]
        public void ExcitationLoadTest()
        {
            Diagram diagram;
            List<TimeHistoryDiagram> th_diagram;

            th_diagram = new List<TimeHistoryDiagram>()
            {
                new TimeHistoryDiagram(0.1, 0),
                new TimeHistoryDiagram(0.2, 1),
            };
        

            Assert.ThrowsException<ArgumentException>(() => new Diagram("diagram1", th_diagram));
            Assert.ThrowsException<ArgumentException>(() => new Diagram("diagram3", new List<double> { 0.2, 1, 2 }, new List<double> { 1, 0.1, 0.3 }));


            th_diagram = new List<TimeHistoryDiagram>()
            {
                new TimeHistoryDiagram(0.0, 0),
                new TimeHistoryDiagram(0.2, 0.2),
                new TimeHistoryDiagram(0.3, 0.5),
            };

            diagram = new Diagram("diagram2", th_diagram);

            Assert.IsTrue(diagram.Records.Count == 3);
            Assert.IsTrue(diagram.Records[0].Time == 0.0);
            Assert.IsTrue(diagram._startValue == 0);
            Assert.IsTrue(diagram.Records[0].Value == 0);


            th_diagram = new List<TimeHistoryDiagram>()
            {
                new TimeHistoryDiagram(0.0, 0.5),
                new TimeHistoryDiagram(0.2, 1),
                new TimeHistoryDiagram(0.3, -1),
            };

            diagram = new Diagram("diagram2", th_diagram);

            Assert.IsTrue(diagram.Records.Count == 3);
            Assert.IsTrue(diagram.Records[0].Time == 0.0);
            Assert.IsTrue(diagram._startValue == 0.5);
            Assert.IsTrue(diagram.Records[0].Value == 0.5);

        }

    }
}