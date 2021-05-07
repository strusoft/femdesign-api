using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{
    [System.Serializable()]
    public partial class GlcDataType
    {
        [XmlElement("layer", Order = 1)]
        public List<MechProps> Layers { get; set; }

        public GlcDataType()
        {

        }
    }
}