using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class HiddenBar: EntityBase, IStructureElement
    {   
        
        private static int _instance = 0;

        [XmlElement("rectangle", Order = 1)]
        public Geometry.RectangleType Rectangle { get; set; }

        [XmlElement("buckling_data", Order = 2)]
        public Bars.Buckling.BucklingData BucklingData { get; set; }

        [XmlElement("end", Order = 3)]
        public string End = "";

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
                HiddenBar._instance++;
                this._identifier = RestrictedString.Length(value, 50) + HiddenBar._instance.ToString();
            }
        }

        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }
    }
}
