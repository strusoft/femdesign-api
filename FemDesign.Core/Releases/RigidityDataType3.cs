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
        public RigidityDataType3(Motions motions, Rotations rotations) : base(motions, rotations)
        {
        }

        /// <summary>
        /// Construct RigidityDataType3 with default friction
        /// </summary>
        public RigidityDataType3(Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits) : base(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits)
        {
        }

        /// <summary>
        /// Construct RigidityDataType3 with defined friction
        /// </summary>
        public RigidityDataType3(Motions motions, Rotations rotations, double friction) : base(motions, rotations)
        {
            this.Friction = friction;
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