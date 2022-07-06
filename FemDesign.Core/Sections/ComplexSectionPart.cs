// https://strusoft.com/

using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Sections
{
    /// <summary>
    /// Reference type section used in entity definition. References to database section.
    /// strusoft.xsd: complex_section_type (child of)
    /// </summary>
    [System.Serializable]
    public partial class ComplexSectionPart
    {
        [XmlAttribute("pos")]
        public string _pos;
        [XmlIgnore]
        public double Pos
        {
            get
            {
                return double.Parse(this._pos);
            }
            set
            {
                this._pos = RestrictedDouble.NonNegMax_1(value).ToString(CultureInfo.InvariantCulture);
            }
        }
        [XmlAttribute("guid")]
        public System.Guid SectionRef { get; set; }
        [XmlIgnore]
        public Sections.Section SectionObj;
        [XmlElement("ecc")]
        public Bars.Eccentricity Eccentricity { get; set; }
        [XmlElement("end")]
        public string End { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ComplexSectionPart()
        {

        }

        /// <summary>
        /// Constructor to create a ModelSection.
        /// </summary>
        /// <param name="pos">Position parameter (0-1).</param>
        /// <param name="section">Cross-section at pos.</param>
        /// <param name="eccentricity">Eccentricity at pos.</param>
        internal ComplexSectionPart(double pos, Section section, Bars.Eccentricity eccentricity)
        {
            this.Pos = pos;
            this.SectionRef = section.Guid;
            this.SectionObj = section;
            this.Eccentricity = eccentricity;
            this.End = "";
        }
    }
}