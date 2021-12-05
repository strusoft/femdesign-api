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
        public ModelLoadCaseInGroup()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="guid">LoadCase guid reference.</param>
        public ModelLoadCaseInGroup(System.Guid guid)
        {
            this.Guid = guid;
        }     
    }
}