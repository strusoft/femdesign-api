using System;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class StartEndType
    {
        [XmlAttribute("start")]
        public double Start { get; set; }

        [XmlAttribute("end")]
        public double End { get; set; }
    }
}