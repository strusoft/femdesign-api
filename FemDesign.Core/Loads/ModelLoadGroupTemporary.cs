// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// temporary_load_group (child of general_load_group)
    /// </summary>
    [System.Serializable]
    public partial class ModelLoadGroupTemporary: GenericClasses.IFemDesignEntity
    {
        [XmlAttribute("safety_factor")]
        public double _safetyFactor;
        [XmlIgnore]
        public double SafetyFactor
        {
            get
            {
                return this._safetyFactor;
            }
            set
            {
                this._safetyFactor = RestrictedDouble.Positive(value);
            }
        }
        [XmlAttribute("psi_0")]
        public double _psi0;
        [XmlIgnore]
        public double Psi0
        {
            get
            {
                return this._psi0;
            }
            set
            {
                this._psi0 = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("psi_1")]
        public double _psi1;
        [XmlIgnore]
        public double Psi1
        {
            get
            {
                return this._psi1;
            }
            set
            {
                this._psi1 = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("psi_2")]
        public double _psi2;
        [XmlIgnore]
        public double Psi2
        {
            get
            {
                return this._psi2;
            }
            set
            {
                this._psi2 = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("leading_cases")]
        public bool LeadingCases { get; set; }
        [XmlAttribute("ignore_sls")]
        public bool IgnoreSls { get; set; } = false;
        [XmlElement("load_case")]
        public List<ModelLoadCaseInGroup> ModelLoadCase { get; set; }// sequence: ModelLoadCaseInGroup
        [XmlAttribute("relationship")]
        public ELoadGroupRelationship Relationship { get; set; }

        public ModelLoadGroupTemporary()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="guid">LoadCase guid reference.</param>
        public ModelLoadGroupTemporary(double safetyFactor,
                                       double psi0, double psi1,double psi2, 
                                       bool potentiallyLeadingAction, List<LoadCase> loadCases,
                                       ELoadGroupRelationship relationsship)
        {
            this.SafetyFactor = safetyFactor;
            this.Psi0 = psi0;
            this.Psi1 = psi1;
            this.Psi2 = psi2;
            this.LeadingCases = potentiallyLeadingAction;
            this.Relationship = relationsship;

            ModelLoadCase = new List<ModelLoadCaseInGroup>();
            for (int i = 0; i < loadCases.Count; i++)
                AddLoadCase(loadCases[i]);
        }

        /// <summary>
        /// Add LoadCase to group.
        /// </summary>
        private void AddLoadCase(LoadCase loadCase)
        {
            if (LoadCaseInLoadGroup(loadCase))
            {
                // pass
            }
            else
            {
                ModelLoadCase.Add(new ModelLoadCaseInGroup(loadCase.Guid));
            }
        }

        /// <summary>
        /// Check if LoadCase in LoadGroup.
        /// </summary>
        private bool LoadCaseInLoadGroup(LoadCase loadCase)
        {
            foreach (ModelLoadCaseInGroup elem in this.ModelLoadCase)
            {
                if (elem.Guid == loadCase.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds attributes for serialization
        /// </summary>
        public void EntityCreated()
        {
            //pass
        }
    }
}