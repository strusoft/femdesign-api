using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class CoordinateSystem
    {
        #region dynamo
        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system of a Line or NurbsCurve(?).
        /// This method realignes the coordinate system.
        /// </summary>
        internal static CoordinateSystem FromDynamoCoordinateSystemLine(Autodesk.DesignScript.Geometry.CoordinateSystem obj)
        {
            Point3d origin = Point3d.FromDynamo(obj.Origin);
            Vector3d localX = Vector3d.FromDynamo(obj.YAxis);
            Vector3d localY = Vector3d.FromDynamo(obj.XAxis.Reverse());
            Vector3d localZ = localX.Cross(localY).Normalize();
            return new CoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// 
        /// Create FdCoordinateSystem from Dynamo coordinate system of a Arc or Circle.
        /// Dynamo Arcs and Circles follow left-hand rule.
        /// This method realignes the coordinate system.
        /// </summary>
        internal static CoordinateSystem FromDynamoCoordinateSystemArcOrCircle(Autodesk.DesignScript.Geometry.CoordinateSystem obj)
        {
            Point3d origin = Point3d.FromDynamo(obj.Origin);
            Vector3d localX = Vector3d.FromDynamo(obj.YAxis);
            Vector3d localY = Vector3d.FromDynamo(obj.XAxis);
            Vector3d localZ = localX.Cross(localY).Normalize();
            return new CoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system of a Surface.
        /// No realignment neccessary.
        /// </summary>
        internal static CoordinateSystem FromDynamoCoordinateSystemSurface(Autodesk.DesignScript.Geometry.CoordinateSystem obj)
        {
            Point3d origin = Point3d.FromDynamo(obj.Origin);
            Vector3d localX = Vector3d.FromDynamo(obj.XAxis);
            Vector3d localY = Vector3d.FromDynamo(obj.YAxis);
            Vector3d localZ = Vector3d.FromDynamo(obj.ZAxis);
            return new CoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system on curve mid u-point.
        /// </summary>
        internal static CoordinateSystem FromDynamoCurve(Autodesk.DesignScript.Geometry.Curve obj)
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
                return CoordinateSystem.FromDynamoCoordinateSystemArcOrCircle(cs);
            }
            else
            {
                return FromDynamoCoordinateSystemLine(cs);
            }
        }

        /// <summary>
        /// Create FdCoordinateSystem from Dynamo coordinate system on surface mid u/v-point.
        /// </summary>
        internal static CoordinateSystem FromDynamoSurface(Autodesk.DesignScript.Geometry.Surface obj)
        {
            Autodesk.DesignScript.Geometry.CoordinateSystem cs = obj.CoordinateSystemAtParameter(0.5, 0.5);
            return CoordinateSystem.FromDynamoCoordinateSystemSurface(cs);
        }
        
        #endregion
    }
}