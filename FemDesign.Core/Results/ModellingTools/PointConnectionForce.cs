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
    /// FemDesign "Point connection forces" result
    /// </summary>
    [Result(typeof(PointConnectionForce), ListProc.PointConnectionForceLoadCase, ListProc.PointConnectionForceLoadCombination)]
    public partial class PointConnectionForce : IResult
    {
        /// <summary>
        /// Identifier "No."
        /// </summary>
        public string Id { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
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

        public FemDesign.Geometry.Point3d Pos => new Geometry.Point3d(X, Y, Z);
        public FemDesign.Geometry.Vector3d Force => new Geometry.Vector3d(Fx, Fy, Fz);
        public FemDesign.Geometry.Vector3d Moment => new Geometry.Vector3d(Mx, My, Mz);

        [JsonConstructor]
        internal PointConnectionForce(string id, double x, double y, double z, int nodeId, double fx, double fy, double fz, double mx, double my, double mz, double fr, double mr, string resultCase)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
            NodeId = nodeId;
            Fx = fx;
            Fy = fy;
            Fz = fz;
            Mx = mx;
            My = my;
            Mz = mz;
            Fr = fr;
            Mr = mr;
            CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression => new Regex(@"^Point connection forces, .*: (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$");

        internal static Regex HeaderExpression => new Regex(@"^Point connection forces, .*: (?'casename'[ -#%'-;=?A-\ufffd]{1,79}?)(?: - selected objects)?$|^No\.\tx\ty\tz\tNode\tFx'\tFy'\tFz'\tMx'\tMy'\tMz'\tFr\tMr\t(Case|Comb\.)|^\[.*\]");

        internal static PointConnectionForce Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string pointname = row[0];
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
            string lc = row[13];
            return new PointConnectionForce(pointname, x, y, z, nodeId, fx, fy, fz, mx, my, mz, fr, mr, lc);
        }
    }
}
