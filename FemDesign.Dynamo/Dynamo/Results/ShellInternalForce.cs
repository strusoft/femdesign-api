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
    public partial class ShellInternalForce : IResult
    {
        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "ShellInternalForce";
        }


        /// <summary>
        /// Deconstruct the Shell Internal Force Results
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="CaseCombName">Name of Load Case/Load Combination for which to return the results. Default value returns the results for the first load case/combination</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "ElementId", "NodeId", "Mx", "My", "Mxy", "Nx", "Ny", "Nxy", "Txz", "Tyz"})]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.ShellInternalForce> Result, string CaseCombName)
        {
            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.ShellInternalForce.DeconstructShellInternalForce(Result, CaseCombName);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];

            var identifier = (List<string>)result["Identifier"];
            var elementId = (List<int>)result["ElementId"];
            var nodeId = (List<int?>)result["NodeId"];

            var mx = (List<double>)result["Mx"];
            var my = (List<double>)result["My"];
            var mxy = (List<double>)result["Mxy"];
            var nx = (List<double>)result["Nx"];
            var ny = (List<double>)result["Ny"];
            var nxy = (List<double>)result["Nxy"];
            var txz = (List<double>)result["Txz"];
            var tyz = (List<double>)result["Tyz"];


            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();

            // Convert Data in NestedLst structure
            var elementIdTree = new List<List<int>>();
            var nodeIdTree = new List<List<int?>>();

            var mxTree = new List<List<double>>();
            var myTree = new List<List<double>>();
            var mxyTree = new List<List<double>>();
            var nxTree = new List<List<double>>();
            var nyTree = new List<List<double>>();
            var nxyTree = new List<List<double>>();
            var txzTree = new List<List<double>>();
            var tyzTree = new List<List<double>>();



            foreach (var id in uniqueId)
            {

                // temporary List to append
                var elementIdTreeTemp = new List<int>();
                elementIdTreeTemp.Add(id);


                var nodeIdTreeTemp = new List<int?>();

                var mxTreeTemp = new List<double>();
                var myTreeTemp = new List<double>();
                var mxyTreeTemp = new List<double>();
                var nxTreeTemp = new List<double>();
                var nyTreeTemp = new List<double>();
                var nxyTreeTemp = new List<double>();
                var txzTreeTemp = new List<double>();
                var tyzTreeTemp = new List<double>();



                // indexes where the uniqueId matches in the list
                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                foreach (int index in indexes)
                {
                    nodeIdTreeTemp.Add(nodeId.ElementAt(index));
                    mxTreeTemp.Add(mx.ElementAt(index));
                    myTreeTemp.Add(my.ElementAt(index));
                    mxyTreeTemp.Add(mxy.ElementAt(index));
                    nxTreeTemp.Add(nx.ElementAt(index));
                    nyTreeTemp.Add(ny.ElementAt(index));
                    nxyTreeTemp.Add(nxy.ElementAt(index));
                    txzTreeTemp.Add(txz.ElementAt(index));
                    tyzTreeTemp.Add(tyz.ElementAt(index));
                }

                elementIdTree.Add(elementIdTreeTemp);
                nodeIdTree.Add(nodeIdTreeTemp);
                mxTree.Add(mxTreeTemp);
                myTree.Add(myTreeTemp);
                mxyTree.Add(mxyTreeTemp);
                nxTree.Add(nxTreeTemp);
                nyTree.Add(nyTreeTemp);
                nxyTree.Add(nxyTreeTemp);
                txzTree.Add(txzTreeTemp);
                tyzTree.Add(tyzTreeTemp);
            }



            return new Dictionary<string, dynamic>
            {
                {"CaseIdentifier", uniqueLoadCase},
                {"ElementId", elementIdTree},
                {"NodeId",nodeIdTree},
                {"Mx", mxTree},
                {"My", myTree},
                {"Mxy", mxyTree},
                {"Nx", nxTree},
                {"Ny", nyTree},
                {"Nxy", nxyTree},
                {"Txz", txzTree},
                {"Tyz", tyzTree},
            };
        }
    }
}