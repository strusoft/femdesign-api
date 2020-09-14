// https://strusoft.com/

using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class FdPoint3d
    {
        [XmlAttribute("x")]
        public double X;
        [XmlAttribute("y")]
        public double Y;
        [XmlAttribute("z")]
        public double Z;

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
        internal FdPoint3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

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
        internal FdPoint2d To2d()
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

        #region dynamo
        /// <summary>
        /// Create FdPoint3d from Dynamo point.
        /// </summary>
        internal static FdPoint3d FromDynamo(Autodesk.DesignScript.Geometry.Point point)
        {
            FdPoint3d newPoint = new FdPoint3d(point.X, point.Y, point.Z);
            return newPoint;
        }

        /// <summary>
        /// Create Dynamo point from FdPoint3d.
        /// </summary>
        public Autodesk.DesignScript.Geometry.Point ToDynamo()
        {
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(this.X, this.Y, this.Z);
        }

        #endregion
    }
}