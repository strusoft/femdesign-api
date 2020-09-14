// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// materials
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Materials
    {
        [XmlElement("material", Order = 1)]
        public List<Material> Material = new List<Material>(); // sequence: material_type
    }
}