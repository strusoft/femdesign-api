using System.Xml.Serialization;
using System.Collections.Generic;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class SurfaceTemperatureLoad: LoadBase
    {
        #region dynamo
        /// <summary>
        /// Define a surface temperature load
        /// </summary>
        /// <param name="surface">Surface</param>
        /// <param name="direction">Direction of load</param>
        /// <param name="tempLocValue">Top and bottom temperature location values. Should be 1 or 3 elements. Use one if uniform and 3 if variable.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SurfaceTemperatureLoad Define(Autodesk.DesignScript.Geometry.Surface surface, Autodesk.DesignScript.Geometry.Vector direction, List<TopBotLocationValue> tempLocValue, LoadCase loadCase, string comment = "")
        {
            // convert geometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);
            Geometry.Vector3d dir = Geometry.Vector3d.FromDynamo(direction);

            // return
            return new SurfaceTemperatureLoad(region, dir, tempLocValue, loadCase, comment);
        }
        #endregion
    }
}