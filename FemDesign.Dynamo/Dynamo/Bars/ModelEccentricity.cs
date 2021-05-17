using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    /// This class is called ModelEccentricity as Eccentricity (ecc_value_type) is the Dynamo facing class and thus need to be named accordingly.
    [IsVisibleInDynamoLibrary(false)]
    public partial class ModelEccentricity
    {
        
    }
}