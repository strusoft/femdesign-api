// https://strusoft.com/

using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_type
    /// </summary>
    [System.Serializable]
    public partial class LoadType : EntityBase
    {
        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        [XmlAttribute("country")]
        public string Country { get; set; } // eurocodetype
        /// <summary>
        /// Name of Load Type.
        /// </summary>
        /// <value></value>
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("psi_0")]
        public double Psi0 { get; set; }
        [XmlElement("psi_1")]
        public double Psi1 { get; set; }
        [XmlElement("psi_2")]
        public double Psi2 { get; set; }
    }
}
