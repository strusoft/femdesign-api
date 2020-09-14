// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{
    /// <summary>
    /// Complex section.
    /// strusoft.xsd: complex_section_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ComplexSection: EntityBase
    {
        [XmlElement("section")]
        public List<ModelSection> Section = new List<ModelSection>();

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ComplexSection()
        {

        }

        /// <summary>
        /// Create simple complex section from section and eccentricity. Equal cross-section and eccentricity at both ends of bar.
        /// </summary>
        /// <param name="section">Cross-section</param>
        /// <param name="eccentricity">Eccentricity</param>
        internal ComplexSection(Section section, Bars.Eccentricity eccentricity)
        {
            this.Guid = System.Guid.NewGuid();
            this.LastChange = System.DateTime.UtcNow;
            this.Action = "added";
            this.Section.Add(new ModelSection(0, section, eccentricity));
            this.Section.Add(new ModelSection(1, section, eccentricity));
        }
    }
}