
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
        /// <param name="loadCases">LoadCases to include in load combination. Single LoadCase or list of LoadCases. Nested lists are not supported - use flatten.</param>
        /// <param name="gammas">Gamma values for respective LoadCase. Single value or list of values. Nested lists are not supported - use flatten.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LoadCombination CreateLoadCombination(string name, [DefaultArgument("\"ultimate_ordinary\"")] string type, List<LoadCase> loadCases, List<double> gammas)
        {

            var _type = FemDesign.GenericClasses.EnumParser.Parse<LoadCombType>(type);
            LoadCombination loadCombination = new LoadCombination(name, _type, loadCases, gammas);
            return loadCombination;
        }
        #endregion
    }
}