// https://strusoft.com/

using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
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

        /// <summary>
        /// Project point on XY-plane.
        /// </summary>
        /// <returns></returns>
        internal FdPoint2d To2d()
        {
            return new FdPoint2d(this.x, this.y);
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

        #region grasshopper
        /// <summary>
        /// Create FdPoint3d from Rhino point.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static FemDesign.Geometry.FdPoint3d FromRhino(Rhino.Geometry.Point3d obj)
        {
            //
            double x, y, z;
            x = obj.X;
            y = obj.Y;
            z = obj.Z;

            // 
            return new FemDesign.Geometry.FdPoint3d(x, y, z);
        }

        /// <summary>
        /// Create Rhino point from FdPoint3d.
        /// </summary>
        public Rhino.Geometry.Point3d ToRhino()
        {
            return new Rhino.Geometry.Point3d(this.x, this.y, this.z);
        }
        #endregion
    }
}