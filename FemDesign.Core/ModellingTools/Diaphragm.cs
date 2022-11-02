using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class Diaphragm: NamedEntityBase, IStructureElement, IStageElement
    {
        [XmlIgnore]
        private static int _diaphragmInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_diaphragmInstances;

        [XmlElement("region", Order = 1)]
        public Geometry.Region Region { get; set; }

        [XmlAttribute("name")]
        public string _name;

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        private Diaphragm()
        {

        }

        public Diaphragm(Geometry.Region region, string identifier)
        {
            // create entity
            this.EntityCreated();

            // add properties
            this.Region = region;
            this.Identifier = identifier;
        }        
    }
}
