// https://strusoft.com/


namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> custom
    /// </summary>
    [System.Serializable]
    public partial class Custom: MaterialBase
    {
        internal Custom()
        {

        }
        public Custom(double mass, double e_0, double e_1, double e_2, double nu_0, double nu_1, double nu_2, double alfa_0, double alfa_1, double alfa_2, double G_0, double G_1, double G_2)
        {
            this.Mass = mass;
            this.E_0 = e_0;
            this.E_1 = e_1;
            this.E_2 = e_2;
            this.nu_0 = nu_0;
            this.nu_1 = nu_1;
            this.nu_2 = nu_2;
            this.alfa_0 = alfa_0;
            this.alfa_1 = alfa_1;
            this.alfa_2 = alfa_2;
            this.G_0 = G_0;
            this.G_1 = G_1;
            this.G_2 = G_2;
        }
        /// <summary>
        /// Create an Uniaxial Custom Material
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="e_0"></param>
        /// <param name="nu_0"></param>
        /// <param name="alfa_0"></param>
        public Custom(double mass, double e_0, double nu_0, double alfa_0)
        {
            this.Mass = mass;
            this.E_0 = e_0;
            this.E_1 = 0.0;
            this.E_2 = 0.0;
            this.nu_0 = nu_0;
            this.nu_1 = 0.0;
            this.nu_2 = 0.0;
            this.alfa_0 = alfa_0;
            this.alfa_1 = 0.0;
            this.alfa_2 = 0.0;
            this.G_0 = 0.0;
            this.G_1 = 0.0;
            this.G_2 = 0.0;
        }

    }
}