// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// load_base_attribs
    /// </summary>
    [System.Serializable]
    public class LoadBase: EntityBase
    {
        [XmlAttribute("load_case")]
        public System.Guid loadCase { get; set; } // load_case_id
        [XmlAttribute("comment")]
        public string comment { get; set; } // comment_string
    }
}