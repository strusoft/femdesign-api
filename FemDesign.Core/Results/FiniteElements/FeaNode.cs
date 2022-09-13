using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;

using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Node" result
    /// </summary>
    [Result(typeof(FeaNode), ListProc.FeaNode)]
    public partial class FeaNode : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// X-coordinate
        /// </summary>
        public double X { get; }
        /// <summary>
        /// Y-coordinate
        /// </summary>
        public double Y { get; }
        /// <summary>
        /// Z-coordinate
        /// </summary>
        public double Z { get; }

        internal FeaNode(int nodeId, double x, double y, double z)
        {
            NodeId = nodeId;
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {NodeId}, {X},{Y},{Z}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Nodes)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"(?'type'Nodes)|No\..|\[.*\]");
            }
        }

        internal static FeaNode Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            int nodeId = Int32.Parse(row[0], CultureInfo.InvariantCulture);
            double x = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double y = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double z = Double.Parse(row[3], CultureInfo.InvariantCulture);
            string test = HeaderData["type"];
            return new FeaNode(nodeId, x, y, z);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        public static Dictionary<string, object> DeconstructFeaNode(List<FemDesign.Results.FeaNode> Result)
        {
            var feaNodes = Result.Cast<FemDesign.Results.FeaNode>();


            // Parse Results from the object
            var nodeId = feaNodes.Select(n => n.NodeId).ToList();


            // Create a Fd Vector/Point for Visualising the Reaction Forces
            var feaNodePoint = new List<FemDesign.Geometry.Point3d>();

            foreach (var node in feaNodes)
            {
                var pos = new FemDesign.Geometry.Point3d(node.X, node.Y, node.Z);
                feaNodePoint.Add(pos);
            }


            return new Dictionary<string, dynamic>
            {
                {"NodeId", nodeId},
                {"Position", feaNodePoint},
            };
        }
    }
}
