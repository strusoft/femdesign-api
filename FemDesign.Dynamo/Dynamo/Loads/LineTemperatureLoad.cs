using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LineTemperatureLoad: LoadBase
    {
        #region dynamo
        /// <summary>
        /// Define a line temperature load
        /// </summary>
        /// <param name="curve">Curve of line temperature load</param>
        /// <param name="direction">Direction of load</param>
        /// <param name="topBottomLocationValues">Top bottom location value</param>
        /// <param name="loadCase">Load case of load</param>
        /// <param name="comments">Comment of load</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineTemperatureLoad Define(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,1)")] Autodesk.DesignScript.Geometry.Vector direction, List<TopBotLocationValue> topBottomLocationValues, LoadCase loadCase, string comments = "")
        {
            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            Geometry.Vector3d v = Geometry.Vector3d.FromDynamo(direction);

            // return
            return new LineTemperatureLoad(edge, v, topBottomLocationValues, loadCase, comments);
        }
        #endregion
    }
}