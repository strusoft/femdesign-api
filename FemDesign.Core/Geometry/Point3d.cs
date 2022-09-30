// https://strusoft.com/

using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class Point3d
    {
        [XmlAttribute("x")]
        public double X;
        [XmlAttribute("y")]
        public double Y;
        [XmlAttribute("z")]
        public double Z;

        /// <summary>
        /// Construct FdPoint3d in origin
        /// </summary>
        public static Point3d Origin => new Point3d(0, 0, 0);

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Point3d()
        {

        }

        /// <summary>
        /// Construct FdPoint3d from coordinates x, y and z.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Move a point p along vector v
        /// </summary>
        public static Point3d operator +(Point3d p, Vector3d v) => new Point3d(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        /// <summary>
        /// Move a point p along vector v
        /// </summary>
        public static Point3d operator +(Vector3d v, Point3d p) => new Point3d(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        /// <summary>
        /// Move a point p along vector -v
        /// </summary>
        public static Point3d operator -(Point3d p, Vector3d v) => new Point3d(p.X - v.X, p.Y - v.Y, p.Z - v.Z);
        /// <summary>
        /// Create vector from p2 to p1
        /// </summary>
        public static Vector3d operator -(Point3d p1, Point3d p2) => new Vector3d(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        public static Point3d operator +(Point3d p1, Point3d p2) => new Point3d(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        public static Point3d operator /(Point3d p1, double number) => new Point3d(p1.X / number, p1.Y / number, p1.Z / number);
        public static Point3d operator *(Point3d p1, double number) => new Point3d(p1.X * number, p1.Y * number, p1.Z * number);
        public static Point3d operator *(double number, Point3d p1) => new Point3d(p1.X * number, p1.Y * number, p1.Z * number);


        /// <summary>
        /// Translate a point by a vector.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns></returns>
        public Point3d Translate(Vector3d v)
        {
            return new Point3d(this.X + v.X, this.Y + v.Y, this.Z + v.Z);
        }

        /// <summary>
        /// Project point on XY-plane.
        /// </summary>
        /// <returns></returns>
        public Point2d To2d()
        {
            return new Point2d(this.X, this.Y);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Point3d p = obj as Point3d;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);            
        }

        public bool Equals(Point3d p)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);
        }

        public bool Equals(Point3d p, double tolerance)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (Math.Abs(X - p.X) < tolerance) && (Math.Abs(Y - p.Y) < tolerance) && (Math.Abs(Z - p.Z) < tolerance);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public static implicit operator Point3d(CoordinateSystem plane)
        {
            var x = plane.Origin.X;
            var y = plane.Origin.Y;
            var z = plane.Origin.Z;
            var point = new Point3d(x, y, z);
            return point;
        }

        public override string ToString()
        {
            return $"({this.X.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Y.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Z.ToString(FemDesign.TextFormatting.decimalRounding)})";
        }
    }
}