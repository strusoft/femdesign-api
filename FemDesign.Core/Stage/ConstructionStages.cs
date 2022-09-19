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
    public partial class ConstructionStages
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
        public List<Stage> Stages { get; set; } = new List<Stage>();


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public ConstructionStages()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stages"></param>
        /// <param name="assignModifedElement"></param>
        /// <param name="assignNewElement"></param>
        /// <param name="ghostMethod"></param>
        public ConstructionStages(List<Stage> stages, bool assignModifedElement = false, bool assignNewElement = false, bool ghostMethod = false)
        {
            // it does not required the Guid
            this.LastChange = DateTime.UtcNow;
            this.Action = "added";
            this.Stages = ConstructionStages.SortStages(stages);
            this.AssignModifiedElement = assignModifedElement;
            this.AssignNewElement = assignNewElement;
            this.GhostMethod = ghostMethod;
        }

        private static List<Stage> SortStages(List<Stage> stages)
        {
            var orderedStages = stages.OrderBy(x => x.Id).ToList();
            return orderedStages;
        }

    }
}
