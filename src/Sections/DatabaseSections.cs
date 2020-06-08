// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Sections container used in database.
    /// </summary>
    public class DatabaseSections
    {
        [XmlElement("section")]
        public List<Section> section = new List<Section>();
    }
}