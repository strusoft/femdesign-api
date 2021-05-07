// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// load_base_attribs
    /// </summary>
    [System.Serializable]
    public partial class LoadBase: EntityBase
    {
        [XmlAttribute("load_case")]
        public System.Guid LoadCase { get; set; } // load_case_id
        [XmlAttribute("comment")]
        public string Comment { get; set; } // comment_string
    }
}