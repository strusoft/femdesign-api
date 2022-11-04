using System.Xml.Serialization;
using FemDesign.Geometry;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Footfall
    {
        #region dynamo

        /// <summary>
        /// Create footfall full excitation point.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point">Point.</param>
        /// <param name="identifier">Identifier.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Footfall FullExcitation(Autodesk.DesignScript.Geometry.Point point, string identifier = "FE", string comment = "")
        {
            var p0 = Point3d.FromDynamo(point);
            return new Footfall(p0, identifier, comment);
        }

        /// <summary>
        /// Create footfall self excitation region.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="region">Surface.</param>
        /// <param name="identifier">Identifier.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Footfall SelfExcitation(Autodesk.DesignScript.Geometry.Surface region, string identifier = "SE", string comment = "")
        {
            var p0 = Geometry.Region.FromDynamo(region);
            return new Footfall(p0, identifier, comment);
        }

        #endregion
    }
}