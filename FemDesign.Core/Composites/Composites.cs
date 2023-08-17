// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Composites
{
    /// <summary>
    /// composites
    /// </summary>
    [System.Serializable]
    public partial class Composites
    {
        [XmlElement("composite_section", Order = 1)]
        public List<CompositeSection> CompositeSection = new List<CompositeSection>();

        [XmlElement("complex_composite", Order = 2)]
        public List<ComplexComposite> ComplexComposite = new List<ComplexComposite>();
    }
}
