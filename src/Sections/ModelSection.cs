// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{
    /// <summary>
    /// Reference type section used in entity definition. References to database section.
    /// strusoft.xsd: complex_section_type (child of)
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ModelSection
    {
        [XmlAttribute("pos")]
        public string pos { get; set; }
        [XmlAttribute("guid")]
        public System.Guid guid { get; set; }
        [XmlElement("ecc")]
        public Bars.Eccentricity ecc { get; set; }
        [XmlElement("end")]
        public string end { get; set; }

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
            this.pos = pos.ToString();
            this.guid = section.guid;
            this.ecc = eccentricity;
            this.end = "";
        }
    }
}