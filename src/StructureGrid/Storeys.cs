using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.StructureGrid
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Storeys
    {
        [XmlElement("storey", Order = 1)]
        public List<Storey> Storey = new List<Storey>();
    }
}