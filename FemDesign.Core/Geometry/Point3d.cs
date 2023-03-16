// https://strusoft.com/

using System;
using System.Collections.Generic;
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

        public static implicit operator Point3d(Plane plane)
        {
            var x = plane.Origin.X;
            var y = plane.Origin.Y;
            var z = plane.Origin.Z;
            var point = new Point3d(x, y, z);
            return point;
        }

        public static implicit operator Point3d(FemDesign.Results.FeaNode feaNode)
        {
            var point = new Geometry.Point3d(feaNode.X, feaNode.Y, feaNode.Z);
            return point;
        }


        private static (double a, double b , double c,  double d) _getPlaneEquation(Point3d p1, Point3d p2, Point3d p3)
        {
            double a1 = p2.X - p1.X;
            double b1 = p2.Y - p1.Y;
            double c1 = p2.Z - p1.Z;

            double a2 = p3.X - p1.X;
            double b2 = p3.Y - p1.Y;
            double c2 = p3.Z - p1.Z;

            double a = b1 * c2 - b2 * c1;
            double b = a2 * c1 - a1 * c2;
            double c = a1 * b2 - b1 * a2;

            double d = (-a * p1.X - b * p1.Y - c * p1.Z);

            return (a, b, c, d);
        }


        public static bool ArePointsCollinear(Point3d p1, Point3d p2, Point3d p3)
        {
            double area = 0.5 * ((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));
            return (area == 0);
        }


        public static bool ArePointsOnPlane(List<Point3d> points)
        {
            int i = 0;
            bool colinearPoints = true;

            while( colinearPoints && i < points.Count )
            {
                colinearPoints = ArePointsCollinear(points[0], points[1], points[i]);
                i++;
            }

            if (colinearPoints)
            {
                throw new Exception("Points are colinear!");
            }

            (double a, double b, double c, double d) = _getPlaneEquation(points[0], points[1], points[i-1]);

            for(int j = 0; j < points.Count; j++)
            {
                bool IsOnPlane = (a * points[j].X + b * points[j].Y + c * points[j].Z + d) == 0;
                if (IsOnPlane == true)
                    continue;
                else
                    return false;
            }

            return true;

        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Point_type_3d(Point3d p) => new StruSoft.Interop.StruXml.Data.Point_type_3d{
            X = p.X,
            Y = p.Y,
            Z = p.Z
        };


        public override string ToString()
        {
            return $"({this.X.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Y.ToString(FemDesign.TextFormatting.decimalRounding)}, {this.Z.ToString(FemDesign.TextFormatting.decimalRounding)})";
        }
    }
}