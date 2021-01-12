using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Stirrups
    {
        [XmlElement("region", Order = 1)]
        public Geometry.Region[] Regions { get; set; }

        [XmlAttribute("start")]
        public double Start { get; set; }

        [XmlAttribute("end")]
        public double End { get; set; }

        [XmlAttribute("distance")]
        public double Distance { get; set; }
    }
}