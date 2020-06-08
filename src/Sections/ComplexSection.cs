// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Complex section.
    /// strusoft.xsd: complex_section_type
    /// </summary>
    [System.Serializable]
    public class ComplexSection: EntityBase
    {
        [XmlElement("section")]
        public List<ModelSection> section = new List<ModelSection>();

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
            this.guid = System.Guid.NewGuid();
            this.lastChange = System.DateTime.UtcNow;
            this.action = "added";
            this.section.Add(new ModelSection(0, section, eccentricity));
            this.section.Add(new ModelSection(1, section, eccentricity));
        }
    }
}