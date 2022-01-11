using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class Diaphragm: EntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _instance = 0;

        [XmlElement("region", Order = 1)]
        public Geometry.Region Region { get; set; }

        [XmlAttribute("name")]
        public string _identifier;

        [XmlIgnore]
        public string Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                Diaphragm._instance++;
                this._identifier = $"{RestrictedString.Length(value, 50)}.{_instance}";
            }
        }

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
