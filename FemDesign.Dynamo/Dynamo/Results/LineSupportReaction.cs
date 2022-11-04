using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LineSupportReaction : IResult
    {
        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "LineSupportReaction";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="CaseCombName">Name of Load Case/Load Combination for which to return the results. Default value returns the results for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "Identifier", "ElementId", "NodeId", "ReactionForce", "ReactionMoment", "ForceResultant", "MomentResultant" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.LineSupportReaction> Result, string CaseCombName)
        {
            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.LineSupportReaction.DeconstructLineSupportReaction(Result, CaseCombName);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            var loadCases = (List<string>)result["CaseIdentifier"];
            var identifier = (List<string>)result["Identifier"];
            var elementId = (List<int>)result["ElementId"];
            var nodeId = (List<int>)result["NodeId"];
            var iReactionForce = (List<FemDesign.Geometry.Vector3d>)result["ReactionForce"];
            var iReactionMoment = (List<FemDesign.Geometry.Vector3d>)result["ReactionMoment"];
            var iForceResultant = (List<double>)result["ForceResultant"];
            var iMomentResultant = (List<double>)result["MomentResultant"];

            // Convert the FdVector to Grasshopper
            var oReactionForce = iReactionForce.Select(x => x.ToDynamo());
            var oReactionMoment = iReactionMoment.Select(x => x.ToDynamo());


            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", loadCases},
                {"Identifier", identifier},
                {"ElementId", elementId},
                {"NodeId", nodeId},
                {"ReactionForce", oReactionForce},
                {"ReactionMoment", oReactionMoment},
                {"ForceResultant", iForceResultant},
                {"MomentResultant", iMomentResultant}
            };
        }
    }
}