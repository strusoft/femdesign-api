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
    public partial class PointSupportReaction : IResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="LoadCase">Name of Load Case for which to return the results. Default value returns the displacement for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "Identifier", "NodeId", "SupportPosition", "ReactionForce", "ReactionMoment", "ForceResultant", "MomentResultant" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.PointSupportReaction> Result, [DefaultArgument("null")] string LoadCase)
        {
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.PointSupportReaction.DeconstructPointSupportReaction(Result, LoadCase);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            var loadCases = (List<string>)result["CaseIdentifier"];
            var identifier = (List<string>)result["Identifier"];
            var nodeId = (List<int>)result["NodeId"];
            var iPos = (List<FemDesign.Geometry.FdPoint3d>)result["Position"];
            var iReactionForce = (List<FemDesign.Geometry.FdVector3d>)result["ReactionForce"];
            var iReactionMoment = (List<FemDesign.Geometry.FdVector3d>)result["ReactionMoment"];
            var iForceResultant = (List<double>)result["ForceResultant"];
            var iMomentResultant = (List<double>)result["MomentResultant"];

            // Convert the FdVector to Grasshopper
            var oPos = iPos.Select(x => x.ToDynamo());
            var oReactionForce = iReactionForce.Select(x => x.ToDynamo());
            var oReactionMoment = iReactionMoment.Select(x => x.ToDynamo());


            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", loadCases},
                {"Identifier", identifier},
                {"NodeId", nodeId},
                {"SupportPosition", oPos},
                {"ReactionForce", oReactionForce},
                {"ReactionMoment", oReactionMoment},
                {"ForceResultant", iForceResultant},
                {"MomentResultant", iMomentResultant}
            };
        }
    }
}
