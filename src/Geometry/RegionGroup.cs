// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// region_group_type
    /// </summary>
    [System.Serializable]
    public class RegionGroup
    {
        [XmlElement("region")]
        public List<Region> region = new List<Region>(); // sequence: region_type
    }
}