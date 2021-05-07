using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// tp_datatype
    /// </summary>
    [System.Serializable]
    public partial class TimberPanelData
    {
        [XmlElement("stiffness", Order=1)]
        public TimberPanelStiffness Stiffness { get; set; }

        [XmlElement("strength", Order=2)]
        public TimberPanelStrength Strength { get; set; }

        [XmlElement("service_class_0_factors", Order=3)]
        public ServiceClassFactors ServiceClassFactors0 { get; set;}

        [XmlElement("service_class_1_factors", Order=4)]
        public ServiceClassFactors ServiceClassFactors1 { get; set;}

        [XmlElement("service_class_2_factors", Order=5)]
        public ServiceClassFactors ServiceClassFactors2 { get; set;}

        [XmlAttribute("description")]
        public string _name;
        [XmlIgnore]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = RestrictedString.Length(value, 40);
            }
        }
        
        [XmlAttribute("thickness")]
        public double _thickness;
        [XmlIgnore]
        public double Thickness
        {
            get
            {
                return this._thickness;
            }
            set
            {
                this._thickness = RestrictedDouble.TimberPanelThickness(value);
            }
        }


    }
}