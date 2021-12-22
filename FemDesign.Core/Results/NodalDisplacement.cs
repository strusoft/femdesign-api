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
    /// FemDesign "Nodal displacements" result
    /// </summary>
    public class NodalDisplacement : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public string Id { get; }
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

        internal NodalDisplacement(string id, int nodeId, double ex, double ey, double ez, double fix, double fiy, double fiz, string resultCase)
        {
            Id = id;
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
                return new Regex(@"(?'type'Nodal displacements), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Nodal displacements), ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)|ID\tNode\tex\tey\tez\tfix\tfiy\tCase|\[.*\]");
            }
        }

        internal static NodalDisplacement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string supportname = row[0];
            int nodeId = int.Parse(row[1], CultureInfo.InvariantCulture);
            double ex = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double ey = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double ez = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fix = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fiy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fiz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            string lc = row[8];
            return new NodalDisplacement(supportname, nodeId, ex, ey, ez, fix, fiy, fiz, lc);
        }
    }
}
