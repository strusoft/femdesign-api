
using System.Xml.Serialization;
using System.Collections.Generic;

#region dynamo
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class SurfaceReinforcement: EntityBase
    {
        #region dynamo
        
        /// <summary>
        /// Create straight surface reinforcement for a portion of a slab. This surface reinforcement will cover the passed surface of any slab it is applied to. Note that the surface must be contained within the target slab.
        /// </summary>
        /// <param name="surface">Surface.</param>
        /// <param name="straight">Straight reinforcement layout.</param>
        /// <param name="wire">Wire.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SurfaceReinforcement BySurface(Surface surface, Straight straight, Wire wire)
        {
            // convert geometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // return
            return SurfaceReinforcement.DefineStraightSurfaceReinforcement(region, straight, wire);
        }
        
        /// <summary>
        /// Add surface reinforcement to slab.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="slab">Slab.</param>
        /// <param name="surfaceReinforcement">SurfaceReinforcment to add to slab. Item or list. Nested lists are not supported.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Shells.Slab AddToSlab(Shells.Slab slab, List<SurfaceReinforcement> surfaceReinforcement)
        {
            // return
            return SurfaceReinforcement.AddReinforcementToSlab(slab, surfaceReinforcement);
        }

        #endregion
    }
}