using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.StructureGrid
{
    [IsVisibleInDynamoLibrary(false)]
    public class Storeys
    {
        [XmlElement("storey", Order = 1)]
        public List<Storey> storey = new List<Storey>();
    }
}