using System;
using System.Xml.Serialization;


namespace FemDesign.Shells
{
    [System.Serializable]
    public partial class ShellBucklingType: EntityBase
    {
        [XmlElement("direction", Order = 1)]
        public Geometry.Vector3d LocalX { get; set; }

        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }

        [XmlAttribute("beta")]
        public double Beta { get; set; }
    }
}