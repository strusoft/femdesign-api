// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// material_type --> steel
    /// </summary>
    [System.Serializable]
    public partial class Steel: MaterialBase
    {
        [XmlAttribute("Fyk16")]
        public string Fyk16 { get; set; } // material_base_value
        [XmlAttribute("Fyk40")]
        public string Fyk40 { get; set; } // material_base_value
        [XmlAttribute("Fyk63")]
        public string Fyk63 { get; set; } // material_base_value
        [XmlAttribute("Fyk80")]
        public string Fyk80 { get; set; } // material_base_value
        [XmlAttribute("Fyk100")]
        public string Fyk100 { get; set; } // material_base_value
        [XmlAttribute("Fyk150")]
        public string Fyk150 { get; set; } // material_base_value
        [XmlAttribute("Fyk200")]
        public string Fyk200 { get; set; } // material_base_value
        [XmlAttribute("Fyk250")]
        public string Fyk250 { get; set; } // material_base_value
        [XmlAttribute("Fyk400")]
        public string Fyk400 { get; set; } // material_base_value
        [XmlAttribute("Fuk3")]
        public string Fuk3 { get; set; } // material_base_value
        [XmlAttribute("Fuk40")]
        public string Fuk40 { get; set; } // material_base_value
        [XmlAttribute("Fuk100")]
        public string Fuk100 { get; set; } // material_base_value
        [XmlAttribute("Fuk150")]
        public string Fuk150 { get; set; } // material_base_value
        [XmlAttribute("Fuk250")]
        public string Fuk250 { get; set; } // material_base_value
        [XmlAttribute("Fuk400")]
        public string Fuk400 { get; set; } // material_base_value
        [XmlAttribute("gammaM0_0")]
        public string gammaM0_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM0_1")]
        public string gammaM0_1 { get; set; } // material_base_value
        [XmlAttribute("gammaM1_0")]
        public string gammaM1_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM1_1")]
        public string gammaM1_1 { get; set; } // material_base_value
        [XmlAttribute("gammaM2_0")]
        public string gammaM2_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM2_1")]
        public string gammaM2_1 { get; set; } // material_base_value
        [XmlAttribute("gammaM5_0")]
        public string gammaM5_0 { get; set; } // material_base_value
        [XmlAttribute("gammaM5_1")]
        public string gammaM5_1 { get; set; } // material_base_value
        [XmlAttribute("Ek")]
        public string Ek { get; set; } // material_base_value
        [XmlAttribute("Ed_0")]
        public string Ed_0 { get; set; } // double
        [XmlAttribute("Ed_1")]
        public string Ed_1 { get; set; } // double
        [XmlAttribute("nu")]
        public string nu { get; set; } // material_nu_value
        [XmlAttribute("G")]
        public string G { get; set; } // material_base_value
        [XmlAttribute("alfa")]
        public string alfa { get; set; } // material_base_value
    }
}