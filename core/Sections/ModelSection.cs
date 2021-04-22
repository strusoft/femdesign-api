// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Reference type section used in entity definition. References to database section.
    /// strusoft.xsd: complex_section_type (child of)
    /// </summary>
    [System.Serializable]
    public class ModelSection
    {
        [XmlAttribute("pos")]
        public string Pos { get; set; }
        [XmlAttribute("guid")]
        public System.Guid SectionRef { get; set; }
        [XmlElement("ecc")]
        public Bars.Eccentricity Eccentricity { get; set; }
        [XmlElement("end")]
        public string End { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ModelSection()
        {

        }

        /// <summary>
        /// Constructor to create a ModelSection.
        /// </summary>
        /// <param name="pos">Position parameter (0-1).</param>
        /// <param name="section">Cross-section at pos.</param>
        /// <param name="eccentricity">Eccentricity at pos.</param>
        internal ModelSection(int pos, Section section, Bars.Eccentricity eccentricity)
        {
            this.Pos = pos.ToString();
            this.SectionRef = section.Guid;
            this.Eccentricity = eccentricity;
            this.End = "";
        }
    }
}