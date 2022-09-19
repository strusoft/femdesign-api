using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    [System.Serializable]
    public partial class ColumnCorbel: EntityBase
    {
        [XmlElement("connectable_parts", Order = 1)]
        public ThreeGuidListType ConnectableParts { get; set; }

        [XmlElement("connectivity", Order = 2)]
        public Connectivity Connectivity { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlIgnore]
        public string Instance
        {
            get
            {
                var found = this.Name.IndexOf(".");
                return this.Name.Substring(found + 1);
            }
        }
        public string Identifier => this.Name.Split('.')[0];

        [XmlAttribute("base_column")]
        public System.Guid BaseColumn { get; set; }

        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial { get; set; }

        [XmlAttribute("made")]
        public string Made { get; set; }

        [XmlAttribute("complex_section")]
        public System.Guid ComplexSection { get; set; }

        [XmlAttribute("pos")]
        public double Position { get; set; }

        [XmlAttribute("alpha")]
        public double Alpha { get; set; }

        [XmlAttribute("d")]
        public double D { get; set; }

        [XmlAttribute("l")]
        public double L { get; set; }

        [XmlAttribute("e")]
        public double E { get; set; }

        [XmlAttribute("x")]
        public double X { get; set; }

        [XmlAttribute("y")]
        public double Y { get; set; }
    }
}