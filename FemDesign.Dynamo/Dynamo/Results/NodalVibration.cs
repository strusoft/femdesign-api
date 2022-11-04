using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;


namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class NodalVibration : IResult
    {
        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "NodalVibration";
        }

        /// <summary>
        /// Read the Modal Shape vectors for the entire model
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="vibrationMode">Number of vibration mode for which to return the results. Default value returns the results for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "ShapeId", "Identifier", "NodeId", "Translation", "Rotation" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.NodalVibration> Result, int vibrationMode)
        {
            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.NodalVibration.DeconstructNodalVibration(Result, vibrationMode.ToString());
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            var shapeIds = (List<int>)result["ShapeId"];
            var identifier = (List<string>)result["Id"];
            var nodeId = (List<int>)result["NodeId"];
            var iTranslation = (List<FemDesign.Geometry.Vector3d>)result["Translation"];
            var iRotation = (List<FemDesign.Geometry.Vector3d>)result["Rotation"];

            // Convert the FdVector to Dynamo
            var oTranslation = iTranslation.Select(x => x.ToDynamo());
            var oRotation = iRotation.Select(x => x.ToDynamo());


            return new Dictionary<string, object>
            {
                {"ShapeId", shapeIds},
                {"Identifier", identifier},
                {"NodeId", nodeId},
                {"Translation", oTranslation},
                {"Rotation", oRotation}
            };
        }
    }
}
