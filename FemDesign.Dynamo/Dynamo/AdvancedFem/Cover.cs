using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;

namespace FemDesign
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Cover
    {
        #region dynamo
        
        /// <summary>
        /// Create a one way cover. Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="supportingStructures">Single structure element och list of structure elements. List cannot be nested - use flatten.</param>
        /// <param name="loadBearingDirection">Vector of load bearing direction.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Cover OneWayCover(Autodesk.DesignScript.Geometry.Surface surface, [DefaultArgument("[]")] List<object> supportingStructures, Autodesk.DesignScript.Geometry.Vector loadBearingDirection = null, string identifier = "CO")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // get loadBearingDirection
            Geometry.Vector3d _loadBearingDirection = Geometry.Vector3d.FromDynamo(loadBearingDirection).Normalize();

            // return
            return Cover.OneWayCover(region, supportingStructures, _loadBearingDirection, identifier);
        }

        /// <summary>
        /// Create a two way cover. 
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="supportingStructures">Single structure element or list of structure elements. List cannot be nested - use flatten.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Cover TwoWayCover(Autodesk.DesignScript.Geometry.Surface surface, [DefaultArgument("[]")] List<object> supportingStructures, string identifier = "CO")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // return
            return Cover.TwoWayCover(region, supportingStructures, identifier);
        }

        /// <summary>
        /// Create Dynamo surface from underlying Region of Cover.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoSurface()
        {
            return this.Region.ToDynamoSurface();
        }

        /// <summary>
        /// Create Dynamo curves from underlying Edges in Region of Cover.
        /// </summary>
        internal List<List<Autodesk.DesignScript.Geometry.Curve>> GetDynamoCurves()
        {
            return this.Region.ToDynamoCurves();
        }

        #endregion
    }
}
