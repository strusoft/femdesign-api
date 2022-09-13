// https://strusoft.com/

using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class FdPoint3d
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
        public static FdPoint3d Origin => new FdPoint3d(0, 0, 0);

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public FdPoint3d()
        {

        }

        /// <summary>
        /// Construct FdPoint3d from coordinates x, y and z.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public FdPoint3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Move a point p along vector v
        /// </summary>
        public static FdPoint3d operator +(FdPoint3d p, FdVector3d v) => new FdPoint3d(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        /// <summary>
        /// Move a point p along vector v
        /// </summary>
        public static FdPoint3d operator +(FdVector3d v, FdPoint3d p) => new FdPoint3d(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        /// <summary>
        /// Move a point p along vector -v
        /// </summary>
        public static FdPoint3d operator -(FdPoint3d p, FdVector3d v) => new FdPoint3d(p.X - v.X, p.Y - v.Y, p.Z - v.Z);
        /// <summary>
        /// Create vector from v2 to v1
        /// </summary>
        public static FdVector3d operator -(FdPoint3d v1, FdPoint3d v2) => new FdVector3d(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);


        /// <summary>
        /// Translate a point by a vector.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns></returns>
        public FdPoint3d Translate(FdVector3d v)
        {
            return new FdPoint3d(this.X + v.X, this.Y + v.Y, this.Z + v.Z);
        }

        /// <summary>
        /// Project point on XY-plane.
        /// </summary>
        /// <returns></returns>
        public FdPoint2d To2d()
        {
            return new FdPoint2d(this.X, this.Y);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            FdPoint3d p = obj as FdPoint3d;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);            
        }

        public bool Equals(FdPoint3d p)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);
        }

        public bool Equals(FdPoint3d p, double tolerance)
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

        public static implicit operator FdPoint3d(FdCoordinateSystem plane)
        {
            var x = plane.Origin.X;
            var y = plane.Origin.Y;
            var z = plane.Origin.Z;
            var point = new FdPoint3d(x, y, z);
            return point;
        }
        public override string ToString()
        {
            return $"({this.X.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Y.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Z.ToString(FemDesign.TextFormatting.decimalRounding)})";
        }
    }
}