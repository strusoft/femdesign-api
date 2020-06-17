using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.StructureGrid
{
    /// <summary>
    /// Class to contain list in entities. For serialization purposes only.
    /// </summary>
    public class Storeys
    {
        [XmlElement("storey", Order = 1)]
        public List<Storey> storey = new List<Storey>();
    }
}