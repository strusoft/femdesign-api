// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_combination_type
    /// </summary>
    [System.Serializable]
    public partial class LoadCombination: EntityBase
    {
        [XmlAttribute("name")]
        public string Name { get; set; } // name159
        [XmlAttribute("type")]
        public LoadCombType Type { get; set; } // loadcombtype 
        [XmlElement("load_case")]
        public List<ModelLoadCase> ModelLoadCase = new List<ModelLoadCase>(); // sequence: ModelLoadCase
        [XmlIgnore]
        public Calculate.CombItem CombItem { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadCombination()
        {

        }
        
        /// <summary>
        /// Internal constructor. Used for GH components and Dynamo nodes.
        /// </summary>
        public LoadCombination(string name, LoadCombType type, List<LoadCase> loadCase, List<double> gamma)
        {
            this.EntityCreated();
            this.Name = name;
            this.Type = type;
            

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
        public List<string> GetLoadCaseGuidsAsString()
        {
            var loadCaseGuids = new List<string>();
            foreach (ModelLoadCase item in this.ModelLoadCase)
            {
                loadCaseGuids.Add(item.Guid.ToString());
            }
            return loadCaseGuids;
        }

        /// <summary>
        /// Get gamma values of LoadCases in LoadCombination.
        /// </summary>
        public List<double> GetGammas()
        {
            var gammas = new List<double>();
            foreach (ModelLoadCase item in this.ModelLoadCase)
            {
                gammas.Add(item.Gamma);
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
               this.ModelLoadCase.Add(new ModelLoadCase(loadCase.Guid, gamma)); 
            }         
        }

        /// <summary>
        /// Check if LoadCase in LoadCombination.
        /// </summary>
        private bool LoadCaseInLoadCombination(LoadCase loadCase)
        {
            foreach (ModelLoadCase elem in this.ModelLoadCase)
            {
                if (elem.Guid == loadCase.Guid)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}