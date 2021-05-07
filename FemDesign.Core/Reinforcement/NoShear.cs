using System;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class NoShearAutoType
    {
        [XmlAttribute("connected_structure")]
        public Guid ConnectedStructure { get; set; }

        [XmlAttribute("factor")]
        public double Factor { get; set; }

        [XmlAttribute("inactive")]
        public bool Inactive { get; set; }
    }

    [System.Serializable]
    public partial class NoShearRegionType: EntityBase
    {
        [XmlElement("automatic", Order = 1)]
        public NoShearAutoType Automatic { get; set; }

        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        [XmlAttribute("base_plate")]
        public Guid BasePlate { get; set; }
    }
}
