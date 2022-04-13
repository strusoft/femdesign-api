﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Bars, Displacements" result
    /// </summary>

    public class BarDisplacement : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string BarId { get; }

        /// <summary>
        /// Position Result
        /// </summary>
        public double Pos { get; }

        /// <summary>
        /// Displacement in Global or Local x
        /// </summary>
        public double Ex { get; }

        /// <summary>
        /// Displacement in Global or Local y
        /// </summary>
        public double Ey { get; }

        /// <summary>
        /// Displacement in Global or Local z
        /// </summary>
        public double Ez { get; }

        /// <summary>
        /// Rotation around Global or Local x
        /// </summary>
        public double Fix { get; }

        /// <summary>
        /// Rotation around Global or Local y
        /// </summary>
        public double Fiy { get; }

        /// <summary>
        /// Rotation around Global or Local z
        /// </summary>
        public double Fiz { get; }

        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal BarDisplacement(string barId, double pos, double ex, double ey, double ez, double fix, double fiy, double fiz, string resultCase)
        {
            this.BarId = barId;
            this.Pos = pos;
            this.Ex = ex;
            this.Ey = ey;
            this.Ez = ez;
            this.Fix = fix;
            this.Fiy = fiy;
            this.Fiz = fiz;
            this.CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {BarId}, {CaseIdentifier}";
        }
        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Bars), (?'result'Displacements),( (?'loadcasetype'[\w\ ]+) -)? Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Bars), (?'result'Displacements),( (?'loadcasetype'[\w\ ]+) -)? Load (?'casecomb'case|comb.+): (?'casename'[ -#%'-;=?A-\ufffd]{1,79})|Bar\t|\[.*\]");
            }
        }

        internal static BarDisplacement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string barname = row[0];
            double pos = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double ex = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double ey = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double ez = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fix = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fiy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fiz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new BarDisplacement(barname, pos, ex, ey, ez, fix, fiy, fiz, lc);
        }
    }
}