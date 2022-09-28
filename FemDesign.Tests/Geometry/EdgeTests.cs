using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FemDesign.Geometry
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

        [TestMethod("Create-VerticalLineEdge")]
        public void VerticalEdgeLine()
        {
            // line
            var p0 = new Point3d(0, 0, 0);
            var p1 = new Point3d(0, 0, 1);

            var edge = new FemDesign.Geometry.Edge(p0, p1);
            var edgeLine = new FemDesign.Geometry.LineEdge(p0, p1);

            Assert.IsTrue(edge.Length == edgeLine.Length);
            Assert.IsTrue(Math.Abs(edgeLine.Length - 1) <= Tolerance.LengthComparison);
        }

        [TestMethod("Create-RadialEdgeLines")]
        public void RadialEdgeLines()
        {
            foreach(var line in CreateLines())
			{
                Assert.IsTrue( line.IsLine() );
			}
        }

        [TestMethod("PointAtLength")]
        public void PointAtLenght()
        {
            foreach(var line in CreateLines())
			{
                var p0 = line.PointAtLength(0.0);
                var p1 = line.PointAtLength(0.5);
                var p2 = line.PointAtLength(1.0);

                Assert.ThrowsException<ArgumentException>(() => line.PointAtLength(20.0));
                Assert.ThrowsException<ArgumentException>(() => line.PointAtLength(-5.0));
            }

            
            var p3 = new Point3d(0, 0, -1);
            var p4 = new Point3d(12, 0, 4);

            var lineEdge = new LineEdge(p3, p4);
            var pointOncurve = lineEdge.PointAtLength(1.0);

            // Values taken from Rhino Method
            Assert.AreEqual(pointOncurve.X, 0.923077, 0.01);
            Assert.AreEqual(pointOncurve.Y, 0.00, 0.01);
            Assert.AreEqual(pointOncurve.Z, -0.615385, 0.01);

            lineEdge = new LineEdge(p3, p4 * -1);
            pointOncurve = lineEdge.PointAtLength(1.0);

            // Value taken from Rhino Method
            Assert.AreEqual(pointOncurve.X, -0.970143, 0.01);
            Assert.AreEqual(pointOncurve.Y, 0.00, 0.01);
            Assert.AreEqual(pointOncurve.Z, -1.242536, 0.01);

            p3 = new Point3d(0, 0, 0);
            p4 = new Point3d(10, 0, 0);

            lineEdge = new LineEdge(p3, p4);
            pointOncurve = lineEdge.PointAtLength(1.0);

            // Value taken from Rhino Method
            Assert.AreEqual(pointOncurve.X, 1.00, 0.01);
            Assert.AreEqual(pointOncurve.Y, 0.00, 0.01);
            Assert.AreEqual(pointOncurve.Z, 0.00, 0.01);


            p3 = new Point3d(0, 0, -5);
            p4 = new Point3d(0, 0, 5);

            lineEdge = new LineEdge(p3, p4);
            pointOncurve = lineEdge.PointAtLength(5.0);

            // Value taken from Rhino Method
            Assert.AreEqual(pointOncurve.X, 0.00, 0.01);
            Assert.AreEqual(pointOncurve.Y, 0.00, 0.01);
            Assert.AreEqual(pointOncurve.Z, 0.00, 0.01);

            pointOncurve = lineEdge.PointAtLength(1.0);

            // Value taken from Rhino Method
            Assert.AreEqual(pointOncurve.X, 0.00, 0.01);
            Assert.AreEqual(pointOncurve.Y, 0.00, 0.01);
            Assert.AreEqual(pointOncurve.Z, -4.00, 0.01);

        }

        private List<LineEdge> CreateLines()
        {
            // line
            var edgeLines = new List<LineEdge>();
            var edges = new List<Edge>();
            var p0 = new Point3d(0, 0, 0);

            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    for (int k = -1; k < 1; k++)
                    {
                        if (i == 0 && j == 0 && k == 0) continue;
                        var p1 = new Point3d(i, j, k);
                        var edgeLine = new FemDesign.Geometry.LineEdge(p0, p1);
                        var edge = new FemDesign.Geometry.Edge(p0, p1);

                        edgeLines.Add(edgeLine);
                        edges.Add(edge);
                    }
                }
            }
            return edgeLines;
        }
    }
}
