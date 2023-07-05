using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only.
    /// </summary>
    [System.Serializable]
    public partial class Regions
    {
        [XmlElement("region", Order = 1)]
        public List<Region> Region = new List<Region>();
    }
}
