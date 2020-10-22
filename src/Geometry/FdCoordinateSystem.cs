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
    public class FdCoordinateSystem
    {
        [XmlElement("local_pos", Order=1)]
        public FdPoint3d Origin { get; set; }
        [XmlElement("local_x", Order=2)]
        public FdVector3d _localX;
        [XmlIgnore]
        public FdVector3d LocalX
        {
            get
            {
                if (this._localX == null)
                {
                    throw new System.ArgumentException("LocalX is null. Property must be set.");
                }
                else
                {
                    return this._localX;
                }
            }
        }        
        [XmlElement("local_y", Order=3)]
        public FdVector3d _localY;
        [XmlIgnore]
        public FdVector3d LocalY
        {
            get
            {
                if (this._localY == null)
                {
                    throw new System.ArgumentException("LocalY is null. Property must be set.");
                }
                else
                {
                    return this._localY;
                }
            } 
        }
        [XmlIgnore]
        public FdVector3d _localZ;
        [XmlIgnore]
        public FdVector3d LocalZ
        {
            get
            {
                if (this.LocalX == null || this.LocalY == null)
                {
                    throw new System.ArgumentException("Impossible to get z-axis as either this.localX or this.localY is null.");
                }
                else
                {     
                    return this.LocalX.Cross(LocalY).Normalize();
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private FdCoordinateSystem()
        {

        }

        /// <summary>
        /// Construct FdCoordinateSystem from origin point and local x and y axes.
        /// </summary>
        public FdCoordinateSystem(FdPoint3d origin, FdVector3d localX, FdVector3d localY)
        {
            this.Origin = origin;
            this._localX = localX;
            this._localY = localY;
            this._localZ = localX.Cross(localY);
            
            if (!this.IsComplete())
            {
                throw new System.ArgumentException("The defined coordinate system is not complete!");  
            }

            if (!this.IsOrthogonal())
            {
                throw new System.ArgumentException($"The defined coordinate system is not orthogonal within the tolerance {Tolerance.DotProduct}");
            }
        }

        /// <summary>
        /// Construct FdCoordinateSystem from origin point and local x, y, z axes.
        /// </summary>
        public FdCoordinateSystem(FdPoint3d origin, FdVector3d localX, FdVector3d localY, FdVector3d localZ)
        {
            this.Origin = origin;
            this._localX = localX;
            this._localY = localY;
            this._localZ = localZ;

            if (!this.IsComplete())
            {
                throw new System.ArgumentException("The defined coordinate system is not complete!");
            }

            if (!this.IsOrthogonal())
            {
                throw new System.ArgumentException($"The defined coordinate system is not orthogonal within the tolerance {Tolerance.DotProduct}");
            }
        }

        /// <summary>
        /// Check if this coordinate system is complete. 
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            return (this.Origin != null) && (this._localX != null) && (this._localY != null) && (this._localZ != null);
        }

        /// <summary>
        /// Check if this coordinate system is orthogonal within the tolernace.
        /// </summary>
        /// <returns></returns>
        public bool IsOrthogonal()
        {
            return (Math.Abs(this._localX.Dot(this._localY)) < Tolerance.DotProduct) && this._localX.Cross(this._localY).Equals(this._localZ, Tolerance.Point3d);
        }

        /// <summary>
        /// Set X-axis and rotate coordinate system accordingly around Z-axis.
        /// </summary>
        public void SetXAroundZ(FdVector3d vector)
        {
            // try to set axis
            FdVector3d val = vector.Normalize();
            FdVector3d z = this.LocalZ;

            double dot = z.Dot(val);
            if (Math.Abs(dot) < Tolerance.DotProduct)
            {
                this._localX = val;
                this._localY = z.Cross(val); // follows right-hand-rule
            }
            else
            {
                throw new System.ArgumentException($"The passed X-axis is not perpendicular to Z-axis. The dot-product is {dot}, but should be 0");
            }
        }

        /// <summary>
        /// Set Y-axis and rotate coordinate system accordingly around X-Axis.
        /// </summary>
        public void SetYAroundX(FdVector3d vector)
        {
            // try to set axis
            FdVector3d val = vector.Normalize();
            FdVector3d x = this.LocalX;

            double dot = x.Dot(val);
            if (Math.Abs(dot) < Tolerance.DotProduct)
            {
                this._localY = val;
                this._localZ = x.Cross(val); // follows right-hand-rule
            }
            else
            {
                throw new System.ArgumentException($"The passed Y-axis is not perpendicular to X-axis. The dot-product is {dot}, but should be 0");
            }
        }

        /// <summary>
        /// Set Y-axis and rotate coordinate system accordingly around Z-axis
        /// </summary>
        public void SetYAroundZ(FdVector3d vector)
        {
            // try set axis
            FdVector3d val = vector.Normalize();
            FdVector3d z = this.LocalZ;

            double dot = z.Dot(val);
            if (Math.Abs(dot) < Tolerance.DotProduct)
            {
                this._localY = val;
                this._localX = val.Cross(z); // follows right-hand-rule
            }
            
            else
            {
                throw new System.ArgumentException($"Y-axis is not perpendicular to Z-axis. The dot-product is {dot}, but should be 0");
            }
        }

        /// <summary>
        /// Set Z-axis and rotate coordinate system accordingly around X-axis
        /// </summary>
        public void SetZAroundX(FdVector3d vector)
        {
            // try to set axis
            Geometry.FdVector3d val = vector.Normalize();
            Geometry.FdVector3d x = this.LocalX;

            double dot = x.Dot(val);
            if (Math.Abs(dot) < Tolerance.DotProduct)
            {
                this._localZ = val;
                this._localY = val.Cross(x); // follows right-hand-rule
            }
            
            else
            {
                throw new System.ArgumentException($"Z-axis is not perpendicular to X-axis. The dot-product is {dot}, but should be 0");
            }
        }


        #region dynamo
        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system of a Line or NurbsCurve(?).
        /// This method realignes the coordinate system.
        /// </summary>
        internal static FdCoordinateSystem FromDynamoCoordinateSystemLine(Autodesk.DesignScript.Geometry.CoordinateSystem obj)
        {
            FdPoint3d origin = FdPoint3d.FromDynamo(obj.Origin);
            FdVector3d localX = FdVector3d.FromDynamo(obj.YAxis);
            FdVector3d localY = FdVector3d.FromDynamo(obj.XAxis.Reverse());
            FdVector3d localZ = localX.Cross(localY).Normalize();
            return new FdCoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// 
        /// Create FdCoordinateSystem from Dynamo coordinate system of a Arc or Circle.
        /// Dynamo Arcs and Circles follow left-hand rule.
        /// This method realignes the coordinate system.
        /// </summary>
        internal static FdCoordinateSystem FromDynamoCoordinateSystemArcOrCircle(Autodesk.DesignScript.Geometry.CoordinateSystem obj)
        {
            FdPoint3d origin = FdPoint3d.FromDynamo(obj.Origin);
            FdVector3d localX = FdVector3d.FromDynamo(obj.YAxis);
            FdVector3d localY = FdVector3d.FromDynamo(obj.XAxis);
            FdVector3d localZ = localX.Cross(localY).Normalize();
            return new FdCoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system of a Surface.
        /// No realignment neccessary.
        /// </summary>
        internal static FdCoordinateSystem FromDynamoCoordinateSystemSurface(Autodesk.DesignScript.Geometry.CoordinateSystem obj)
        {
            FdPoint3d origin = FdPoint3d.FromDynamo(obj.Origin);
            FdVector3d localX = FdVector3d.FromDynamo(obj.XAxis);
            FdVector3d localY = FdVector3d.FromDynamo(obj.YAxis);
            FdVector3d localZ = FdVector3d.FromDynamo(obj.ZAxis);
            return new FdCoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system on curve mid u-point.
        /// </summary>
        internal static FdCoordinateSystem FromDynamoCurve(Autodesk.DesignScript.Geometry.Curve obj)
        {
            // CoordinateSystemAtParameter returns a coordinate system on curve
            // with origin at the point at the given parameter.
            // The XAxis is aligned with the curve normal,
            // the YAxis is aligned with the curve tangent at this point, 
            // and the ZAxis is aligned with the up-vector or binormal at this point
            Autodesk.DesignScript.Geometry.CoordinateSystem cs = obj.CoordinateSystemAtParameter(0.5);

            // Note: Arcs and Circles in Dynamo are defined with left-hand rule while coordinate system is defined by right-hand rule
            if (obj.GetType() == typeof(Autodesk.DesignScript.Geometry.Arc) || obj.GetType() == typeof(Autodesk.DesignScript.Geometry.Circle))
            {
                return FdCoordinateSystem.FromDynamoCoordinateSystemArcOrCircle(cs);
            }
            else
            {
                return FromDynamoCoordinateSystemLine(cs);
            }
        }

        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system on surface mid u/v-point.
        /// </summary>
        internal static FdCoordinateSystem FromDynamoSurface(Autodesk.DesignScript.Geometry.Surface obj)
        {
            Autodesk.DesignScript.Geometry.CoordinateSystem cs = obj.CoordinateSystemAtParameter(0.5, 0.5);
            return FdCoordinateSystem.FromDynamoCoordinateSystemSurface(cs);
        }
        #endregion

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