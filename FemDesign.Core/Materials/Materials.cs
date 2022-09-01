// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    /// <summary>
    /// materials
    /// </summary>
    [System.Serializable]
    public partial class Materials
    {
        [XmlElement("material", Order = 1)]
        public List<Material> Material = new List<Material>(); // sequence: material_type

    }
}