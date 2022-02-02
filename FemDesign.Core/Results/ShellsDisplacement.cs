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
    /// FemDesign "Shells, displacements" result
    /// </summary>
    public class ShellsDisplacement : IResult
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
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// Displacement in global x
        /// </summary>
        public double Ex { get; }
        /// <summary>
        /// Displacement in global y
        /// </summary>
        public double Ey { get; }
        /// <summary>
        /// Displacement in global z
        /// </summary>
        public double Ez { get; }
        /// <summary>
        /// Rotation around global x
        /// </summary>
        public double Fix { get; }
        /// <summary>
        /// Rotation around global y
        /// </summary>
        public double Fiy { get; }
        /// <summary>
        /// Rotation around global z
        /// </summary>
        public double Fiz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal ShellsDisplacement(string id, int elementId, int nodeId, double ex, double ey, double ez, double fix, double fiy, double fiz, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            Ex = ex;
            Ey = ey;
            Ez = ez;
            Fix = fix;
            Fiy = fiy;
            Fiz = fiz;
            CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {NodeId}, {CaseIdentifier}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Shells, Displacements( \(Extract\))?), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Shells, Displacements( \(Extract\))?), ((?'loadcasetype'[\w\s]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)|Shell\t.*|\[.*\]");
            }
        }

        internal static ShellsDisplacement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string name, lc;
            int elementId, nodeId;
            double ex, ey, ez, fix, fiy, fiz;
            if (row.Count() == 13) // Extract
            {
                name = row[0];
                elementId = int.Parse(row[2], CultureInfo.InvariantCulture);
                nodeId = int.Parse(row[3], CultureInfo.InvariantCulture);
                ex = Double.Parse(row[4], CultureInfo.InvariantCulture);
                ey = Double.Parse(row[5], CultureInfo.InvariantCulture);
                ez = Double.Parse(row[6], CultureInfo.InvariantCulture);
                fix = Double.Parse(row[7], CultureInfo.InvariantCulture);
                fiy = Double.Parse(row[8], CultureInfo.InvariantCulture);
                fiz = Double.Parse(row[9], CultureInfo.InvariantCulture);
                lc = HeaderData["casename"];
            }
            else
            {
                name = row[0];
                elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
                nodeId = int.Parse(row[2], CultureInfo.InvariantCulture);
                ex = Double.Parse(row[3], CultureInfo.InvariantCulture);
                ey = Double.Parse(row[4], CultureInfo.InvariantCulture);
                ez = Double.Parse(row[5], CultureInfo.InvariantCulture);
                fix = Double.Parse(row[6], CultureInfo.InvariantCulture);
                fiy = Double.Parse(row[7], CultureInfo.InvariantCulture);
                fiz = Double.Parse(row[8], CultureInfo.InvariantCulture);
                lc = HeaderData["casename"];
            }
            return new ShellsDisplacement(name, elementId, nodeId, ex, ey, ez, fix, fiy, fiz, lc);
        }
    }
}
