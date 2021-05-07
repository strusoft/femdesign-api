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
        public string _type; // loadcasetype_type
        [XmlIgnore]
        public string Type
        {
            get {return this._type;}
            set {this._type = RestrictedString.LoadCaseType(value);}
        }
        [XmlAttribute("duration_class")]
        public string _durationClass; // loadcasedurationtype
        [XmlIgnore]
        public string DurationClass
        {
            get {return this._durationClass;}
            set {this._durationClass = RestrictedString.LoadCaseDurationType(value);}
        }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCase()
        {

        }
        
        /// <summary>
        /// Internal constructor.
        /// </summary>
        public LoadCase(string name, string type, string durationClass)
        {
            this.EntityCreated();
            this.Type = type;
            this.DurationClass = durationClass;
            this.Name = name;
        }

        /// <summary>
        /// Creates a load case.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="name">Name of LoadCase.</param>
        /// <param name="type">LoadCase type. "static"/"dead_load"/"soil_dead_load"/"shrinkage"/"prestressing"/"fire"/"seis_sxp"/"seis_sxm"/"seis_syp"/"seis_sym"</param>
        /// <param name="durationClass">LoadCase duration class. "permanent"/"long-term"/"medium-term"/"short-term"/"instantaneous"</param>
        /// <returns></returns>
        public static LoadCase CreateLoadCase(string name, string type = "static", string durationClass = "permanent")
        {
            LoadCase loadCase = new LoadCase(name, type, durationClass);
            return loadCase;
        }

        /// <summary>
        /// Returns a LoadCase from a list of LoadCases by name. The first LoadCase with a matching name will be returned.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="loadCases">List of LoadCase.</param>
        /// <param name="name">Name of LoadCase.</param>
        /// <returns></returns>
        public static LoadCase GetLoadCaseFromListByName(List<LoadCase> loadCases, string name)
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