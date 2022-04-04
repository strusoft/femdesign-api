// https://strusoft.com/

using System;
using System.Xml.Serialization;



namespace StruSoft.Interop.StruXml.Data
{
    /// <summary>
    /// Complex section.
    /// strusoft.xsd: complex_section_type
    /// </summary>
    public partial class Composite_part_type
    {
        [XmlIgnore]
        public FemDesign.Materials.Material MaterialObj { get; set; }

        [XmlIgnore]
        public FemDesign.Sections.Section SectionObj  { get; set; }


    }
}