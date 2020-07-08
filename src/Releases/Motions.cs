// https://strusoft.com/


namespace FemDesign.Releases
{
    [System.Serializable]
    public class Motions: StiffnessType
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
        /// <param name="x_neg">Kx' compression.</param>
        /// <param name="x_pos">Kx' tension.</param>
        /// <param name="y_neg">Ky' compression.</param>
        /// <param name="y_pos">Ky' tension.</param>
        /// <param name="z_neg">Kz' compression.</param>
        /// <param name="z_pos">Kz' tension.</param>
        private Motions(double x_neg, double x_pos, double y_neg, double y_pos, double z_neg, double z_pos)
        {
            this.x_neg = x_neg;
            this.x_pos = x_pos;
            this.y_neg = y_neg;
            this.y_pos = y_pos;
            this.z_neg = z_neg;
            this.z_pos = z_pos; 
        }

        /// <summary>
        /// Define a new translations release [kN/m or kN/m/m].
        /// </summary>
        /// <param name="x_neg">Kx' compression [kN/m or kN/m/m].</param>
        /// <param name="x_pos">Kx' tension [kN/m or kN/m/m].</param>
        /// <param name="y_neg">Ky' compression [kN/m or kN/m/m].</param>
        /// <param name="y_pos">Ky' tension [kN/m or kN/m/m].</param>
        /// <param name="z_neg">Kz' compression [kN/m or kN/m/m].</param>
        /// <param name="z_pos">Kz' tension [kN/m or kN/m/m].</param>
        public static Motions Define(double x_neg = 0, double x_pos = 0, double y_neg = 0, double y_pos = 0, double z_neg = 0, double z_pos = 0)
        {
            return new Motions(x_neg, x_pos, y_neg, y_pos, z_neg, z_pos);
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
            return new Motions(0,0,0,0,0,0);
        }
    }
}