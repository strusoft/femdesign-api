using System.Collections.Generic;
using System.Xml.Serialization;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class SurfaceLoad: ForceLoadBase
    {
        /// <summary>
        /// Create a uniform surface load.
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="force">Force.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SurfaceLoad Uniform(Autodesk.DesignScript.Geometry.Surface surface, Autodesk.DesignScript.Geometry.Vector force, LoadCase loadCase, string comment = "")
        {
            Geometry.Region region = Geometry.Region.FromDynamo(surface);
            Geometry.Vector3d _force = Geometry.Vector3d.FromDynamo(force);

            SurfaceLoad surfaceLoad = SurfaceLoad.Uniform(region, _force, loadCase, false, comment);

            return surfaceLoad;
        }

        /// <summary>
        /// Create a variable surface load.
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="direction">Vector. Direction of force.</param>
        /// <param name="loadLocationValue">Loads. List of 3 items [q1, q2, q3].</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SurfaceLoad Variable(Autodesk.DesignScript.Geometry.Surface surface, Autodesk.DesignScript.Geometry.Vector direction, List<LoadLocationValue> loadLocationValue, LoadCase loadCase, string comment = "")
        {
            if (loadLocationValue.Count != 3)
            {
                throw new System.ArgumentException("loads must contain 3 items");
            }

            Geometry.Region region = Geometry.Region.FromDynamo(surface);
            Geometry.Vector3d loadDirection = Geometry.Vector3d.FromDynamo(direction).Normalize();

            SurfaceLoad surfaceLoad = SurfaceLoad.Variable(region, loadDirection, loadLocationValue, loadCase, false, comment);
            return surfaceLoad;
        }

        /// <summary>
        /// Convert surface of SurfaceLoad to a Dynamo surface.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoGeometry()
        {
            return this.Region.ToDynamoSurface();
        }
    }
}