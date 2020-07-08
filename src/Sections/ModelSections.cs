// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{
    /// <summary>
    /// Sections container used in model.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ModelSections
    {
        [XmlElement("section", Order = 1)]
        public List<FemDesign.Sections.Section> section = new List<FemDesign.Sections.Section>(); // sequence: section_type
        [XmlElement("complex_section", Order = 2)]
        public List<FemDesign.Sections.ComplexSection> complexSection = new List<FemDesign.Sections.ComplexSection>(); // sequence: complex_section_type
    }
}