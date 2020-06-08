// https://strusoft.com/
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class FdCoordinateSystem
    {
        public FdPoint3d origin { get; set; }
        public FdVector3d localX { get; set; }
        public FdVector3d localY { get; set; }
        public FdVector3d localZ { get; set; }

        /// <summary>
        /// Construct FdCoordinateSystem from origin point and local x, y, z axes.
        /// </summary>
        public FdCoordinateSystem(FdPoint3d _origin, FdVector3d _localX, FdVector3d _localY, FdVector3d _localZ)
        {
            this.origin = _origin;
            this.localX = _localX;
            this.localY = _localY;
            this.localZ = _localZ;
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