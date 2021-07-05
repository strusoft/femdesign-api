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
        public readonly string Id;
        /// <summary>
        /// Finite element identifier
        /// </summary>
        public readonly int ElementId;
        /// <summary>
        /// Finite element node id
        /// </summary>
        public readonly int NodeId;
        /// <summary>
        /// Local Fx'
        /// </summary>
        public readonly double Fx;
        /// <summary>
        /// Local Fy'
        /// </summary>
        public readonly double Fy;
        /// <summary>
        /// Local Fz'
        /// </summary>
        public readonly double Fz;
        /// <summary>
        /// Local Mx'
        /// </summary>
        public readonly double Mx;
        /// <summary>
        /// Local My'
        /// </summary>
        public readonly double My;
        /// <summary>
        /// Local Mz'
        /// </summary>
        public readonly double Mz;
        /// <summary>
        /// Force resultant
        /// </summary>
        public readonly double Fr;
        /// <summary>
        /// Moment resultant
        /// </summary>
        public readonly double Mr;
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public readonly string CaseIdentifier;

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
