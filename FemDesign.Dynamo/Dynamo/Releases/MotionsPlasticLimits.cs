
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Releases
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class MotionsPlasticLimits : PlasticityType3d
    {
        /// <summary>
        /// Define a new translations release plastic limit [kN/m or kN/m/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression. [kN/m or kN/m/m]. Default input null gives no plastic limit. </param>
        /// <param name="xPos">Kx' tension. [kN/m or kN/m/m]. Default input null gives no plastic limit. </param>
        /// <param name="yNeg">Ky' compression. [kN/m or kN/m/m]. Default input null gives no plastic limit. </param>
        /// <param name="yPos">Ky' tension. [kN/m or kN/m/m]. Default input null gives no plastic limit. </param>
        /// <param name="zNeg">Kz' compression. [kN/m or kN/m/m]. Default input null gives no plastic limit. </param>
        /// <param name="zPos">Kz' tension. [kN/m or kN/m/m]. Default input null gives no plastic limit. </param>
        [IsVisibleInDynamoLibrary(true)]
        public static MotionsPlasticLimits Define([DefaultArgument("null")] double? xNeg = null, [DefaultArgument("null")] double? xPos = null, [DefaultArgument("null")] double? yNeg = null, [DefaultArgument("null")] double? yPos = null, [DefaultArgument("null")] double? zNeg = null, [DefaultArgument("null")] double? zPos = null)
        {
            return new MotionsPlasticLimits(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }

        /// <summary>
        /// Default translations release plastic limit. [kN/m or kN/m/m] Default input gives no plastic limit. .
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        public static MotionsPlasticLimits Default()
        {
            return MotionsPlasticLimits.Define();
        }
    }
}