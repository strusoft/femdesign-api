using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;


namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class FeaNode : IResult
    {
        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "FeaNode";
        }

        /// <summary>
        /// Deconstruct the Fea Node Element
        /// </summary>
        /// <param name="FdFeaModel">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "NodeId", "Position" })]
        public static Dictionary<string, object> Deconstruct(FDfea FdFeaModel)
        {
            // Read Result from Abstract Method
            var result = FemDesign.Results.FeaNode.DeconstructFeaNode(FdFeaModel.FeaNode);


            var nodeId = (List<int>)result["NodeId"];
            var feaNodePoint = (List<FemDesign.Geometry.Point3d>)result["Position"];


            // Convert the FdPoint to Rhino
            var ofeaNodePoint = feaNodePoint.Select(x => x.ToDynamo());

            return new Dictionary<string, object>
            {
                {"NodeId", nodeId},
                {"Position", ofeaNodePoint}
            };
        }
    }
}
