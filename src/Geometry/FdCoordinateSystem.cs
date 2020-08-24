// https://strusoft.com/
using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public class FdCoordinateSystem
    {
        [XmlElement("local_pos", Order=1)]
        public FdPoint3d origin { get; set; }
        [XmlElement("local_x", Order=2)]
        public FdVector3d _localX;
        [XmlIgnore]
        public FdVector3d localX
        {
            get
            {
                return this._localX;
            }
            set
            {
                Geometry.FdVector3d val = value.Normalize();
                Geometry.FdVector3d z = this.localZ;

                double dot = z.Dot(val);
                if (Math.Abs(dot) < Tolerance.dotProduct)
                {
                    this._localX = val;
                    this._localY = z.Cross(val); // follows right-hand-rule
                }
                
                else
                {
                    throw new System.ArgumentException($"X-axis is not perpendicular to Z-axis. The dot-product is {dot}, but should be 0");
                }
            }
        }        
        [XmlElement("local_y", Order=3)]
        public FdVector3d _localY;
        [XmlIgnore]
        public FdVector3d localY
        {
            get
            {
                return this._localY;
            }
        }
        [XmlIgnore]
        public FdVector3d _localZ;
        [XmlIgnore]
        public FdVector3d localZ
        {
            get
            {
                if (this.localX == null || this.localY == null)
                {
                    throw new System.ArgumentException("Impossible to construct z-axis as either this.localX or this.localY is null.");
                }
                else
                {     
                    return this.localX.Cross(localY).Normalize();
                }
            }
            set
            {
                Geometry.FdVector3d val = value.Normalize();
                Geometry.FdVector3d x = this.localX;

                double dot = x.Dot(val);
                if (Math.Abs(dot) < Tolerance.dotProduct)
                {
                    this._localZ = val;
                    this._localY = val.Cross(x); // follows right-hand-rule
                }
                
                else
                {
                    throw new System.ArgumentException($"Z-axis is not perpendicular to X-axis. The dot-product is {dot}, but should be 0");
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public FdCoordinateSystem()
        {

        }

        /// <summary>
        /// Construct FdCoordinateSystem from origin point and local x, y, z axes.
        /// </summary>
        public FdCoordinateSystem(FdPoint3d _origin, FdVector3d _localX, FdVector3d _localY, FdVector3d _localZ)
        {
            this.origin = _origin;
            this._localX = _localX;
            this._localY = _localY;
            this._localZ = _localZ;
        }


        #region grasshopper

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane.
        /// </summary>
        internal static FdCoordinateSystem FromRhinoPlane(Rhino.Geometry.Plane obj)
        {
            FdPoint3d origin = FdPoint3d.FromRhino(obj.Origin);
            FdVector3d localX = FdVector3d.FromRhino(obj.XAxis);
            FdVector3d localY = FdVector3d.FromRhino(obj.YAxis);
            FdVector3d localZ = FdVector3d.FromRhino(obj.ZAxis);
            return new FdCoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane on curve mid u-point.
        /// </summary>
        internal static FdCoordinateSystem FromRhinoCurve(Rhino.Geometry.Curve obj)
        {
            // reparameterize if neccessary
            if (obj.Domain.T0 == 0 && obj.Domain.T1 == 1)
            {
                // pass
            }
            else
            {
                obj.Domain = new Rhino.Geometry.Interval(0, 1);
            }

            // get frame @ mid-point
            Rhino.Geometry.Plane plane;
            obj.FrameAt(0.5, out plane);
            return FdCoordinateSystem.FromRhinoPlane(plane);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane on surface mid u/v-point.
        /// </summary>
        internal static FdCoordinateSystem FromRhinoSurface(Rhino.Geometry.Surface obj)
        {
            // reparameterize if necessary
            if (obj.Domain(0).T0 == 0 && obj.Domain(1).T0 == 0 && obj.Domain(0).T1 == 1 && obj.Domain(1).T1 == 1)
            {
                // pass
            }
            else
            {
                obj.SetDomain(0, new Rhino.Geometry.Interval(0, 1));
                obj.SetDomain(1, new Rhino.Geometry.Interval(0, 1));
            }

            Rhino.Geometry.Plane plane;
            obj.FrameAt(0.5, 0.5, out plane);
            return FdCoordinateSystem.FromRhinoPlane(plane);
        }
        #endregion
    }
}