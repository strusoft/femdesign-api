// https://strusoft.com/
using FemDesign.GenericClasses;


namespace FemDesign.Shells
{
    public partial class ShellEccentricity
    {
        public VerticalAlignment Alignment { get; set; }
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
        
        public ShellEccentricity(VerticalAlignment alignment, double eccentricity, bool eccentricityCalculation, bool eccentricityByCracking)
        {
            this.Alignment = alignment;
            this.Eccentricity = eccentricity;
            this.EccentricityCalculation = eccentricityCalculation;
            this.EccentricityByCracking = eccentricityByCracking;
        }

        /// <summary>
        /// Create a default ShellEccentricity.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static ShellEccentricity Default => new ShellEccentricity(VerticalAlignment.Center, 0, false, false);

        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Eccentricity} m";
        }
    }
}