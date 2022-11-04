using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.AuxiliaryResults;
using FemDesign.Geometry;

namespace FemDesign.AuxiliaryResults
{
    [TestClass()]
    public class LabelledSectionTests
    {
        [TestInitialize]
        public void Initialize()
        {
            LabelledSection.ResetInstanceCount();
        }

        [TestMethod("LabelledSection instance count")]
        public void ResetInstanceCountTest()
        {

            var verticies = new List<Point3d>() { new Point3d(0, 0, 0), new Point3d(1, 0, 0) };
            var LS1 = new LabelledSection(verticies);
            var LS2 = new LabelledSection(verticies);
            Assert.IsTrue(LS1.Name.EndsWith("1"), $"First instance should be named \"1\". Got {LS1.Name}");
            Assert.IsTrue(LS2.Name.EndsWith("2"), $"Second instance should be named \"2\". Got {LS2.Name}");
            
            LabelledSection.ResetInstanceCount();
            var LS3 = new LabelledSection(verticies);
            Assert.IsTrue(LS3.Name.EndsWith("1"), "Instance should be reset.");
        }

        [TestMethod("LabelledSection geometry")]
        public void LabelledSectionTest()
        {
            var line = new List<Point3d>() { new Point3d(0, 0, 0), new Point3d(1, 0, 0) };
            var triangle = new List<Point3d>() { new Point3d(0, 0, 0), new Point3d(1, 0, 0), new Point3d(1, 1, 0), new Point3d(0, 0, 0) };

            var LS1 = new LabelledSection(line);
            Assert.IsNotNull(LS1._lineSegment, "Two verticies should give LS with linesegment");
            Assert.IsNull(LS1._polyline, "Two verticies should not give LS with polyline");

            var LS2 = new LabelledSection(triangle);
            Assert.IsNotNull(LS2._polyline, "Three or more verticies should give LS with polyline");
            Assert.IsNull(LS2._lineSegment, "Three or more verticies should not give LS with linesegment");
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("LabelledSection serialization")]
        public void LabelledSectionSerializeTest()
        {
            Model model = new Model(Country.COMMON);

            var verticies = new List<Point3d>() { new Point3d(0, 0, 0), new Point3d(1, 0, 0), new Point3d(1, 1, 0), new Point3d(0, 1, 0) };
            var labelledSections = new List<LabelledSection>()
            {
                new LabelledSection(verticies.GetRange(0, 3)),
                new LabelledSection(verticies.GetRange(2, 2)),
                new LabelledSection(identifier: "LS", verticies[3], verticies[0]),
            };

            model.AddElements(labelledSections);
            string temp = System.IO.Path.GetTempPath() + ".struxml";
            model.SerializeModel(temp);

            Model deserialized = Model.DeserializeFromFilePath(temp);

            Assert.IsTrue(deserialized.Entities.LabelledSections.LabelledSections.Count == model.Entities.LabelledSections.LabelledSections.Count, "All LabelledSections should be (de-)serialized.");
            for (int i = 0; i < model.Entities.LabelledSections.LabelledSections.Count; i++)
            {
                var originalLS = model.Entities.LabelledSections.LabelledSections[i];
                var deserializedLS = deserialized.Entities.LabelledSections.LabelledSections[i];
                Assert.IsTrue(deserializedLS.Verticies.Count == originalLS.Verticies.Count, "All verticies should be (de-)serialized.");
            }

            System.IO.File.Delete(temp);
        }
    }
}