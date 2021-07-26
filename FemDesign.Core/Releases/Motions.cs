// https://strusoft.com/


namespace FemDesign.Releases
{
    [System.Serializable]
    public partial class Motions : StiffnessType
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Motions()
        {

        }

        /// <summary>
        /// Private constructor [kN/m or kN/m/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression.</param>
        /// <param name="xPos">Kx' tension.</param>
        /// <param name="yNeg">Ky' compression.</param>
        /// <param name="yPos">Ky' tension.</param>
        /// <param name="zNeg">Kz' compression.</param>
        /// <param name="zPos">Kz' tension.</param>
        public Motions(double xNeg = 0, double xPos = 0, double yNeg = 0, double yPos = 0, double zNeg = 0, double zPos = 0)
        {
            this.XNeg = xNeg;
            this.XPos = xPos;
            this.YNeg = yNeg;
            this.YPos = yPos;
            this.ZNeg = zNeg;
            this.ZPos = zPos; 
        }

        /// <summary>
        /// Define a new translations release [kN/m or kN/m/m].
        /// </summary>
        /// <param name="xNeg">Kx' compression [kN/m or kN/m/m].</param>
        /// <param name="xPos">Kx' tension [kN/m or kN/m/m].</param>
        /// <param name="yNeg">Ky' compression [kN/m or kN/m/m].</param>
        /// <param name="yPos">Ky' tension [kN/m or kN/m/m].</param>
        /// <param name="zNeg">Kz' compression [kN/m or kN/m/m].</param>
        /// <param name="zPos">Kz' tension [kN/m or kN/m/m].</param>
        public static Motions Define(double xNeg = 0, double xPos = 0, double yNeg = 0, double yPos = 0, double zNeg = 0, double zPos = 0)
        {
            return new Motions(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }
        /// <summary>
        /// Define a rigid translation release for a point-type release (1.000e+10)
        /// </summary>
        public static Motions RigidPoint()
        {
            return new Motions(10000000000, 10000000000, 10000000000, 10000000000, 10000000000, 10000000000);
        }
        /// <summary>
        /// /// Define a rigid translation release for a line-type release (1.000e+7)
        /// </summary>
        public static Motions RigidLine()
        {
            return new Motions(10000000, 10000000, 10000000, 10000000, 10000000, 10000000);
        }
        /// <summary>
        /// Define a free translation release.
        /// </summary>
        public static Motions Free()
        {
            return new Motions(0, 0, 0, 0, 0, 0);
        }
    }
}