using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Geometry.ProximityTests
{
    [TestClass()]
    public class ProximityTests
    {
        [TestMethod()]
        public void LineLineProximityTest1()
        {
            // Line 1
            var p1 = new FdPoint3d(0, 0, 0);
            var v1 = new FdVector3d(2, 0, 0);
            // Line 2
            var p2 = new FdPoint3d(1, -1, 1);
            var v2 = new FdVector3d(0, 2, 0);

            var (t1, t2) = Proximity.LineLineProximity(p1, v1, p2, v2);

            Assert.AreEqual(0.5, t1, Tolerance.LengthComparison);
            Assert.AreEqual(0.5, t2, Tolerance.LengthComparison);
        }

        [TestMethod()]
        public void LineLineProximityTest2()
        {
            // Line 1
            var p1 = new FdPoint3d(1, 1, 0);
            var v1 = new FdVector3d(2, 0, 0);
            // Line 2
            var p2 = new FdPoint3d(1, 0, 1);
            var v2 = new FdVector3d(2, 2, 0);

            var (t1, t2) = Proximity.LineLineProximity(p1, v1, p2, v2);

            Assert.AreEqual(0.5, t1, Tolerance.LengthComparison);
            Assert.AreEqual(0.5, t2, Tolerance.LengthComparison);
        }
    }
}