using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class TopBotLocationValue: Geometry.Point3d
    {
        #region dynamo
        /// <summary>
        /// Define a top bottom location value
        /// </summary>
        /// <param name="point">Location of value</param>
        /// <param name="topVal">Top value</param>
        /// <param name="bottomVal">Bottom value</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static TopBotLocationValue Define(Autodesk.DesignScript.Geometry.Point point, double topVal, double bottomVal)
        {
            // convert geometry
            Geometry.Point3d p = Geometry.Point3d.FromDynamo(point); 

            return new TopBotLocationValue(p, topVal, bottomVal);
        }
        #endregion
    }
}