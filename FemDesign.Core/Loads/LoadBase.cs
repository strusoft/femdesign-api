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
        public System.Guid LoadCaseGuid { get; set; }

        [XmlAttribute("comment")]
        public string Comment { get; set; } // comment_string

        [XmlIgnore]
        public Loads.LoadCase _loadCase { get; set; }

        [XmlIgnore]
        public Loads.LoadCase LoadCase
        {
            get { return _loadCase; }
            set
            {
                _loadCase = value;
                LoadCaseGuid = value.Guid;
            }
        }
    }
}