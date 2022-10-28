using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;

namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class BarInternalForce : IResult
    {

        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "BarInternalForce";
        }

        /// <summary>
        /// Create new model. Add entities to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="CaseCombName">Name of Load Case/Load Combination for which to return the results. Default value returns the results for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "ElementId", "PositionResult", "Fx", "Fy", "Fz", "Mx", "My", "Mz" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.BarInternalForce> Result, string CaseCombName)
        {
            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.BarInternalForce.DeconstructBarInternalForce(Result, CaseCombName);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];
            var elementId = (List<string>)result["ElementId"];
            var positionResult = (List<double>)result["PositionResult"];
            var fx = (List<double>)result["Fx"];
            var fy = (List<double>)result["Fy"];
            var fz = (List<double>)result["Fz"];
            var mx = (List<double>)result["Mx"];
            var my = (List<double>)result["My"];
            var mz = (List<double>)result["Mz"];

            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            var elementIdTree = new List<List<string>> ();
            var positionResultTree = new List<List<double>>();
            var fxTree = new List<List<double>>();
            var fyTree = new List<List<double>>();
            var fzTree = new List<List<double>>();
            var mxTree = new List<List<double>>();
            var myTree = new List<List<double>>();
            var mzTree = new List<List<double>>();



            foreach (var id in uniqueId)
            {
                var elementIdTreeTemp = new List<string>();
                var positionResultTreeTemp = new List<double>();
                var fxTreeTemp = new List<double>();
                var fyTreeTemp = new List<double>();
                var fzTreeTemp = new List<double>();
                var mxTreeTemp = new List<double>();
                var myTreeTemp = new List<double>();
                var mzTreeTemp = new List<double>();


                // indexes where the uniqueId matches in the list
                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                foreach (int index in indexes)
                {
                    elementIdTreeTemp.Add(elementId.ElementAt(index));
                    positionResultTreeTemp.Add(positionResult.ElementAt(index));

                    fxTreeTemp.Add(fx.ElementAt(index));
                    fyTreeTemp.Add(fy.ElementAt(index));
                    fzTreeTemp.Add(fz.ElementAt(index));
                    mxTreeTemp.Add(mx.ElementAt(index));
                    myTreeTemp.Add(my.ElementAt(index));
                    mzTreeTemp.Add(mz.ElementAt(index));
                }

                elementIdTree.Add(elementIdTreeTemp);
                positionResultTree.Add(positionResultTreeTemp);

                fxTree.Add(fxTreeTemp);
                fyTree.Add(fyTreeTemp);
                fzTree.Add(fzTreeTemp);
                mxTree.Add(mxTreeTemp);
                myTree.Add(myTreeTemp);
                mzTree.Add(mzTreeTemp);
            }

            return new Dictionary<string, object>
            {
                {"CaseIdentifier", uniqueLoadCase},
                {"ElementId", elementIdTree},
                {"PositionResult", positionResultTree},
                {"Fx", fxTree},
                {"Fy", fyTree},
                {"Fz", fzTree},
                {"Mx", mxTree},
                {"My", myTree},
                {"Mz", mzTree}
            };
        }
    }
}