// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class RectangleType
    {
        [XmlElement("base_corner", Order = 1)]
        public FdPoint3d BaseCorner { get; set; }

        [XmlElement("x_direction", Order = 2)]
        public FdVector3d LocalX { get; set; }

        [XmlElement("y_direction", Order = 3)]
        public FdVector3d LocalY { get; set; }

        [XmlAttribute("x_size")]
        public double DimX {get; set; }

        [XmlAttribute("y_size")]
        public double DimY { get; set; }
    }
}
