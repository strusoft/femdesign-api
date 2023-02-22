using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Geometry
{
    public class Plane
    {
        public Point3d Origin { get; set; }
        private Vector3d _xDir;
        public Vector3d XDir
        {
            get
            {
                return _xDir;
            }
        }
        private Vector3d _yDir;
        public Vector3d YDir
        {
            get
            {
                return _yDir;
            }
        }
        public Vector3d Normal
        {
            get
            {
                return XDir.Cross(YDir).Normalize();
            }
        }
        public static Plane XY
        {
            get
            {
                return new Plane(Point3d.Origin, Vector3d.UnitX, Vector3d.UnitY);
            }
        }
        public static Plane XZ
        {
            get
            {
                return new Plane(Point3d.Origin, Vector3d.UnitX, Vector3d.UnitZ);
            }
        }
        public static Plane YZ
        {
            get
            {
                return new Plane(Point3d.Origin, Vector3d.UnitY, Vector3d.UnitZ);
            }
        }
        public Plane(Point3d origin, Vector3d localX, Vector3d localY)
        {
            Origin = origin;
            if (localX.IsPerpendicular(localY))
            {
                _xDir = localX.Normalize();
                _yDir = localY.Normalize();
            }
            else
            {
                throw new System.ArgumentException($"Can't construct plane. LocalX: {localX} and LocalY: {localY} are not perpendicular");
            }
        }

        /// <summary>
        /// Returns the closes point on plane
        /// </summary>
        public Point3d ProjectPointOnPlane(Point3d p)
        {
            // get vector from plane origin to p
            Vector3d op = p - Origin;

            // calculate projection of OP on plane normal
            Vector3d v = op.Dot(Normal)*Normal;

            // move p with v
            Point3d pointOnPlane = p - v;

            // return point on plane
            return pointOnPlane;
        }
    }
}
