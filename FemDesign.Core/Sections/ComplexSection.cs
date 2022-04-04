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

        public void CheckStartEnd()
        {
            if (this.Parts[0].Pos != 0)
            {
                throw new System.Exception($"First position of complex section must be 0 but is: {this.Parts[0].Pos}");
            }
            if (this.Parts.Last().Pos != 1)
            {
                throw new System.Exception($"Last position of complex section must be 1 but is: {this.Parts[0].Pos}");
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
        internal ComplexSection(Section startSection, Section endSection, Bars.Eccentricity startEccentricity, Bars.Eccentricity endEccentricity)
        {
            this.EntityCreated();
            this.Parts.Add(new ComplexSectionPart(0, startSection, startEccentricity));
            this.Parts.Add(new ComplexSectionPart(1, endSection, endEccentricity));
        }

        /// <summary>
        /// Create complex section from sections and eccentricity
        /// </summary>
        internal ComplexSection(Section[] sections, Bars.Eccentricity[] eccentricities)
        {
            // sections
            var _sections = new Section[2];
            if (sections.Length == 1)
            {
                // create new list with start/end section
                _sections = new Section[2]{ sections[0], sections[0] };
            }
            else if (sections.Length == 2)
            {
                _sections = sections;
            }
            else
            {
                throw new System.ArgumentException($"Length of sections: {sections.Length}, must be 1 (uniform) or 2 (start/end section)");
            }

            // eccentricities
            var _eccentricities = new Bars.Eccentricity[2];
            if (eccentricities.Length == 1)
            {
                // create new list with start/end section
                _eccentricities = new Bars.Eccentricity[2]{ eccentricities[0], eccentricities[0] };
            }
            else if (eccentricities.Length == 2)
            {
                _eccentricities = eccentricities;
            }
            else
            {
                throw new System.ArgumentException($"Length of eccentricities: {eccentricities.Length}, must be 1 (uniform) or 2 (start/end eccentricity)");
            }

            // construct complex section
            var positions = new double[2]{0, 1};
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

        internal ComplexSection(Section[] sections, double[] positions, Bars.Eccentricity[] eccentricities)
        {
            if (sections.Length < 2)
            {
                throw new System.ArgumentException($"Number of sections: {sections.Length}, must be 2 or more");
            }
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

            // check parts
            this.CheckStartEnd();
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