
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LoadCombination: EntityBase
    {
        #region dynamo
        /// <summary>Create LoadCombination from a LoadCase or a list of LoadCases.</summary>
        /// <remarks>Create</remarks>
        /// <param name="name">Name of LoadCombination</param>
        /// <param name="type">LoadCombination type. "ultimate_ordinary"/"ultimate_accidental"/"ultimate_seismic"/"serviceability_quasi_permanent"/"serviceability_frequent"/"serviceability_characteristic"</param>
        /// <param name="loadCase">LoadCase to include in load combination. Single LoadCase or list of LoadCases. Nested lists are not supported - use flatten.</param>
        /// <param name="gamma">Gamma value for respective LoadCase. Single value or list of values. Nested lists are not supported - use flatten.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LoadCombination CreateLoadCombination(string name, [DefaultArgument("ultimate_ordinary")] string type, List<LoadCase> loadCase, List<double> gamma)
        {
            LoadCombination loadCombination = new LoadCombination(name, type, loadCase, gamma);
            return loadCombination;
        }
        #endregion
    }
}