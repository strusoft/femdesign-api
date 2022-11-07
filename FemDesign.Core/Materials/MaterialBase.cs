// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Materials
{
    [System.Serializable]
    public partial class MaterialBase
    {
        // material_type_attribs

        [XmlAttribute("mass")]
        public double _mass; // non_neg_max_1e20
        [XmlIgnore]
        public double Mass
        {
            get { return _mass; }
            set { RestrictedDouble.NonNegMax_1e20(value); } // non_neg_max_1e20
        }

        [XmlAttribute("E_0")]
        public double E_0  { get; set; }
        [XmlAttribute("E_1")]
        public double E_1 { get; set; }
        [XmlAttribute("E_2")]
        public double E_2 { get; set; }
        [XmlAttribute("nu_0")]
        public double nu_0 { get; set; }
        [XmlAttribute("nu_1")]
        public double nu_1 { get; set; }
        [XmlAttribute("nu_2")]
        public double nu_2 { get; set; }
        [XmlAttribute("alfa_0")]
        public double alfa_0 { get; set; }
        [XmlAttribute("alfa_1")]
        public double alfa_1 { get; set; }
        [XmlAttribute("alfa_2")]
        public double alfa_2 { get; set; }
        [XmlAttribute("G_0")]
        public double G_0 { get; set; }
        [XmlAttribute("G_1")]
        public double G_1 { get; set; }
        [XmlAttribute("G_2")]
        public double G_2 { get; set; }
    }
}