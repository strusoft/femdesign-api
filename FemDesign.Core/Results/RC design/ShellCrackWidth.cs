using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;


namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "RC design: Shell, utilization" result
    /// </summary>
    public class ShellCrackWidth : IResult
    {
        /// <summary>
        /// Shell name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element id
        /// </summary>
        public int ElementId { get; }
        /// <summary>
        /// Face of the shell
        /// </summary>
        public Face Face { get; }
        /// <summary>
        /// Creck width in primary direction
        /// </summary>
        public double Width1 { get; }
        /// <summary>
        /// Primary direction angle
        /// </summary>
        public double Direction1 { get; }
        /// <summary>
        /// Creck width in secondary direction
        /// </summary>
        public double Width2 { get; }
        /// <summary>
        /// Secondary direction angle
        /// </summary>
        public double Direction2 { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal ShellCrackWidth(string id, int elementId, Face face, double w1, double d1, double w2, double d2, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            Face = face;
            Width1 = w1;
            Direction1 = d1;
            Width2 = w2;
            Direction2 = d2;
            CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return $"{{{base.ToString()}, {Id}, {ElementId}, {Face}}}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'max'Max. of load combinations, Shell, Crack width)|(?'type'Shell, Crack width), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'max'Max. of load combinations, Shell, Crack width)|(?'type'Shell, Crack width), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})|ID\tElem|\[.*\]");
            }
        }

        internal static ShellCrackWidth Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            // TODO: Read line by line instead of saving to "HeaderData"
            // This is due to a bug where femdesign don't fill in all table values
            // This is a workaround

            if (string.IsNullOrEmpty(row[0]))
                row[0] = HeaderData["id"];
            else
                HeaderData["id"] = row[0];

            if (string.IsNullOrEmpty(row[1]))
                row[1] = HeaderData["elementId"];
            else
                HeaderData["elementId"] = row[1];

            string id = row[0];
            int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
            Face face = (Face)Enum.Parse(typeof(Face), row[2], true);
            double w1 = double.Parse(row[3], CultureInfo.InvariantCulture);
            double d1 = double.Parse(row[4], CultureInfo.InvariantCulture);
            double w2 = double.Parse(row[5], CultureInfo.InvariantCulture);
            double d2 = double.Parse(row[6], CultureInfo.InvariantCulture);

            string lc = (row.Length > 7) ? row[7] : HeaderData["casename"];

            return new ShellCrackWidth(id, elementId, face, w1, d1, w2, d2, lc);
        }
    }
}
