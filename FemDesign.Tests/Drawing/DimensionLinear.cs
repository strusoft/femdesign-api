using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FemDesign.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FemDesign.Drawing
{
    [TestClass()]
    public class DimensionLinearTest
    {
        [TestMethod("Calculate distances of linear dimension")]
        public void Distances()
        {
            var p0 = new Point3d(0, 0, 0);
            var p1 = new Point3d(1, 1, 5);
            var p2 = new Point3d(3, 3, 7);
            var refPoints = new List<Point3d> { p0, p1, p2 };
            
            // 1
            var plane = new Plane(Point3d.Origin, new Vector3d(1, 1, 0), new Vector3d(-1, 1, 0));
            var dim = new DimensionLinear(refPoints, plane);
            Assert.IsTrue(Math.Abs(dim.Distances[0] - 1.414214) < 1e-6);
            Assert.IsTrue(Math.Abs(dim.Distances[1] - 2.828427) < 1e-6);

            // 2
            plane = new Plane(Point3d.Origin, new Vector3d(1, 1, 1), new Vector3d(-1, 1, 0));
            dim = new DimensionLinear(refPoints, plane);
            Assert.IsTrue(Math.Abs(dim.Distances[0] - 4.041452) < 1e-6);
            Assert.IsTrue(Math.Abs(dim.Distances[1] - 3.464102) < 1e-6);    
        }
    }
}
