using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class BarReinforcement: EntityBase
    {
        [XmlElement("base_bar", Order = 1)]
        public GuidListType BaseBar { get; set; }

        [XmlElement("wire", Order = 2)]
        public Wire Wire { get; set; }

        // choice stirrups
        [XmlElement("stirrups", Order = 3)]
        public Stirrups Stirrups { get; set; }

        // choice longitudinal bar
        [XmlElement("longitudinal_bar", Order = 4)]
        public LongitudinalBar LongitudinalBar { get; set; }
    }
}