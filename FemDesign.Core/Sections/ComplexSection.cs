// https://strusoft.com/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Complex section.
    /// strusoft.xsd: complex_section_type
    /// </summary>
    [System.Serializable]
    public partial class ComplexSection : EntityBase
    {
        [XmlElement("section")]
        public List<ComplexSectionPart> Parts = new List<ComplexSectionPart>();
        public Sections.Section[] Sections
        {
            get
            {
                return this.Parts.Select(x => x.SectionObj).ToArray();
            }
            set
            {
                if (this.Sections.Length == value.Length)
                {
                    for (int idx = 0; idx < value.Length; idx++)
                    {
                        this.Parts[idx].SectionRef = value[idx].Guid;
                        this.Parts[idx].SectionObj = value[idx];
                    }
                }
            }
        }
        public double[] Positions
        {
            get
            {
                return this.Parts.Select(x => x.Pos).ToArray();
            }
        }
        public Bars.Eccentricity[] Eccentricities
        {
            get
            {
                return this.Parts.Select(x => x.Eccentricity).ToArray();
            }
            set
            {

            }
        }

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
            this.Parts.Add(new ComplexSectionPart(0, section, eccentricity));
            this.Parts.Add(new ComplexSectionPart(1, section, eccentricity));
        }

        /// <summary>
        /// Create complex section from a start and end section and eccentricity.
        /// </summary>
        /// <param name="section">Cross-section</param>
        /// <param name="eccentricity">Eccentricity</param>
        internal ComplexSection(Section startSection, Section endSection, Bars.Eccentricity startEccentricity, Bars.Eccentricity endEccentricity)
        {
            this.EntityCreated();
            this.Parts.Add(new ComplexSectionPart(0, startSection, startEccentricity));
            this.Parts.Add(new ComplexSectionPart(1, endSection, endEccentricity));
        }

        /// <summary>
        /// Create complex section from section, position and eccentricity. 
        /// </summary>
        /// <param name="sections">Cross-section</param>
        /// <param name="eccentricities">Eccentricity</param>
        internal ComplexSection(Section[] sections, double[] positions, Bars.Eccentricity[] eccentricities)
        {
            if (sections.Length == positions.Length && positions.Length == eccentricities.Length)
            {
                this.EntityCreated();
                for (int idx = 0; idx < sections.Length; idx++)
                {
                    this.Parts.Add(new ComplexSectionPart(positions[idx], sections[idx], eccentricities[idx]));
                }
            }
            else
            {
                throw new System.ArgumentException($"Input arguments have different length. sections: {sections.Length}, positions: {positions.Length}, eccentricities: {eccentricities.Length}");
            }
        }


        /// <summary>
        /// Construct a complex section from a list of ModelSections
        /// </summary>
        /// <param name="complexSectionParts">List of model sections</param>
        internal ComplexSection(List<ComplexSectionPart> complexSectionParts)
        {
            this.EntityCreated();
            this.Parts = complexSectionParts;
        }
    }
}