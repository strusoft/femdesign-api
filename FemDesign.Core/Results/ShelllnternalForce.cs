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
    /// FemDesign "Bars, End forces" result
    /// </summary>
    public class ShellInternalForce : IResult
    {
        /// <summary>
        /// Bar name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element element id
        /// </summary>
        public int ElementId { get; }
        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// Mx' [kNm/m]
        /// </summary>
        public double Mx { get; }
        /// <summary>
        /// My' [kNm/m]
        /// </summary>
        public double My { get; }
        /// <summary>
        /// Mx'y' [kNm/m]
        /// </summary>
        public double Mxy { get; }
        /// <summary>
        /// Nx' [kN/m]
        /// </summary>
        public double Nx { get; }
        /// <summary>
        /// Ny' [kN/m]
        /// </summary>
        public double Ny { get; }
        /// <summary>
        /// Nx'y' [kN/m]
        /// </summary>
        public double Nxy { get; }
        /// <summary>
        /// Tx'z' [kN/m]
        /// </summary>
        public double Txz { get; }
        /// <summary>
        /// Ty'z' [kN/m]
        /// </summary>
        public double Tyz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal ShellInternalForce(string id, int elementId, int nodeId, double mx, double my, double mxy, double nx, double ny, double nxy, double txz, double tyz, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            Mx = mx;
            My = my;
            Mxy = mxy;
            Nx = nx;
            Ny = ny;
            Nxy = nxy;
            Txz = txz;
            Tyz = tyz;
            CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {ElementId}, {NodeId}, {CaseIdentifier}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Shells), (?'result'Internal forces) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Shells), (?'result'Internal forces) ?(?'extract'\(Extract\))?, ((?'loadcasetype'[\w\ ]+)? - )?Load (?'casecomb'case|comb\.+): (?'casename'[\w\ ]+)|ID\tElem\tNode\tMx'\tMy'\tMx'y'\tNx'\tNy'\tNx'y'\tTx'z'\tTy'z'\tCase|\[.*\]");
            }
        }

        internal static ShellInternalForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            if (HeaderData.ContainsKey("extract"))
            {
                string shellname = row[0];
                int elementId = int.Parse(row[2], CultureInfo.InvariantCulture);
                int nodeId = int.Parse(row[3], CultureInfo.InvariantCulture);
                double mx = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double my = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double mxy = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double nx = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double ny = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double nxy = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double txz = Double.Parse(row[10], CultureInfo.InvariantCulture);
                double tyz = Double.Parse(row[11], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new ShellInternalForce(shellname, elementId, nodeId, mx, my, mxy, nx, ny, nxy, txz, tyz, lc);
            }
            else
            {
                string shellname = row[0];
                int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
                int nodeId = int.Parse(row[2], CultureInfo.InvariantCulture);
                double mx = Double.Parse(row[3], CultureInfo.InvariantCulture);
                double my = Double.Parse(row[4], CultureInfo.InvariantCulture);
                double mxy = Double.Parse(row[5], CultureInfo.InvariantCulture);
                double nx = Double.Parse(row[6], CultureInfo.InvariantCulture);
                double ny = Double.Parse(row[7], CultureInfo.InvariantCulture);
                double nxy = Double.Parse(row[8], CultureInfo.InvariantCulture);
                double txz = Double.Parse(row[9], CultureInfo.InvariantCulture);
                double tyz = Double.Parse(row[10], CultureInfo.InvariantCulture);
                string lc = HeaderData["casename"];
                return new ShellInternalForce(shellname, elementId, nodeId, mx, my, mxy, nx, ny, nxy, txz, tyz, lc);
            }
        }
    }
}
