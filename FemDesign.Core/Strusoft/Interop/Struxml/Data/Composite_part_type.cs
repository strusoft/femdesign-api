using System.Xml.Serialization;
using System.Collections.Generic;
using System;
namespace StruSoft.Interop.StruXml.Data
{
    
    public partial class Composite_section_type
    {

        [XmlIgnore]
        public FemDesign.Materials.Material Material;
        
        [XmlIgnore]
        public FemDesign.Sections.Section Section;
    }

}