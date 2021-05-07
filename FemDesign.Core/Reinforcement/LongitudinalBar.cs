using System;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public class LongitudinalBar
    {
        [XmlElement("cross-sectional_position", Order = 1)]
        public Geometry.FdPoint2d Position2d { get; set; }

        [XmlElement("anchorage", Order = 2)]
        public StartEndType Anchorage { get; set; }

        [XmlElement("prescribed_lengthening", Order = 3)]
        public StartEndType PrescribedLengthening { get; set; }

        [XmlAttribute("start")]
        public double Start { get; set; }

        [XmlAttribute("end")]
        public double End { get; set; }

        [XmlAttribute("auxiliary")]
        public bool Auxiliary { get; set; }
    }
}