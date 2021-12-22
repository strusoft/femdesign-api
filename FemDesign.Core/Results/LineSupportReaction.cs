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
    /// FemDesign "Point support group, Reactions" result
    /// </summary>
    public class LineSupportReaction : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element identifier
        /// </summary>
        public int ElementId { get; }
        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// Local Fx'
        /// </summary>
        public double Fx { get; }
        /// <summary>
        /// Local Fy'
        /// </summary>
        public double Fy { get; }
        /// <summary>
        /// Local Fz'
        /// </summary>
        public double Fz { get; }
        /// <summary>
        /// Local Mx'
        /// </summary>
        public double Mx { get; }
        /// <summary>
        /// Local My'
        /// </summary>
        public double My { get; }
        /// <summary>
        /// Local Mz'
        /// </summary>
        public double Mz { get; }
        /// <summary>
        /// Force resultant
        /// </summary>
        public double Fr { get; }
        /// <summary>
        /// Moment resultant
        /// </summary>
        public double Mr { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public string CaseIdentifier { get; }

        internal LineSupportReaction(string id, int elementId, int nodeId, double fx, double fy, double fz, double mx, double my, double mz, double fr, double mr, string resultCase)
        {
            Id = id;
            ElementId = elementId;
            NodeId = nodeId;
            Fx = fx;
            Fy = fy;
            Fz = fz;
            My = mx;
            My = my;
            Mz = mz;
            Fr = fr;
            Mr = mr;
            CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {CaseIdentifier}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Line support group), (?'result'Reactions), (?'loadcasetype'[\w\ ]+) - Load (?'casecomb'[\w\ ]+): (?'casename'[\w\ ]+)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Line support group), (?'result'Reactions), (?'loadcasetype'[\w\ ]+) - Load (?'casecomb'[\w\ ]+): (?'casename'[\w\ ]+)|ID|\[.*\]");
            }
        }

        internal static LineSupportReaction Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string supportname = row[0];
            int elementId = int.Parse(row[1], CultureInfo.InvariantCulture);
            int nodeId = int.Parse(row[2], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[7], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[8], CultureInfo.InvariantCulture);
            double fr = Double.Parse(row[9], CultureInfo.InvariantCulture);
            double mr = Double.Parse(row[10], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new LineSupportReaction(supportname, elementId, nodeId, fx, fy, fz, mx, my, mz, fr, mr, lc);
        }
    }
}
