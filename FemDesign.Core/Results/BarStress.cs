using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Bars, Stresses" result
    /// </summary>
    public class BarStress : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Position Result
        /// The value is the relative distance from the
        /// starting point of the bar
        /// </summary>
        public double Pos { get; }

        /// <summary>
        /// Maximum Normal Tension
        /// </summary>
        public double SigmaXiMax { get; }

        /// <summary>
        /// Minimum Normal Tension
        /// </summary>
        public double SigmaXiMin { get; }

        /// <summary>
        /// Von Mises Stress
        /// </summary>
        public double SigmaVM { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal BarStress(string id, double pos, double sigmaXiMax, double sigmaXiMin, double sigmaVM, string resultCase)
        {
            this.Id = id;
            this.Pos = pos;
            this.SigmaXiMax = sigmaXiMax;
            this.SigmaXiMin = sigmaXiMin;
            this.SigmaVM = sigmaVM;
            this.CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {CaseIdentifier}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Bars), (?'result'Stresses), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Bars), (?'result'Stresses), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})|ID\t|\[.*\]");
            }
        }

        internal static BarStress Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string barname = row[0];
            double pos = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double sigmaXiMax = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double sigmaXiMin = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double sigmaVM = Double.Parse(row[4], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarStress(barname, pos, sigmaXiMax, sigmaXiMin, sigmaVM, lc);
        }
    }
}