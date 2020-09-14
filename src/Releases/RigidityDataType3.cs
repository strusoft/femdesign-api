// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// rigidity_data_type3
    /// </summary>
    [System.Serializable]
    public class RigidityDataType3: RigidityDataType2
    {
        [XmlAttribute("friction")]
        public double _friction; // reduction_factor_type. Default = 0.3
        [XmlIgnore]
        public double Friction 
        {
            get {return this._friction;}
            set {this._friction = RestrictedDouble.NonNegMax_1(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private RigidityDataType3()
        {

        }

        /// <summary>
        /// Private constructor of simple RigidityDataType3
        /// </summary>
        private RigidityDataType3(Motions motions, Rotations rotations)
        {
            this.Motions = motions;
            this.Rotations = rotations;
        }

        /// <summary>
        /// Construct simple RigidityDataType3 (linear stiffness).
        /// </summary>
        public static RigidityDataType3 Define(Motions motions, Rotations rotations)
        {
            return new RigidityDataType3(motions, rotations);
        }

        /// <summary>
        /// Construct simple hinged line RidigityDataType3.
        /// </summary>
        public static RigidityDataType3 HingedLine()
        {
            return new RigidityDataType3(Motions.RigidLine(), Rotations.Free());
        }

        /// <summary>
        /// Construct simple rigid line RigidityDataType3.
        /// </summary>
        public static RigidityDataType3 RigidLine()
        {
            return new RigidityDataType3(Motions.RigidLine(), Rotations.RigidLine());
        }


    }
}