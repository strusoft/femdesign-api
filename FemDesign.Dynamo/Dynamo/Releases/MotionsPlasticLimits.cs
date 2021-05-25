
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
        /// <param name="xNeg">Kx' compression. [kN/m or kN/m/m].</param>
        /// <param name="xPos">Kx' tension. [kN/m or kN/m/m].</param>
        /// <param name="yNeg">Ky' compression. [kN/m or kN/m/m].</param>
        /// <param name="yPos">Ky' tension. [kN/m or kN/m/m].</param>
        /// <param name="zNeg">Kz' compression. [kN/m or kN/m/m].</param>
        /// <param name="zPos">Kz' tension. [kN/m or kN/m/m].</param>
        [IsVisibleInDynamoLibrary(true)]
        public static MotionsPlasticLimits Define(double xNeg = 1e15, double xPos = 1e15, double yNeg = 1e15, double yPos = 1e15, double zNeg = 1e15, double zPos = 1e15)
        {
            return new MotionsPlasticLimits(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }

        /// <summary>
        /// Default translations release plastic limit. Default 10^15 [kN/m or kN/m/m].
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        public static MotionsPlasticLimits Default()
        {
            return MotionsPlasticLimits.Define();
        }
    }
}