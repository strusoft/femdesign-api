// https://strusoft.com/

using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class EdgeConnectionBase: EntityBase
    {
        [XmlAttribute("moving_local")]
        public bool movingLocal { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
        
        [XmlAttribute("joined_start_point")]
        public bool joinedStartPoint { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
        
        [XmlAttribute("joined_end_point")]
        public bool joinedEndPoint { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
    }
}