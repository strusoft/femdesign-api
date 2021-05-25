// https://strusoft.com/


namespace FemDesign.Releases
{
    [System.Serializable]
    public partial class RotationsPlasticLimits : PlasticityType3d
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private RotationsPlasticLimits()
        {

        }

        /// <summary>
        /// Define a new rotations release plastic limit [kN/m or kN/m/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression. [kN/m or kN/m/m].</param>
        /// <param name="xPos">Kx' tension. [kN/m or kN/m/m].</param>
        /// <param name="yNeg">Ky' compression. [kN/m or kN/m/m].</param>
        /// <param name="yPos">Ky' tension. [kN/m or kN/m/m].</param>
        /// <param name="zNeg">Kz' compression. [kN/m or kN/m/m].</param>
        /// <param name="zPos">Kz' tension. [kN/m or kN/m/m].</param>
        public RotationsPlasticLimits(double xNeg = 1e15, double xPos = 1e15, double yNeg = 1e15, double yPos = 1e15, double zNeg = 1e15, double zPos = 1e15)
        {
            this.XNeg = xNeg;
            this.XPos = xPos;
            this.YNeg = yNeg;
            this.YPos = yPos;
            this.ZNeg = zNeg;
            this.ZPos = zPos; 
        }
    }
}