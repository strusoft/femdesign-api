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
        public double x;
        [XmlAttribute("y")]
        public double y;
        [XmlAttribute("z")]
        public double z;

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
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Translate a point by a vector.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns></returns>
        public FdPoint3d Translate(FdVector3d v)
        {
            return new FdPoint3d(this.x + v.x, this.y + v.y, this.z + v.z);
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
            return (x == p.x) && (y == p.y) && (z == p.z);            
        }

        public bool Equals(FdPoint3d p)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) && (z == p.z);
        }

        public bool Equals(FdPoint3d p, double tolerance)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (Math.Abs(x - p.x) < tolerance) && (Math.Abs(y - p.y) < tolerance) && (Math.Abs(z - p.z) < tolerance);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
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
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(this.x, this.y, this.z);
        }

        #endregion
    }
}