// https://strusoft.com/

using System;
using System.Xml.Serialization;



namespace StruSoft.Interop.StruXml.Data
{
    /// <summary>
    /// Complex section.
    /// strusoft.xsd: complex_section_type
    /// </summary>
    public partial class Composite_section_type
    {
        [XmlIgnore]
        public StruSoft.Interop.StruXml.Data.Composite_data CompositeSectionDataObj { get; set; }
    }
}