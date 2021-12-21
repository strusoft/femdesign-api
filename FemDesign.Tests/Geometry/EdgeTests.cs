using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FemDesign.Geometry.Tests
{
    [TestClass()]
    public class EdgeTests
    {
        [TestMethod("GetLength")]
        public void GetLength()
        {
            // arc by 
            var center = new FdPoint3d(0,0,0);
            var radius = 1;
            var startAngle = 0;
            var endAngle = Math.PI;
            var xAxis = new FdVector3d(1,0,0);
            var cs = FdCoordinateSystem.Global();
            var arc1 = new Edge(radius, startAngle, endAngle, center, xAxis, cs);
            Assert.IsTrue(Math.Abs(arc1.Length - Math.PI) <= Tolerance.LengthComparison);

            // circle
            var circle = new Edge(radius, center, cs);
            Assert.IsTrue(Math.Abs(circle.Length - 2 * Math.PI) <= Tolerance.LengthComparison);

            // line
            var p0 = new FdPoint3d(0,0,0);
            var p1 = new FdPoint3d(1,0,0);
            var y = new FdVector3d(0,1,0);
            var line = new Edge(p0, p1, y);
            Assert.IsTrue(Math.Abs(line.Length - 1) <= Tolerance.LengthComparison);

            // some other type
            var edge = line;
            edge._type = "someType";
            Assert.ThrowsException<ArgumentException>(() => edge.Length, "Should throw exception on not supported edge type");
            
            // arc by three points
            var start = new FdPoint3d(-1,0,0);
            var mid = new FdPoint3d(0,1,0);
            var end = new FdPoint3d(1,0,0);
            var arc2 = new Edge(start, mid, end, cs);
            //Assert.ThrowsException<ArgumentException>(() => arc2.Length, "Should throw exception stating that method to calculate sweep angle is not implemented yet.");
            Assert.IsTrue(Math.Abs(arc2.Length - Math.PI) <= Tolerance.LengthComparison); // test should fail here 
        }
    }
}