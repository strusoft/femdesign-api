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
    public class Wire
    {
        [XmlIgnore]
        private Materials.Material _reinforcingMaterial;
        [XmlIgnore]
        public Materials.Material reinforcingMaterial
        {
            get
            {
                return this._reinforcingMaterial;
            }
            set
            {
                if (value.reinforcingSteel == null)
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
        public double diameter
        {
            get {return this._diameter;}
            set {this._diameter = RestrictedDouble.RcDiameterValue(value);}
        }
        [XmlAttribute("reinforcing_material")]
        public System.Guid reinforcingMaterialGuid { get; set; } // guidtype
        [XmlAttribute("profile")]
        public string _profile; // wire_profile_type
        [XmlIgnore]
        public string profile
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
        private Wire(double diameter, Materials.Material reinforcingMaterial, string profile)
        {
            this.reinforcingMaterial = reinforcingMaterial;
            this.diameter = diameter;
            this.reinforcingMaterialGuid = reinforcingMaterial.guid;
            this.profile = profile;
        }
        
        /// <summary>
        /// Define a reinforcement bar (wire) for a normal reinforcement layout.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="diameter">Diameter of reinforcement bar.</param>
        /// <param name="reinforcingMaterial">Material. Material of reinforcement bar.</param>
        /// <param name="profile">"smooth"/"ribbed"</param>
        /// <returns></returns>
        public static Wire Define(double diameter, Materials.Material reinforcingMaterial, string profile = "ribbed")
        {
            return new Wire(diameter, reinforcingMaterial, profile);
        }
    }
}