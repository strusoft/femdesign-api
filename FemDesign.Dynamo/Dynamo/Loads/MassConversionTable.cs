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
        /// Define a MassConversionTable from a LoadCase or a list of LoadCases. 
        /// The MassConversionTable is considered as a load type and should be added to the model as a load.
        /// </summary>
        /// <param name="loadCases">LoadCase to include in MassConversionTable. Single LoadCase or list of LoadCases. </param>
        /// <param name="factors">Factor for mass conversion of each respective LoadCase. Single value or list of values.</param>
        /// <returns>
        /// MassConversionTable. The MassConversionTable is considered as a load type
        /// and should be added to the model as a load.
        /// </returns>
        [IsVisibleInDynamoLibrary(true)]
        public static MassConversionTable Define(List<LoadCase> loadCases, List<double> factors)
        {
            return new MassConversionTable(factors, loadCases);
        }
        #endregion
    }
}