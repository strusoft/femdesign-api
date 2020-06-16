using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.StructureGrid
{
    public class Storeys
    {
        [XmlElement("storey", Order = 1)]
        public List<Storey> storey = new List<Storey>();
    }
}