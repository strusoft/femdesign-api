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
    public class PointSupportReaction : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public readonly string Id;
        /// <summary>
        /// X-coordinate
        /// </summary>
        public readonly double X;
        /// <summary>
        /// Y-coordinate
        /// </summary>
        public readonly double Y;
        /// <summary>
        /// Z-coordinate
        /// </summary>
        public readonly double Z;
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

        internal PointSupportReaction(string id, double x, double y, double z, int nodeId, double fx, double fy, double fz, double mx, double my, double mz, double fr, double mr, string resultCase)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
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

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Point support group), (?'result'Reactions), (?'loadcasetype'[\w\ ]+) - Load (?'casecomb'[\w\ ]+): (?'casename'[\w\ ]+)|(ID)|(\[.*\])");
            }
        }

        internal static PointSupportReaction Parse(string[] row, CsvReader reader, Dictionary<string, string> HeaderData)
        {
            string supportname = row[0];
            double x = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double y = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double z = Double.Parse(row[3], CultureInfo.InvariantCulture);
            int nodeId = int.Parse(row[4], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[8], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[9], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[10], CultureInfo.InvariantCulture);
            double fr = Double.Parse(row[11], CultureInfo.InvariantCulture);
            double mr = Double.Parse(row[12], CultureInfo.InvariantCulture);
            string lc = HeaderData["casename"];
            return new PointSupportReaction(supportname, x, y, z, nodeId, fx, fy, fz, mx, my, mz, fr, mr, lc);
        }
    }
}
