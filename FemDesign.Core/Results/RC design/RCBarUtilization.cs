using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;
using Newtonsoft.Json;
using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "RC design: Shell, utilization" result
    /// </summary>
    [Result(typeof(RCBarUtilization), ListProc.RCDesignBarUtilizationLoadCombination)]
    public class RCBarUtilization : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string Id { get; }
        public double Max { get; }
        /// <summary>
        /// Section utilization
        /// </summary>
        public double SEC { get; }
        /// <summary>
        /// Stirrups utilization
        /// </summary>
        public double ST { get; }
        /// <summary>
        /// Concrete utilization
        /// </summary>
        public double C { get; }
        /// <summary>
        /// Torsional utilization
        /// </summary>
        public double T { get; }

        /// <summary>
        /// Utilization for crack width
        /// </summary>
        public double CW { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCBarUtilization(string id, double max, double sec, double st, double c, double t, double cw, string resultCase)
        {
            Id = id;
            Max = max;
            SEC = sec;
            ST = st;
            C = c;
            T = t;
            CW = CW;
            CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'Cmax'Max. of load combinations, RC bar, Utilization)|(?'Gmax'Max.of load groups, RC bar, Utilization)|(?'type'RC bar, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'RC bar, Utilization), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})|Bar\t|\[.*\]");
            }
        }

        internal static RCBarUtilization Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            double max = double.Parse(row[1], CultureInfo.InvariantCulture);
            double sec = double.Parse(row[2], CultureInfo.InvariantCulture);
            double st = double.Parse(row[3], CultureInfo.InvariantCulture);
            double c = double.Parse(row[4], CultureInfo.InvariantCulture);
            double t = double.Parse(row[5], CultureInfo.InvariantCulture);
            double cw = row[6] == "-" ? 0 : double.Parse(row[6], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"]; ;
            return new RCBarUtilization(id, max, sec, st, c, t, cw, lc);  
        }
    }
}
