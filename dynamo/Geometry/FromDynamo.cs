using FemDesign;
using FemDesign.Geometry;
using FemDesign.Supports;

namespace FemDesign.Dynamo
{
    internal static partial class Convert
    {
        #region Point
        
        /// <summary>
        /// Create FdPoint3d from Dynamo point.
        /// </summary>
        public static FdPoint3d FromDynamo(this Autodesk.DesignScript.Geometry.Point point)
        {
            return new FdPoint3d(point.X, point.Y, point.Z);
        }

        #endregion
    }
}
