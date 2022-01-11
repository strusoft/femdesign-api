using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.AuxiliaryResults
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only.
    /// </summary>
    [System.Serializable]
    public partial class LabelledSectionsGeometry
    {
        [XmlElement("section_geometry", Order = 1)]
        public List<LabelledSection> LabelledSections = new List<LabelledSection>();
    }
}