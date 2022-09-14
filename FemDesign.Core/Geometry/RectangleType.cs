// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class RectangleType
    {
        [XmlElement("base_corner", Order = 1)]
        public Point3d BaseCorner { get; set; }

        [XmlElement("x_direction", Order = 2)]
        public Vector3d LocalX { get; set; }

        [XmlElement("y_direction", Order = 3)]
        public Vector3d LocalY { get; set; }

        [XmlAttribute("x_size")]
        public double DimX {get; set; }

        [XmlAttribute("y_size")]
        public double DimY { get; set; }
    }
}
