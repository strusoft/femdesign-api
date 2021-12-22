
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Releases
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class RotationsPlasticLimits : PlasticityType3d
    {
        /// <summary>
        /// Define a new rotations release plastic limit [kN/m or kN/m/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression. [kN/m or kN/m/m]. Default input gives no plastic limit.</param>
        /// <param name="xPos">Kx' tension. [kN/m or kN/m/m]. Default input gives no plastic limit.</param>
        /// <param name="yNeg">Ky' compression. [kN/m or kN/m/m]. Default input gives no plastic limit.</param>
        /// <param name="yPos">Ky' tension. [kN/m or kN/m/m]. Default input gives no plastic limit.</param>
        /// <param name="zNeg">Kz' compression. [kN/m or kN/m/m]. Default input gives no plastic limit.</param>
        /// <param name="zPos">Kz' tension. [kN/m or kN/m/m]. Default input gives no plastic limit.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static RotationsPlasticLimits Define([DefaultArgument("null")] double? xNeg = null, [DefaultArgument("null")] double? xPos = null, [DefaultArgument("null")] double? yNeg = null, [DefaultArgument("null")] double? yPos = null, [DefaultArgument("null")] double? zNeg = null, [DefaultArgument("null")] double? zPos = null)
        {
            return new RotationsPlasticLimits(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }

        /// <summary>
        /// Default rotations release plastic limit. [kN/m or kN/m/m]. Default input gives no plastic limit.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public static RotationsPlasticLimits Default()
        {
            return RotationsPlasticLimits.Define();
        }
    }
}