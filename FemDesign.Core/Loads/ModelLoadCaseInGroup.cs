// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case (child of load_group_table_type)
    /// </summary>
    [System.Serializable]
    public partial class ModelLoadCaseInGroup
    {
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; } // common_load_case --> guidtype indexed_guid
        [XmlIgnore]
        public LoadGroupBase LoadGroup { get; set; }

        /// parameterless constructor for serialization
        private ModelLoadCaseInGroup() { }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="guid">LoadCase guid reference.</param>
        /// <param name="parentLoadGroup"></param>
        public ModelLoadCaseInGroup(System.Guid guid, LoadGroupBase parentLoadGroup)
        {
            this.Guid = guid;
            LoadGroup = parentLoadGroup;
        }     
    }
}