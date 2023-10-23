using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class HiddenBar: NamedEntityBase, IStructureElement
    {   
        
        private static int _hiddenBarInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_hiddenBarInstances;

        [XmlElement("rectangle", Order = 1)]
        public Geometry.RectangleType Rectangle { get; set; }

        [XmlElement("buckling_data", Order = 2)]
        public Bars.Buckling.BucklingData BucklingData { get; set; }

        [XmlElement("end", Order = 3)]
        public string End = "";

        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }
    }
}
