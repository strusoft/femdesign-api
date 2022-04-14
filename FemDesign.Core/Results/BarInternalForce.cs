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
    /// FemDesign "Bars, Internal Force" result
    /// </summary>
    public class BarInternalForce : IResult
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
        /// Axial force
        /// </summary>
        public double Fx { get; }

        /// <summary>
        /// Shear force along major axis
        /// </summary>
        public double Fy { get; }

        /// <summary>
        /// Shear force along minor axis
        /// </summary>
        public double Fz { get; }

        /// <summary>
        /// Moment around axis
        /// </summary>
        public double Mx { get; }

        /// <summary>
        /// Moment around major bending axis
        /// </summary>
        public double My { get; }

        /// <summary>
        /// Moment around minor bending axis
        /// </summary>
        public double Mz { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal BarInternalForce(string id, double pos, double fx, double fy, double fz, double mx, double my, double mz, string resultCase)
        {
            this.Id = id;
            this.Pos = pos;
            this.Fx = fx;
            this.Fy = fy;
            this.Fz = fz;
            this.Mx = mx;
            this.My = my;
            this.Mz = mz;
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
                return new Regex(@"(?'type'Bars), (?'result'Internal forces), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Bars), (?'result'Internal forces), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})|ID\t|\[.*\]");
            }
        }

        internal static BarInternalForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string barname = row[0];
            double pos = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarInternalForce(barname, pos, fx, fy, fz, mx, my, mz, lc);
        }
    }
}
