// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.LineConnectionTypes
{
    /// <summary>
    /// line_connection_types
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class LineConnectionTypes
    {
        [XmlElement("predefined_type")]
        public List<PredefinedType> PredefinedType = new List<PredefinedType>(); // sequence: rigidity_datalib_type3
    }
}