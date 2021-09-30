// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load types
    /// </summary>
    [System.Serializable]
    public partial class LoadTypes
    {
        [XmlElement("load_type", Order = 1)]
        public List<FemDesign.Loads.LoadType> LoadType = new List<FemDesign.Loads.LoadType>(); // sequence: load_type
    }
}