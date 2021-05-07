// https://strusoft.com/

using System.Globalization;
using System.Xml.Serialization;


namespace FemDesign.Releases
{
    /// <summary>
    /// rigidity_data_type3
    /// </summary>
    [System.Serializable]
    public partial class RigidityDataType3: RigidityDataType2
    {
        /// <summary>
        /// Type string in order to make field nullable. When null FEM-Design will load default value.
        /// </summary>
        [XmlAttribute("friction")]
        public string _friction; // reduction_factor_type. Default = 0.3
        [XmlIgnore]
        public double Friction 
        {
            get
            {
                if (this._friction == null)
                {
                    throw new System.ArgumentException("_friction is null");
                }
                else
                {    
                    return System.Convert.ToDouble(this._friction, CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this._friction = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private RigidityDataType3()
        {

        }

        /// <summary>
        /// Construct RigidityDataType3 with default friction
        /// </summary>
        public RigidityDataType3(Motions motions, Rotations rotations)
        {
            this.Motions = motions;
            this.Rotations = rotations;
        }

        /// <summary>
        /// Construct RigidityDataType3 with defined friction
        /// </summary>
        public RigidityDataType3(Motions motions, Rotations rotations, double friction)
        {
            this.Motions = motions;
            this.Rotations = rotations;
            this.Friction = friction;
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