// https://strusoft.com/

using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case_type
    /// </summary>
    [System.Serializable]
    public partial class LoadCase: EntityBase
    {
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

        public override string ToString()
        {
            return $"{this.GetType().Name} ID: {this.Name}, Type: {this.Type}, Duration: {this.DurationClass}";
        }
    }
}