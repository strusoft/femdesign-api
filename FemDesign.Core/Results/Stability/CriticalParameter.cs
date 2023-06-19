using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;
using FemDesign.Calculate;
using Newtonsoft.Json;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Critical parameters" result
    /// </summary>
    [Result(typeof(CriticalParameter), ListProc.CriticalParameters)]
    public partial class CriticalParameter : IStabilityResult
    {
        public string CaseIdentifier { get; }
        public int Shape { get; }
        public double CriticalParam { get; }

        [JsonConstructor]
        internal CriticalParameter(string resultCase, int shape, double criticalParam)
        {
            CaseIdentifier = resultCase;
            Shape = shape;
            CriticalParam = criticalParam;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Critical parameters) \(Only positive\)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Critical parameters) \(Only positive\)|Comb\tShape\tCritical param.|\t\t\[.*\]");
            }
        }

        internal static CriticalParameter Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string caseIdentifier = row[0];
            int shape = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            double criticalParam = Double.Parse(row[2], CultureInfo.InvariantCulture);
            return new CriticalParameter(caseIdentifier, shape, criticalParam);
        }
    }
}