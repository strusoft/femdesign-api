// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_combination_type
    /// </summary>
    public class LoadCombination: EntityBase
    {
        [XmlAttribute("name")]
        public string name { get; set; } // name159
        [XmlAttribute("type")]
        public string _type; // loadcombtype 
        [XmlIgnore]
        public string type
        {
            get {return this._type;}
            set {this._type = RestrictedString.LoadCombType(value);}
        }
        [XmlElement("load_case")]
        public List<ModelLoadCase> modelLoadCase = new List<ModelLoadCase>(); // sequence: ModelLoadCase
        [XmlIgnore]
        public Calculate.CombItem combItem { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCombination()
        {

        }
        
        /// <summary>
        /// Internal constructor. Used for GH components and Dynamo nodes.
        /// </summary>
        internal LoadCombination(string name, string type, List<LoadCase> loadCase, List<double> gamma)
        {
            this.EntityCreated();
            this.name = name;
            this.type = type;
            

            if (loadCase.GetType() == typeof(List<LoadCase>) && gamma.GetType() == typeof(List<double>))
            {
                List<LoadCase> loadCases = (List<LoadCase>)loadCase;
                List<double> gammas = (List<double>)gamma;

                if (loadCases.Count == gammas.Count)
                {
                    for (int i = 0; i < loadCases.Count; i++)
                    {
                        this.AddLoadCase(loadCases[i], gammas[i]);
                    }
                }
                else
                {
                    throw new System.ArgumentException("loadCase and gamma must have equal length");
                }
            }
            else
            {
                throw new System.ArgumentException("loadCase must be Loads.LoadCase or List<Loads.LoadCase>, gamma must be double or List<double>");
            }          
        }

        /// <summary>
        /// Get LoadCase guids of LoadCases in LoadCombination.
        /// </summary>
        internal List<string> GetLoadCaseGuidsAsString()
        {
            var loadCaseGuids = new List<string>();
            foreach (ModelLoadCase item in this.modelLoadCase)
            {
                loadCaseGuids.Add(item.guid.ToString());
            }
            return loadCaseGuids;
        }

        /// <summary>
        /// Get gamma values of LoadCases in LoadCombanation.
        /// </summary>
        internal List<double> GetGammas()
        {
            var gammas = new List<double>();
            foreach (ModelLoadCase item in this.modelLoadCase)
            {
                gammas.Add(item.gamma);
            }
            return gammas;
        }

        /// <summary>
        /// Add LoadCase to LoadCombination.
        /// </summary>
        private void AddLoadCase(LoadCase loadCase, double gamma)
        {
            if (this.LoadCaseInLoadCombination(loadCase))
            {
                // pass
            }
            else
            {
               this.modelLoadCase.Add(new ModelLoadCase(loadCase.guid, gamma)); 
            }         
        }

        /// <summary>
        /// Check if LoadCase in LoadCombination.
        /// </summary>
        private bool LoadCaseInLoadCombination(LoadCase loadCase)
        {
            foreach (ModelLoadCase elem in this.modelLoadCase)
            {
                if (elem.guid == loadCase.guid)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}