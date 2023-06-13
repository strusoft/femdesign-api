using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StruSoft.Interop.StruXml.Data;


namespace FemDesign.Loads
{
    [System.Serializable]
    public class RelationTable
    {
        [XmlElement("record")]
        public List<LoadGroupRelationRecords> LoagGroupRelationRecords { get; set; }

        private RelationTable() { }
    }

    [System.Serializable]
    public class LoadGroupRelationRecords
    {
        [XmlAttribute("name")]
        public List<string> Name { get; set; }
        [XmlAttribute("factors")]
        public List<double> Factors { get; set; }

        private LoadGroupRelationRecords() { }
    }

}
