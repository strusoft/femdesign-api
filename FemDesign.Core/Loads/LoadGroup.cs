
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    public class LoadGroup
    {
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
        /// The unfavourable safety coefficient used when combining the load cases
        public double SafetyFactorUnfavourable { get; set; }
        /// The favourable safety coefficient used when combining the load cases
        public double SafetyFactorFavourable { get; set; }
        /// The accidental unfavourable safety coefficient used when combining the load cases
        public double SafetyFactorAccidentalUnfavourable { get; set; }
        /// The accidental favourable safety coefficient used when combining the load cases
        public double SafetyFactorAccidentalFavourable { get; set; }
        /// Coefficient used when combining permanent load cases
        public double Xi { get; set; }
        /// How to combine the load cases in the group
        public ELoadGroupRelationship LoadCaseRelation { get; set; }
        /// Specifies if load cases in group can be leading actions
        public bool PotentiallyLeadingAction { get; set; }  

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadGroup() { }

        /// <summary>
        /// Constructor for permanent load group
        /// </summary>
        /// <param name="name">Name of load group</param>
        /// <param name="type">Type of loads in group, permanent or temporary</param>
        /// <param name="loadCases">List of load cases in the group</param>
        /// /// <param name="gamma_d">Partial coefficient used to account for the safety class</param>
        /// <param name="safetyFactorUnfavourable">The general coefficient used when combining the load cases</param>
        /// <param name="safetyFactorFavourable">The general coefficient used when combining the load cases</param>
        /// <param name="loadCaseRelation">How to combine the load cases in the group</param>
        /// <param name="xi">Coefficient used when combining permanent load cases</param>
        public LoadGroup(string name, ELoadGroupType type, List<LoadCase> loadCases, double gamma_d,
                         double safetyFactorUnfavourable, double safetyFactorFavourable,  
                         ELoadGroupRelationship loadCaseRelation, double xi)
        {
            Name = name;
            Type = type;
            LoadCases = loadCases;
            Gamma_d = gamma_d;
            SafetyFactorUnfavourable = safetyFactorUnfavourable;
            SafetyFactorFavourable = safetyFactorFavourable;
            SafetyFactorAccidentalUnfavourable = 1;
            SafetyFactorAccidentalFavourable = 1;
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
        /// /// <param name="gamma_d">Partial coefficient used to account for the safety class</param>
        /// <param name="safetyFactor">The general coefficient used when combining the load cases</param>
        /// <param name="loadCaseRelation">How to combine the load cases in the group</param>
        /// <param name="potentiallyLeadingAction">Specifies if load cases in group can be leading actions</param> 
        public LoadGroup(string name, ELoadGroupType type, List<LoadCase> loadCases, LoadCategory loadCategory, double gamma_d, double safetyFactorUnfavourable, double safetyFactorFavourable, ELoadGroupRelationship loadCaseRelation, bool potentiallyLeadingAction)
        {
            Name = name;
            Type = type;
            LoadCases = loadCases;
            LoadCategory = loadCategory;
            gamma_d = gamma_d;
            SafetyFactorUnfavourable = safetyFactorUnfavourable;
            SafetyFactorFavourable = safetyFactorFavourable;
            SafetyFactorAccidentalUnfavourable = 1;
            SafetyFactorAccidentalFavourable = 1;
            LoadCaseRelation = loadCaseRelation;
            PotentiallyLeadingAction = potentiallyLeadingAction;
            LoadCaseRelation = loadCaseRelation;
        }
    }
}