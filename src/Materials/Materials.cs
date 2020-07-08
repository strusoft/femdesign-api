// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// materials
    /// </summary>
    [System.Serializable]
    public class Materials
    {
        [XmlElement("material", Order = 1)]
        public List<Material> material = new List<Material>(); // sequence: material_type
    }
}