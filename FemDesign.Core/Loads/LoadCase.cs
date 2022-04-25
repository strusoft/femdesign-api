// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case_type
    /// </summary>
    [System.Serializable]
    public partial class LoadCase
    {

        [XmlAttribute("guid")]
        public string _guid 
        { 
            get { return IndexedGuid.ToString(); }
            set { IndexedGuid = new IndexedGuid(value); }
        }

        [XmlIgnore]
        public IndexedGuid IndexedGuid { get; set; }

        #region EntityBase

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

        /// <summary>
        /// Invoke when an instance is created.
        /// 
        /// Creates a new guid, adds timestamp and changes action.
        /// </summary>
        public void EntityCreated()
        {
            this.IndexedGuid = System.Guid.NewGuid();
            this.LastChange = DateTime.UtcNow;
            this.Action = "added";
        }

        /// <summary>
        /// Invoke when an instance is modified.
        /// 
        /// Changes timestamp and action.
        /// </summary>
        public void EntityModified()
        {
            this.LastChange = DateTime.UtcNow;
            this.Action = "modified";
        }

        #endregion

        // attributes
        [XmlAttribute("name")]
        public string Name { get; set; } // name79
        [XmlAttribute("type")]
        public LoadCaseType Type { get; set; } // loadcasetype_type
        [XmlAttribute("duration_class")]
        public LoadCaseDuration DurationClass { get; set; } // loadcasedurationtype

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCase()
        {

        }

        /// <summary>
        /// Create a LoadCase
        /// </summary>
        /// <param name="name">Name/Identifier of LoadCase.</param>
        /// <param name="type">One of "static", "dead_load", "shrinkage", "seis_max", "seis_sxp", "seis_sxm", "seis_syp", "seis_sym", "soil_dead_load", "prestressing", "fire", "deviation", "notional".</param>
        /// <param name="durationClass">One of "permanent", "long-term", "medium-term", "short-term", "instantaneous".</param>
        public LoadCase(string name, LoadCaseType type, LoadCaseDuration durationClass)
        {
            this.EntityCreated();
            this.Name = name;
            this.Type = type;
            this.DurationClass = durationClass;
        }

        /// <summary>
        /// Returns a LoadCase from a list of LoadCases by name. The first LoadCase with a matching name will be returned.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="loadCases">List of LoadCase.</param>
        /// <param name="name">Name of LoadCase.</param>
        public static LoadCase LoadCaseFromListByName(List<LoadCase> loadCases, string name)
        {
            foreach (LoadCase _loadCase in loadCases)
            {
                if (_loadCase.Name == name)
                {
                    return _loadCase;
                }
            }
            return null;
        }
    }
}