
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_group_type
    /// </summary>
    public class LoadGroup
    {
        // Attributes
        /// Name of the group
        public string Name { get; set; }
        /// Load group type, permanent or temporary
        public ELoadGroupType Type { get; set; }
        /// Values used to combine load cases
        public LoadCategory LoadCategory { get; set; }
        /// List of load cases that belong to the group
        public List<LoadCase> LoadCases { get; set; }
        /// Partial coefficient used to account for the safety class
        public double Gamma_d { get; set; }
        /// The general coefficient used when combining the load cases
        public double SafetyFactor { get; set; }
        /// Coefficient used when combining permanent load cases
        public double Xi { get; set; }
        /// How to combine the load cases in the group
        public ELoadGroupRelation LoadCaseRelation { get; set; }
        /// Specifies if load cases in group can be leading actions
        public bool PotentiallyLeadingAction { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LoadGroup() { }

        /// <summary>
        /// Constructor for permanent load group
        /// </summary>
        /// <param name="name">Name of load group</param>
        /// <param name="type">Type of loads in group, permanent or temporary</param>
        /// <param name="loadCases">List of load cases in the group</param>
        /// <param name="gamma_d">Partial coefficient used to account for the safety class</param>
        /// <param name="safetyFactor">The general coefficient used when combining the load cases</param>
        /// <param name="loadCaseRelation">How to combine the load cases in the group</param>
        /// <param name="xi">Coefficient used when combining permanent load cases</param>
        public LoadGroup(string name, ELoadGroupType type, List<LoadCase> loadCases, double gamma_d, double safetyFactor, ELoadGroupRelation loadCaseRelation, double xi)
        {
            Name = name;
            Type = type;
            LoadCases = loadCases;
            Gamma_d = gamma_d;
            SafetyFactor = safetyFactor;
            Xi = xi;
            LoadCaseRelation = loadCaseRelation;
            
        }

        /// <summary>
        /// Constructor for temporary load group
        /// </summary>
        /// <param name="name">Name of load group</param>
        /// <param name="type">Type of loads in group, permanent or temporary</param>
        /// <param name="loadCases">List of load cases in the group</param>
        /// <param name="loadCategory">Load category with psi values used to combine load cases</param>
        /// <param name="gamma_d">Partial coefficient used to account for the safety class</param>
        /// <param name="safetyFactor">The general coefficient used when combining the load cases</param>
        /// <param name="loadCaseRelation">How to combine the load cases in the group</param>
        /// <param name="potentiallyLeadingAction">Specifies if load cases in group can be leading actions</param> 
        public LoadGroup(string name, ELoadGroupType type, List<LoadCase> loadCases, LoadCategory loadCategory, double gamma_d, double safetyFactor, ELoadGroupRelation loadCaseRelation, bool potentiallyLeadingAction)
        {
            Name = name;
            Type = type;
            LoadCases = loadCases;
            LoadCategory = loadCategory;
            Gamma_d = gamma_d;
            SafetyFactor = safetyFactor;
            LoadCaseRelation = loadCaseRelation;
            PotentiallyLeadingAction = potentiallyLeadingAction;
        }


    }
}