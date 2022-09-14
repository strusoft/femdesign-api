// https://strusoft.com/


namespace FemDesign.Shells
{
    /// <summary>
    /// Not a strusoft.xsd type.
    /// </summary>
    public partial class ShellOrthotropy
    {
        private double _orthoAlfa; // two_quadrants.
        public double OrthoAlfa
        {
            get {return this._orthoAlfa;}
            set {this._orthoAlfa = RestrictedDouble.TwoQuadrantsRadians(value);}
        }
        private double _orthoRatio; // orthotropy_type
        public double OrthoRatio
        {
            get {return this._orthoRatio;}
            set {this._orthoRatio = RestrictedDouble.NonNegMax_1(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ShellOrthotropy()
        {

        }
        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="orthoAlfa">Degrees</param>
        /// <param name="orthoRatio">Double</param>
        public ShellOrthotropy(double orthoAlfa, double orthoRatio)
        {
            this.OrthoAlfa = FemDesign.Geometry.Degree.ToRadians(orthoAlfa);
            this.OrthoRatio = orthoRatio;
        }
        

        /// <summary>
        /// Create a default definition for ShellOrthotropy.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static ShellOrthotropy Default => new ShellOrthotropy(0, 1);
    }
}