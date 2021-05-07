using FemDesign;
using FemDesign.Geometry;
using FemDesign.Supports;

namespace FemDesign.Dynamo
{
    internal static partial class Convert
    {
        #region Point

        /// <summary>
        /// Create Dynamo point from FdPoint3d.
        /// </summary>
        internal static Autodesk.DesignScript.Geometry.Point ToDynamo(this FdPoint3d point)
        {
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(point.X, point.Y, point.Z);
        }
        #endregion

        #region Supports

        internal static Autodesk.DesignScript.Geometry.Point GetDynamoGeometry(this PointSupport pointSupport)
        {
            return pointSupport.Position.ToDynamo();
        }
        #endregion
    }
}
