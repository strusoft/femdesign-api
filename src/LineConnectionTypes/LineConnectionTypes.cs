// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.LineConnectionTypes
{
    /// <summary>
    /// line_connection_types
    /// </summary>
    [System.Serializable]
    public class LineConnectionTypes
    {
        [XmlElement("predefined_type")]
        public List<PredefinedType> predefinedType = new List<PredefinedType>(); // sequence: rigidity_datalib_type3
    }
}