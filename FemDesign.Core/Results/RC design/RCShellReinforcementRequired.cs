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
    [Result(typeof(RCShellReinforcementRequired), ListProc.RCDesignShellRequiredReinforcementLoadCombination)]
    public class RCShellReinforcementRequired : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element element id
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// Finite element node id
        /// </summary>
        public int? NodeId { get; }

        public double XBottom { get; }
        public double YBottom { get; }
        public double XTop { get; }
        public double YTop { get; }
        public double? XMid { get; }
        public double? YMid { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        [JsonConstructor]
        internal RCShellReinforcementRequired(string id, int elementId, int nodeId, double xBottom, double yBottom, double xTop, double yTop, double xMid, double yMid, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            XBottom = xBottom;
            YBottom = yBottom;
            XTop = xTop;
            YTop = yTop;
            XMid = xMid;
            YMid = yMid;
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
                return new Regex(@"(?'max'Max. of load combinations, Shell, Required reinforcement)|(?'type'Shell, Required reinforcement), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'max'Max. of load combinations, Shell, Required reinforcement)|(?'type'Shell, Required reinforcement), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})|Shell\t|\[.*\]");
            }
        }

        internal static RCShellReinforcementRequired Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            //if (HeaderData.ContainsKey("max"))
            //{
            //    string id = row[0];
            //    double rbx = double.Parse(row[3], CultureInfo.InvariantCulture);
            //    double rby = double.Parse(row[4], CultureInfo.InvariantCulture);
            //    double rtx = double.Parse(row[5], CultureInfo.InvariantCulture);
            //    double rty = double.Parse(row[6], CultureInfo.InvariantCulture);
            //    double bu = double.Parse(row[7], CultureInfo.InvariantCulture);
            //    bool sc = row[8] == "OK";
            //    double cwb = double.Parse(row[9], CultureInfo.InvariantCulture);
            //    double cwt = double.Parse(row[10], CultureInfo.InvariantCulture);
            //    string lc = row[2];
            //    return new RCShellUtilization(id, rbx, rby, rtx, rty, bu, sc, cwb, cwt, lc);
            //}

            {
                string id = row[0];
                int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
                int nodeId = Int32.Parse(row[2] == "-" ? "-1" : row[2], CultureInfo.InvariantCulture);
                double xBottom = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double yBottom = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double xTop = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double yTop = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double xMid = Double.Parse(row[7] == "-" ? "0" : row[7], CultureInfo.InvariantCulture);
                double yMid = Double.Parse(row[8] == "-" ? "0" : row[8], CultureInfo.InvariantCulture);

                string lc = HeaderData["casename"];
                return new RCShellReinforcementRequired(id, elementId, nodeId, xBottom, yBottom, xTop, yTop, xMid, yMid, lc);
            }

        }
    }
}
