// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    /// <summary>
    /// load_combination_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
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
        
        #region dynamo
        /// <summary>Create LoadCombination from a LoadCase or a list of LoadCases.</summary>
        /// <remarks>Create</remarks>
        /// <param name="name">Name of LoadCombination</param>
        /// <param name="type">LoadCombination type. "ultimate_ordinary"/"ultimate_accidental"/"ultimate_seismic"/"serviceability_quasi_permanent"/"serviceability_frequent"/"serviceability_characteristic"</param>
        /// <param name="loadCase">LoadCase to include in load combination. Single LoadCase or list of LoadCases. Nested lists are not supported - use flatten.</param>
        /// <param name="gamma">Gamma value for respective LoadCase. Single value or list of values. Nested lists are not supported - use flatten.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LoadCombination CreateLoadCombination(string name, [DefaultArgument("ultimate_ordinary")] string type, List<LoadCase> loadCase, List<double> gamma)
        {
            LoadCombination loadCombination = new LoadCombination(name, type, loadCase, gamma);
            return loadCombination;
        }
        /// <summary>
        /// Set calculation parameters for a specific load combination.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="ImpfRqd">Required imperfection shapes</param>
        /// <param name="StabRqd">Required buckling shapes for stability analysis</param>
        /// <param name="NLE">Consider elastic non-linear behaviour of structural elements</param>
        /// <param name="PL">Consider plastic behaviour of structural elements</param>
        /// <param name="NLS">Consider non-linear behaviour of soil</param>
        /// <param name="Cr">Cracked section analysis. Note that Cr only executes properly in RCDesign with DesignCheck set to true.</param>
        /// <param name="f2nd">2nd order analysis</param>
        /// <param name="Im">Imperfection shape for 2nd order analysis</param>
        /// <param name="Waterlevel">Ground water level</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public LoadCombination SetCalculationParameters(int ImpfRqd = 0, int StabRqd = 0, bool NLE = false, bool PL = false, bool NLS = false, bool Cr = false, bool f2nd = false, bool Im = false, int Waterlevel = 0)
        {
            this.combItem = new Calculate.CombItem(ImpfRqd, StabRqd, NLE, PL, NLS, Cr, f2nd, Im, Waterlevel);
            return this;
        }

        #endregion
    }
}