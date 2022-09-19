using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class PunchingArea: EntityBase
    {
        [XmlElement("base_shell", Order = 1)]
        public GuidListType BaseShell { get; set; }

        [XmlElement("connected_bar", Order = 2)]
        public GuidListType[] ConnectedBar { get; set; }

        [XmlElement("local_pos", Order = 3)]
        public Geometry.Point3d LocalPos { get; set; }

        [XmlElement("local_x", Order = 4)]
        public Geometry.Vector3d LocalX { get; set; }

        [XmlElement("local_y", Order = 5)]
        public Geometry.Vector3d LocalY { get; set; }

        [XmlElement("reference_points_offset", Order = 6)]
        public Geometry.Point3d RefPointsOffset { get; set; }

        [XmlElement("region", Order = 7)]
        public Geometry.Region Region { get; set; }

        [XmlAttribute("downward")]
        public bool Downward { get; set; }

        [XmlAttribute("manual_design")]
        public bool ManualDesign { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
