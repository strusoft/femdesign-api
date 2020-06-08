// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// reinforcing_materials.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class ReinforcingMaterials
    {
        [XmlElement("material", Order = 1)]
        public List<Material> material = new List<Material>(); // sequence: rfmaterial_type
    }
}