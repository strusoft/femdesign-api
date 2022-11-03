using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;


namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class BarDisplacement : IResult
    {
        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "BarDisplacement";
        }

        /// <summary>
        /// Deconstruct the Bar Displacement Results
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="CaseCombName">Name of Load Case/Load Combination for which to return the results. Default value returns the results for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "ElementId", "PositionResult", "Translation", "Rotation" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.BarDisplacement> Result, string CaseCombName)
        {
            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.BarDisplacement.DeconstructBarDisplacements(Result, CaseCombName);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];
            var elementId = (List<string>)result["ElementId"];
            var positionResult = (List<double>)result["PositionResult"];
            var iTranslation = (List<FemDesign.Geometry.Vector3d>)result["Translation"];
            var iRotation = (List<FemDesign.Geometry.Vector3d>)result["Rotation"];

            // Convert the FdVector to Dynamo
            var oTranslation = iTranslation.Select(x => x.ToDynamo());
            var oRotation = iRotation.Select(x => x.ToDynamo());

            var uniqueId = elementId.Distinct().ToList();

            // Convert Data in DataTree structure
            var elementIdTree = new List<List<string>>();
            var positionResultTree = new List<List<double>>();
            var oTranslationTree = new List<List<Autodesk.DesignScript.Geometry.Vector>>();
            var oRotationTree = new List<List<Autodesk.DesignScript.Geometry.Vector>>();


            foreach (var id in uniqueId)
            {
                var elementIdTreeTemp = new List<string>();
                var positionResultTreeTemp = new List<double>();
                var oTranslationTreeTemp = new List<Autodesk.DesignScript.Geometry.Vector>();
                var oRotationTreeTemp = new List<Autodesk.DesignScript.Geometry.Vector>();


                // indexes where the uniqueId matches in the list
                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                foreach (int index in indexes)
                {
                    //loadCasesTree.Add(loadCases.ElementAt(index), new GH_Path(i));
                    elementIdTreeTemp.Add(elementId.ElementAt(index));
                    positionResultTreeTemp.Add(positionResult.ElementAt(index));
                    oTranslationTreeTemp.Add(oTranslation.ElementAt(index));
                    oRotationTreeTemp.Add(oRotation.ElementAt(index));
                }

                elementIdTree.Add(elementIdTreeTemp);
                positionResultTree.Add(positionResultTreeTemp);
                oTranslationTree.Add(oTranslationTreeTemp);
                oRotationTree.Add(oRotationTreeTemp);
            }

            return new Dictionary<string, object>
            {
                {"CaseIdentifier", loadCases},
                {"ElementId", elementIdTree},
                {"PositionResult", positionResultTree},
                {"Translation", oTranslationTree},
                {"Rotation", oRotationTree}
            };
        }
    }
}
