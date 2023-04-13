using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FemDesign.Calculate;
using Newtonsoft.Json;
namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Bars, Stresses" result
    /// </summary>
    [Result(typeof(BarSteelUtilization), ListProc.SteelDesignBarUtilizationLoadCombination)]
    public partial class BarSteelUtilization : IResult
    {
        public string Id { get; }
        /// <summary>
        /// Applied profile
        /// </summary>
        public string Section { get; }
        public string Status { get; }
        /// <summary>
        /// Maximum utilisation from all the verifications
        /// </summary>
        public double Max { get; }
        /// <summary>
        /// Resistance of cross-section - Part 1-1: 6.2.1
        /// </summary>
        public double RCS { get; }
        /// <summary>
        /// Flexural buckling - Part 1-1: 6.3.1
        /// </summary>
        public double FB { get; }
        /// <summary>
        /// Torsional flexural buckling - Part 1-1: 6.3.1
        /// </summary>
        public double TFB { get; }
        /// <summary>
        /// Lateral torsional buckling, top flange - Part 1-1: 6.3.2.4
        /// </summary>
        public double LTBt { get; }
        /// <summary>
        /// Lateral torsional buckling, bottom flange - Part 1-1: 6.3.2.4
        /// </summary>
        public double LTBb { get; }
        /// <summary>
        /// Shear buckling - Part 1-5: 5
        /// </summary>
        public double SB { get; }
        /// <summary>
        /// Interaction - Part 1-1: 6.3.3
        /// </summary>
        public double IA { get; }
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal BarSteelUtilization(string id, string section, string status, double max, double rcs, double fb, double tfb, double ltbt, double ltbb, double sb, double ia, string caseIdentifier)
        {
            this.Id = id;
            this.Section = section;
            this.Status = status;
            this.Max = max;
            this.RCS = rcs;
            this.FB = fb;
            this.TFB = tfb;
            this.LTBt = ltbt;
            this.LTBb = ltbb;
            this.SB = sb;
            this.IA = ia;
            this.CaseIdentifier = caseIdentifier;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Steel bar), (?'result'Utilization), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Steel bar), (?'result'Utilization), ((?'loadcasetype'[\w\s\-]+)? - )?Load (?'casecomb'case|comb\.): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})$|Member\t|\[.+\]");
            }
        }

        internal static BarSteelUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            string section = row[1];
            string status = row[2];
            double max = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double rcs = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fb = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double tfb = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double ltbt = row[7] == "-" ? 0 : Double.Parse(row[7], CultureInfo.InvariantCulture);
            double ltbb = row[8] == "-" ? 0 : Double.Parse(row[8], CultureInfo.InvariantCulture);
            double sb = row[9] == "-" ? 0 : Double.Parse(row[9], CultureInfo.InvariantCulture);
            double ia = row[10] == "-" ? 0 : Double.Parse(row[10], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarSteelUtilization(id, section, status, max, rcs, fb, tfb, ltbt, ltbb, sb, ia, lc);
        }
    }
}