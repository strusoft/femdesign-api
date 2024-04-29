using FemDesign.Calculate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    [Result(typeof(Equilibrium), ListProc.EquilibriumLoadCase, ListProc.EquilibriumLoadCombination)]
    public class Equilibrium : IResult
    {
        public string CaseIdentifier { get; }
        public string Component { get; }
        public double Loads { get; }
        public double Reactions { get; }
        /// <summary>
        /// Error express in percentage.
        /// </summary>
        public double Error { get; }


        [JsonConstructor]
        internal Equilibrium(string caseIdentifier, string component, double loads, double reactions, double error)
        {
            this.CaseIdentifier = caseIdentifier;
            this.Component = component;
            this.Loads = loads;
            this.Reactions = reactions;
            this.Error = error;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Equilibrium)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Equilibrium)|(Load comb|Case)|\[.*\]");
            }
        }

        internal static Equilibrium Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string caseIdentifier = row[0];
            string component = row[1];
            double loads = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double reactions = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double error = row[4] == "-" ? 0 : Double.Parse(row[4], CultureInfo.InvariantCulture);
            return new Equilibrium(caseIdentifier, component, loads, reactions, error);
        }

    }
}
