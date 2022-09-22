using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FemDesign.Geometry.EdgeTests
{
    [TestClass()]
    public class EdgeTests
    {
        [TestMethod("GetLength Arc")]
        public void GetLengthArc()
        {
            // arc by 
            var center = new Point3d(0, 0, 0);
            var radius = 1;
            var startAngle = 0;
            var endAngle = Math.PI;
            var xAxis = new Vector3d(1, 0, 0);
            var cs = CoordinateSystem.Global();
            var arc1 = new Edge(radius, startAngle, endAngle, center, xAxis, cs);
            Assert.IsTrue(Math.Abs(arc1.Length - Math.PI) <= Tolerance.LengthComparison);
        }

        [TestMethod("GetLength Circle")]
        public void GetLengthCircle()
        {
            // circle
            var center = new Point3d(0, 0, 0);
            var radius = 1;
            var cs = CoordinateSystem.Global();
            var circle = new Edge(radius, center, cs);
            Assert.IsTrue(Math.Abs(circle.Length - 2 * Math.PI) <= Tolerance.LengthComparison);
        }

        [TestMethod("GetLength Line")]
        public void GetLengthLine()
        {
            // line
            var p0 = new Point3d(0, 0, 0);
            var p1 = new Point3d(1, 0, 0);
            var y = new Vector3d(0, 1, 0);
            var line = new Edge(p0, p1, y);
            Assert.IsTrue(Math.Abs(line.Length - 1) <= Tolerance.LengthComparison);
        }

        [TestMethod("GetLength Invalid")]
        public void GetLengthInvalid()
        {
            // some other type
            var p0 = new Point3d(0, 0, 0);
            var p1 = new Point3d(1, 0, 0);
            var y = new Vector3d(0, 1, 0);
            var line = new Edge(p0, p1, y);
            var edge = line;
            edge._type = "someType";
            Assert.ThrowsException<ArgumentException>(() => edge.Length, "Should throw exception on not supported edge type");
        }

        [TestMethod("GetLength Arc 3 point")]
        public void GetLengthArc3Point()
        {
            // arc by three points
            var start = new Point3d(-1, 0, 0);
            var mid = new Point3d(0, 1, 0);
            var end = new Point3d(1, 0, 0);
            var cs = CoordinateSystem.Global();
            var arc2 = new Edge(start, mid, end, cs);

            Assert.AreEqual(Math.PI, arc2.Length, Tolerance.LengthComparison);
        }

        [TestMethod("X-Axis")]
        public void XAxis()
        {
            var edge = new Edge(Point3d.Origin, new Point3d(1, 1, 0), Vector3d.UnitZ);
            var direction = edge.XAxis.Normalize();
        }

        [TestMethod("Create-LineEdge")]
        public void EdgeLine()
		{
            // line
            var p0 = new Point3d(0, 0, 0);
            var p1 = new Point3d(1, 0, 0);
            var y = new Vector3d(0, 1, 0);

            var edge = new FemDesign.Geometry.Edge(p0, p1, y);
            var edgeLine = new FemDesign.Geometry.LineEdge(p0, p1, y);

           
            Assert.IsTrue(edge.Length == edgeLine.Length);
            Assert.IsTrue(Math.Abs(edgeLine.Length - 1) <= Tolerance.LengthComparison);


        }
    }
}
