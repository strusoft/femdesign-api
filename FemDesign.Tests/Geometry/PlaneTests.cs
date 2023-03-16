using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FemDesign.Geometry
{
    [TestClass]
    public class PlaneTest
    {
        private static Vector3d _xDir = new Vector3d(0.903751, -0.076999, -0.421076);
        private static Vector3d _yDir = new Vector3d(-0.076999, 0.938401, -0.336861);
        public Plane Plane
        {
            get
            {
                return new Plane(Point3d.Origin, _xDir, _yDir);
            }
        }
        [TestMethod]
        public void SetXAroundZ()
        {
            var plane = Plane;
            plane.SetXAroundZ(new Vector3d(0.744172, 0.402517, -0.533093));
            Assert.IsTrue(plane.LocalY.Equals(new Vector3d(-0.518559, 0.851179, -0.081192), 1e-6));
        }
        [TestMethod]
        public void SetYAroundX()
        {
            var plane = Plane;
            plane.SetYAroundX(new Vector3d(0.143855, 0.981109, 0.129346));
            Assert.IsTrue(plane.LocalZ.Equals(new Vector3d(0.403162, -0.177471, 0.897755), 1e-6));
        }
        [TestMethod]
        public void SetYAroundZ()
        {
            var plane = Plane;
            plane.SetYAroundZ(new Vector3d(-0.518559, 0.851178, -0.081192));
            Assert.IsTrue(plane.LocalX.Equals(new Vector3d(0.744172, 0.402518, -0.533093), 1e-6));
        }
        [TestMethod]
        public void SetZAroundX()
        {
            var plane = Plane;
            plane.SetZAroundX(new Vector3d(0.403162, -0.17747, 0.897755));
            Assert.IsTrue(plane.LocalY.Equals(new Vector3d(0.143855, 0.981109, 0.129346), 1e-6));
        }
    }
}
