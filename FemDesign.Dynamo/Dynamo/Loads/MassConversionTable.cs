using System.Xml.Serialization;
using System.Collections.Generic;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class MassConversion
    {

    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class MassConversionTable
    {
        #region dynamo
        /// <summary>
        /// Define a new mass conversion table.
        /// </summary>
        /// <param name="loadCases">LoadCase to include in MassConversionTable. Single LoadCase or list of LoadCases.</param>
        /// <param name="factors">Factor for mass conversion of each respective LoadCase. Single value or list of values.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static MassConversionTable Define(List<LoadCase> loadCases, List<double> factors)
        {
            return new MassConversionTable(factors, loadCases);
        }
        #endregion
    }
}