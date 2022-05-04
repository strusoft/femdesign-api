using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;


namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class NodalDisplacement : IResult
    {
        /// <summary>
        /// Create new model. Add entities to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="LoadCase">Name of Load Case for which to return the results. Default value returns the displacement for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "NodeId", "Translation", "Rotation" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.NodalDisplacement> Result, [DefaultArgument("null")] string LoadCase)
        {
            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = DeconstructNodalDisplacements(Result, LoadCase);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            var loadCases = (List<string>)result["CaseIdentifier"];
            var nodeId = (List<int>)result["NodeId"];
            var iTranslation = (List<FemDesign.Geometry.FdVector3d>)result["Translation"];
            var iRotation = (List<FemDesign.Geometry.FdVector3d>)result["Rotation"];


            // Convert the FdVector to Dynamo
            var oTranslation = iTranslation.Select(x => x.ToDynamo());
            var oRotation = iRotation.Select(x => x.ToDynamo());


            return new Dictionary<string, object>
            {
                {"CaseIdentifier", loadCases},
                {"NodeId", nodeId},
                {"Translation", oTranslation},
                {"Rotation", oRotation}
            };
        }
    }
}
