using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{
    [System.Serializable()]
    public partial class CltDataType
    {
        [XmlElement("default_kdef", Order = 1)]
        public TimberServiceClassKdfes DefaultKdef { get; set; }

        [XmlElement("layer", Order = 2)]
        public List<LimitStresses> Layers { get; set; }

        [XmlAttribute("manufacturer")]
        public string Manufacturer { get; set; }

        [XmlAttribute("r33")]
        public double R33 { get; set; }

        [XmlAttribute("r66")]
        public double R66 { get; set; }

        [XmlAttribute("r77")]
        public double R77 { get; set; }

        [XmlAttribute("r88")]
        public double R88 { get; set; }

        public CltDataType()
        {

        }
    }
}