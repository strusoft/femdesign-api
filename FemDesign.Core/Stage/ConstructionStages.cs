using System;
using System.Xml.Serialization;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// Stage
    /// </summary>
    [System.Serializable]
    public partial class ConstructionStages : EntityBase, IFemDesignEntity
    {
        [XmlAttribute("last_change")]
        public string _lastChange;
        [XmlIgnore]
        internal DateTime LastChange
        {
            get
            {
                return DateTime.Parse(this._lastChange);
            }
            set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }
        [XmlAttribute("action")]
        public string Action { get; set; }

        [XmlAttribute("auto-assign_modified_elements")]
        public bool AssignModifiedElement { get; set; } = false;

        [XmlAttribute("auto-assign_newly_created_elements")]
        public bool AssignNewElement { get; set; } = false;

        [XmlAttribute("ghost_method")]
        public bool GhostMethod { get; set; } = false;

        [XmlElement("stage")]
        public List<Stage> Stages { get; set; }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ConstructionStages()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stages"></param>
        /// <param name="assignModifedElement"></param>
        /// <param name="assignNewElement"></param>
        /// <param name="ghostMethod"></param>
        public ConstructionStages(List<Stage> stages, bool assignModifedElement, bool assignNewElement, bool ghostMethod)
        {
            // it does not required the Guid
            this.LastChange = DateTime.UtcNow;
            this.Action = "added";
            this.Stages = stages;
            this.AssignModifiedElement = assignModifedElement;
            this.AssignNewElement = assignNewElement;
            this.GhostMethod = ghostMethod;
        }

}
}
