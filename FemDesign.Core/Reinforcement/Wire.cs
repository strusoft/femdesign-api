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
        public string _profile; // wire_profile_type
        [XmlIgnore]
        public string Profile
        {
            get {return this._profile;}
            set {this._profile = RestrictedString.WireProfileType(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Wire()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        public Wire(double diameter, Materials.Material reinforcingMaterial, string profile)
        {
            this.ReinforcingMaterial = reinforcingMaterial;
            this.Diameter = diameter;
            this.ReinforcingMaterialGuid = reinforcingMaterial.Guid;
            this.Profile = profile;
        }
    }
}