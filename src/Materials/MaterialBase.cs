// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class MaterialBase
    {
        // material_type_attribs
        [XmlAttribute("mass")]
        public string Mass { get; set; } // non_neg_max_1e20
        [XmlAttribute("E_0")]
        public string E_0  { get; set; } // double
        [XmlAttribute("E_1")]
        public string E_1 { get; set; } // double
        [XmlAttribute("E_2")]
        public string E_2 { get; set; } // double
        [XmlAttribute("nu_0")]
        public string nu_0 { get; set; } // double
        [XmlAttribute("nu_1")]
        public string nu_1 { get; set; } // double
        [XmlAttribute("nu_2")]
        public string nu_2 { get; set; } // double
        [XmlAttribute("alfa_0")]
        public string alfa_0 { get; set; } // double
        [XmlAttribute("alfa_1")]
        public string alfa_1 { get; set; } // double
        [XmlAttribute("alfa_2")]
        public string alfa_2 { get; set; } // double
        [XmlAttribute("G_0")]
        public string G_0 { get; set; } // double
        [XmlAttribute("G_1")]
        public string G_1 { get; set; } // double
        [XmlAttribute("G_2")]
        public string G_2 { get; set; } // double
    }
}