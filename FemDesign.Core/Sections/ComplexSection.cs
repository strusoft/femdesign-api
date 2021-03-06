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
    public partial class ComplexSection: EntityBase
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
            this.EntityCreated();
            this.Section.Add(new ModelSection(0, section, eccentricity));
            this.Section.Add(new ModelSection(1, section, eccentricity));
        }

        /// <summary>
        /// Create complex section from a start and end section and eccentricity.
        /// </summary>
        /// <param name="section">Cross-section</param>
        /// <param name="eccentricity">Eccentricity</param>
        internal ComplexSection(Section startSection, Section endSection, Bars.Eccentricity startEccentricity, Bars.Eccentricity endEccentricity)
        {
            this.EntityCreated();
            this.Section.Add(new ModelSection(0, startSection, startEccentricity));
            this.Section.Add(new ModelSection(1, endSection, endEccentricity));
        }

        /// <summary>
        /// Construct a complex sectoin from a list of ModelSections
        /// </summary>
        /// <param name="modelSections">List of model sections</param>
        internal ComplexSection(List<ModelSection> modelSections)
        {
            this.EntityCreated();
            this.Section = modelSections;
        }
    }
}