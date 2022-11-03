
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class PressureLoad: ForceLoadBase
    {
        #region dynamo
        /// <summary>
        /// Create a hydrostatic surface load.
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="direction">Vector. Direction of force.</param>
        /// <param name="z0">Surface level of soil/water (on the global Z axis).</param>
        /// <param name="q0">Load intensity at the surface level.</param>
        /// <param name="qh">Increment of load intensity per meter (along the global Z axis).</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static PressureLoad Define(Autodesk.DesignScript.Geometry.Surface surface, Autodesk.DesignScript.Geometry.Vector direction, double z0, double q0, double qh, LoadCase loadCase, string comment = "")
        {
            // get fdGeometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // normalize direction
            Geometry.Vector3d _loadDirection = Geometry.Vector3d.FromDynamo(direction).Normalize();

            // create SurfaceLoad
            PressureLoad _pressureLoad = new PressureLoad(region, _loadDirection, z0, q0, qh, loadCase, comment, false, ForceLoadType.Force);
            return _pressureLoad;
        }

        /// <summary>
        /// Convert surface of PressureLoad to a Dynamo surface.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoGeometry()
        {
            return this.Region.ToDynamoSurface();
        }
        #endregion
    }
}