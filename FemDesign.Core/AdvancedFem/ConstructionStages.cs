using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FemDesign
{
    /// <summary>
    /// Stage
    /// </summary>
    [System.Serializable]
    public partial class ConstructionStages
    {
        [XmlElement("stage")]
        public List<Stage> Stages { get; set; }

        [XmlAttribute("auto-assign_modified_elements")]
        public bool AssignModifiedElement { get; set; } = false;

        [XmlAttribute("auto-assign_newly_created_elements")]
        public bool AssignNewElement { get; set; } = false;

        [XmlAttribute("ghost_method")]
        public bool GhostMethod { get; set; } = false;



}
}
