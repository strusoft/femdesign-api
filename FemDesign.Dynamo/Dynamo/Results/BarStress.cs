using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;

namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class BarStress : IResult
    {

        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "BarStress";
        }

        /// <summary>
        /// Read Bar Stress from a previously run model.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="CaseCombName">Name of Load Case/Load Combination for which to return the results. Default value returns the results for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "ElementId", "PositionResult", "SigmaXiMax", "SigmaXiMin", "SigmaVM"})]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.BarStress> Result, string CaseCombName)
        {
            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.BarStress.DeconstructBarStress(Result, CaseCombName);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];
            var elementId = (List<string>)result["ElementId"];
            var positionResult = (List<double>)result["PositionResult"];
            var sigmaXiMax = (List<double>)result["SigmaXiMax"];
            var sigmaXiMin = (List<double>)result["SigmaXiMin"];
            var sigmaVM = (List<double>)result["SigmaVM"];

            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            var elementIdTree = new List<List<string>>();
            var positionResultTree = new List<List<double>>();
            var sigmaXiMaxTree = new List<List<double>>();
            var sigmaXiMinTree = new List<List<double>>();
            var sigmaVMTree = new List<List<double>>();


            foreach (var id in uniqueId)
            {
                var elementIdTreeTemp = new List<string>();
                var positionResultTreeTemp = new List<double>();
                var sigmaXiMaxTreeTemp = new List<double>();
                var sigmaXiMinTreeTemp = new List<double>();
                var sigmaVMTreeTemp = new List<double>();


                // indexes where the uniqueId matches in the list
                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                foreach (int index in indexes)
                {
                    elementIdTreeTemp.Add(elementId.ElementAt(index));
                    positionResultTreeTemp.Add(positionResult.ElementAt(index));

                    sigmaXiMaxTreeTemp.Add(sigmaXiMax.ElementAt(index));
                    sigmaXiMinTreeTemp.Add(sigmaXiMin.ElementAt(index));
                    sigmaVMTreeTemp.Add(sigmaVM.ElementAt(index));
                }

                elementIdTree.Add(elementIdTreeTemp);
                positionResultTree.Add(positionResultTreeTemp);

                sigmaXiMaxTree.Add(sigmaXiMaxTreeTemp);
                sigmaXiMinTree.Add(sigmaXiMinTreeTemp);
                sigmaVMTree.Add(sigmaVMTreeTemp);
            }

            return new Dictionary<string, object>
            {
                {"CaseIdentifier", uniqueLoadCase},
                {"ElementId", elementIdTree},
                {"PositionResult", positionResultTree},
                {"SigmaXiMax", sigmaXiMaxTree},
                {"SigmaXiMin", sigmaXiMinTree},
                {"SigmaVM", sigmaVMTree},
            };
        }
    }
}