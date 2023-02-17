using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FemDesign.Geometry
{
    [TestClass()]
    public class PlaneTest
    {
        [TestMethod("Project point on plane")]
        public void ProjectPointOnPlane()
        {
            // xy plane
            var xy = Plane.XY;
            var p = new Point3d(1, 1, 3);
            var p2 = xy.ProjectPointOnPlane(p);
            Assert.IsTrue(p2.Equals(new Point3d(1, 1, 0), Tolerance.Point3d));

            // 
            var xDir = new Vector3d(0.960357, -0.079287, -0.267261);
            var yDir = new Vector3d(-0.079287, 0.841427, -0.534522);
            var plane = new Plane(Point3d.Origin, xDir, yDir);
            var p3 = plane.ProjectPointOnPlane(p);
            Assert.IsTrue(p3.Equals(new Point3d(0.142857, -0.714286, 0.428571), 1e-5));
        }
    }
}
