// https://strusoft.com/

using System;
using System.Xml.Serialization;



namespace StruSoft.Interop.StruXml.Data
{
    /// <summary>
    /// Composite Section.
    /// strusoft.xsd: Composite_section_type
    /// </summary>
    public partial class Composite_section_type
    {
        [XmlIgnore]
        public StruSoft.Interop.StruXml.Data.Composite_data CompositeSectionDataObj { get; set; }
    }
}