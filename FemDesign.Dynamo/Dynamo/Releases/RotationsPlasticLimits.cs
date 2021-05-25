
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
        /// <param name="xNeg">Kx' compression. [kN/m or kN/m/m].</param>
        /// <param name="xPos">Kx' tension. [kN/m or kN/m/m].</param>
        /// <param name="yNeg">Ky' compression. [kN/m or kN/m/m].</param>
        /// <param name="yPos">Ky' tension. [kN/m or kN/m/m].</param>
        /// <param name="zNeg">Kz' compression. [kN/m or kN/m/m].</param>
        /// <param name="zPos">Kz' tension. [kN/m or kN/m/m].</param>
        [IsVisibleInDynamoLibrary(true)]
        public static RotationsPlasticLimits Define(double xNeg = 1e15, double xPos = 1e15, double yNeg = 1e15, double yPos = 1e15, double zNeg = 1e15, double zPos = 1e15)
        {
            return new RotationsPlasticLimits(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }

        /// <summary>
        /// Default rotations release plastic limit. Default 10^15 [kN/m or kN/m/m].
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        public static RotationsPlasticLimits Default()
        {
            return RotationsPlasticLimits.Define();
        }
    }
}