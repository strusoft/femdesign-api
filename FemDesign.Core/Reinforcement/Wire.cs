// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// rf_wire_type
    /// 
    /// Reinforcement wire
    /// </summary>
    [System.Serializable]
    public partial class Wire
    {
        [XmlIgnore]
        private Materials.Material _reinforcingMaterial;
        [XmlIgnore]
        public Materials.Material ReinforcingMaterial
        {
            get
            {
                return this._reinforcingMaterial;
            }
            set
            {
                if (value.ReinforcingSteel == null)
                {
                    throw new System.ArgumentException("Material must be a reinforcing material.");
                }
                else
                {
                    this._reinforcingMaterial = value;
                }
            }
        }
        [XmlAttribute("diameter")]
        public double _diameter; // rc_diameter_value
        [XmlIgnore]
        public double Diameter
        {
            get {return this._diameter;}
            set {this._diameter = RestrictedDouble.RcDiameterValue(value);}
        }
        [XmlAttribute("reinforcing_material")]
        public System.Guid ReinforcingMaterialGuid { get; set; } // guidtype
        [XmlAttribute("profile")]
        public WireProfileType Profile { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Wire()
        {

        }

        /// <summary>
        /// Reinforcement wire.
        /// </summary>
        public Wire(double diameter, Materials.Material reinforcingMaterial, WireProfileType profile)
        {
            this.ReinforcingMaterial = reinforcingMaterial;
            this.Diameter = diameter;
            this.ReinforcingMaterialGuid = reinforcingMaterial.Guid;
            this.Profile = profile;
        }
    }
}