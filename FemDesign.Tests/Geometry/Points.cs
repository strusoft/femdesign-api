using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FemDesign;

namespace FemDesign.Geometry
{
    [TestClass]
    public class Points
    {
        [TestMethod("Colinear Points")]
        public void ColinearPoints()
        {
            var p1 = new Geometry.Point3d(1, 0, 0);
            var p2 = new Geometry.Point3d(2, 0, 0);
            var p3 = new Geometry.Point3d(3, 0, 0);
            var p4 = new Geometry.Point3d(4, 0, 0);

            var points = new List<Point3d> { p1, p2, p3, p4 };


            var p5 = new Geometry.Point3d(0, 1, 0);
            points.Add(p5);
            Assert.IsTrue(Point3d.ArePointsOnPlane(points));

            var p6 = new Geometry.Point3d(0, 2, 0);
            points.Add(p6);
            Assert.IsTrue(Point3d.ArePointsOnPlane(points));

            var p7 = new Geometry.Point3d(0, 1, 1);
            points.Add(p7);
            Assert.IsFalse(Point3d.ArePointsOnPlane(points));
        }
    }
}
