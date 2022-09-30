// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_combination_type
    /// </summary>
    [System.Serializable]
    public partial class LoadCombination : EntityBase
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
        public LoadCombination(string name, LoadCombType type, List<LoadCase> loadCases, List<double> gammas, Calculate.CombItem combItem = null)
        {
            Initialize(name, type, combItem);

            if (loadCases.Count == gammas.Count)
                for (int i = 0; i < loadCases.Count; i++)
                    this.AddLoadCase(loadCases[i], gammas[i]);
            else
                throw new System.ArgumentException("loadCase and gamma must have equal length");

        }

        public LoadCombination(string name, LoadCombType type, params (LoadCase lc, double gamma)[] values)
        {
            Initialize(name, type);

            foreach (var (lc, gamma) in values)
                this.AddLoadCase(lc, gamma);
        }

        private void Initialize(string name, LoadCombType type, Calculate.CombItem combItem = null)
        {
            this.EntityCreated();
            this.Name = name;
            this.Type = type;
            this.SetCalculationSettings(combItem);
        }

        /// <summary>
        /// Set the load combination-specific calculation settings. This is known as "Setup by load combinations" in FEM-Design GUI.
        /// </summary>
        /// <param name="combItem">Load combination-specific settings. The default settings will be used if the value is null.</param>
        public void SetCalculationSettings(Calculate.CombItem combItem)
        {
            this.CombItem = combItem ?? Calculate.CombItem.Default();
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
                this.ModelLoadCase.Add(new ModelLoadCase(loadCase, gamma));
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

        public override string ToString()
        {
            const int space = -10;
            const int caseNameSpace = -12;
            const int gammaSpace = -3;
            var repr = "";
            repr += $"{this.Name,space} {this.Type}\n";
            foreach (var item in this.ModelLoadCase)
            {
                if(item.LoadCase == null) { return base.ToString(); } // Deserialisation can not get the loadcase name from the object. Only the GUID
                else
                    repr += $"{"",space - 1}{item.LoadCase.Name,caseNameSpace} {item.Gamma,gammaSpace}\n";
            }
            
            return repr;
        }
    }
}