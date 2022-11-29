// https://strusoft.com/
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// load_base_attribs
    /// </summary>
    [System.Serializable]
    public partial class LoadBase: EntityBase, ILoadElement
    {
        [XmlAttribute("load_case")]
        public System.Guid LoadCaseGuid { get; set; } // load_case_id
        [XmlAttribute("comment")]
        public string Comment { get; set; } // comment_string

        [XmlIgnore] // force_load_type
        public Loads.LoadCase LoadCase { get; set; }

    }
}