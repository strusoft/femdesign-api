using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ShellBucklingType: EntityBase
    {
        [XmlElement("direction", Order = 1)]
        public Geometry.FdVector3d LocalX { get; set; }

        [XmlElement("contour", Order = 2)]
        public Geometry.Contour Contour { get; set; }

        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }

        [XmlAttribute("beta")]
        public double Beta { get; set; }
    }
}