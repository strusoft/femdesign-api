
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
        // attributes
        public string Name { get; set; }
        public ELoadGroupType Type { get; set; }
        public List<double> Psi_values = new List<double>();
        public List<LoadCase> LoadCases = new List<LoadCase>();
        public double Gamma_d { get; set; }
        public double SafetyFactor { get; set; }
        public double Xi { get; set; }
        public ELoadGroupRelation LoadCaseRelation { get; set; }

        public LoadGroup() { }

        /// <summary>
        /// Constructor for permanent load group
        /// </summary>
        /// <param name="name">Name/Identifier of LoadGroup.</param>
        public LoadGroup(string name, ELoadGroupType type, List<LoadCase> loadCases, List<double> psi, double gamma_d, double safetyFactor, ELoadGroupRelation loadCaseRelation, double xi)
        {
            Name = name;
            Type = type;
            LoadCases = loadCases;
            Psi_values = psi;
            Gamma_d = gamma_d;
            SafetyFactor = safetyFactor;
            Xi = xi;
            LoadCaseRelation = loadCaseRelation;
            
        }

        /// <summary>
        /// Constructor for variable load group
        /// </summary>
        /// <param name="name">Name/Identifier of LoadGroup.</param>
        public LoadGroup(string name, ELoadGroupType type, List<LoadCase> loadCases, List<double> psi, double gamma_d, double safetyFactor, ELoadGroupRelation loadCaseRelation)
        {
            Name = name;
            Type = type;
            LoadCases = loadCases;
            Psi_values = psi;
            Gamma_d = gamma_d;
            SafetyFactor = safetyFactor;
            LoadCaseRelation = loadCaseRelation;
        }


    }
}