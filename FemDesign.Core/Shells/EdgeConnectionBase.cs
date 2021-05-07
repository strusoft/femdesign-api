// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Shells
{
    [System.Serializable]
    public partial class EdgeConnectionBase: EntityBase
    {
        [XmlAttribute("moving_local")]
        public bool MovingLocal { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
        
        [XmlAttribute("joined_start_point")]
        public bool JoinedStartPoint { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
        
        [XmlAttribute("joined_end_point")]
        public bool JoinedEndPoint { get; set; } // bool. Default false according to strusoft.xsd but true in GUI?
    }
}