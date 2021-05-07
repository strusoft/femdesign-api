// https://strusoft.com/


namespace FemDesign.Shells
{
    public partial class ShellEccentricity
    {
        private string _alignment; // ver_align
        public string Alignment
        {
            get {return this._alignment;}
            set {this._alignment = RestrictedString.VerticalAlign(value);}
        }
        private double _eccentricity; // align_offset // abs_max_1e20
        public double Eccentricity
        {
            get {return this._eccentricity;}
            set {this._eccentricity = RestrictedDouble.AbsMax_1e20(value);}
        }
        public bool EccentricityCalculation { get; set; } // bool
        public bool EccentricityByCracking { get; set; } // bool

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ShellEccentricity()
        {

        }
        
        public ShellEccentricity(string alignment, double eccentricity, bool eccentricityCalculation, bool eccentricityByCracking)
        {
            this.Alignment = alignment;
            this.Eccentricity = eccentricity;
            this.EccentricityCalculation = eccentricityCalculation;
            this.EccentricityByCracking = eccentricityByCracking;
        }

        /// <summary>
        /// Create a ShellEccentricity
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="alignment">Alignment. "top"/"bottom"/"center".</param>
        /// <param name="eccentricity">Eccentricity.</param>
        /// <param name="eccentricityCalculation">Consider eccentricity in calculation? True/false.</param>
        /// <param name="eccentricityByCracking">Consider eccentricity caused by cracking in cracked section analysis? True/false.</param>
        /// <returns></returns>
        public static ShellEccentricity Create(string alignment = "center", double eccentricity = 0, bool eccentricityCalculation = false, bool eccentricityByCracking = false)
        {
            return new ShellEccentricity(alignment, eccentricity, eccentricityCalculation, eccentricityByCracking);
        }

        /// <summary>
        /// Create a default ShellEccentricity.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static ShellEccentricity Default()
        {
            return new ShellEccentricity("center", 0, false, false);
        }
    }
}