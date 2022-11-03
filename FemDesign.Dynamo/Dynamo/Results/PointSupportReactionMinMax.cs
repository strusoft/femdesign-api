using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region dynamo
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;
#endregion

namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class PointSupportReactionMinMax : IResult
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="CaseCombName">Name of Load Case/Load Combination for which to return the results. Default value returns the results for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "MinMax", "CaseIdentifier", "Identifier", "NodeId", "SupportPosition", "ReactionForce", "ReactionMoment", "ForceResultant", "MomentResultant" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.PointSupportReactionMinMax> Result)
        {
            
            List<string> minMax = new List<string>();
            List<string> loadCases = new List<string>();
            List<string> identifier = new List<string>();
            List<int> nodeId = new List<int>();
            List<Point> pos = new List<Point>();
            List<Vector> reactionForce = new List<Vector>();
            List<Vector> reactionMoment = new List<Vector>();
            List<double> forceResultant = new List<double>();
            List<double> momentResultant = new List<double>();

            foreach (var item in Result)
            {
                minMax.Add(item.Max);
                loadCases.Add(item.CaseIdentifier);
                identifier.Add(item.Id);
                nodeId.Add(item.NodeId);
                pos.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(item.X, item.Y, item.Z));
                reactionForce.Add(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(item.Fx, item.Fy, item.Fz));
                reactionMoment.Add(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(item.Mx, item.My, item.Mz));
                forceResultant.Add(item.Fr);
                momentResultant.Add(item.Mr);
            }

            return new Dictionary<string, dynamic>
            {
                {"MinMax", minMax },
                {"CaseIdentifier", loadCases},
                {"Identifier", identifier},
                {"NodeId", nodeId},
                {"SupportPosition", pos},
                {"ReactionForce", reactionForce},
                {"ReactionMoment", reactionMoment},
                {"ForceResultant", forceResultant},
                {"MomentResultant", momentResultant}
            };
        }


        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "PointSupportReactionMinMax";
        }
    }
}
