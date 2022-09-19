using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Shells
{
    [System.Serializable]
    public partial class WallCorbel: EntityBase, IStructureElement
    {
        [XmlElement("start_point", Order = 1)]
        public Geometry.Point3d StartPoint { get; set; }

        [XmlElement("end_point", Order = 2)]
        public Geometry.Point3d EndPoint { get; set; }

        [XmlElement("connectable_parts", Order = 3)]
        public TwoGuidListType ConnectableParts { get; set; }
        
        // choice rigidity
        [XmlElement("rigidity", Order = 4)]
        public Releases.RigidityDataType3 Rigidity { get; set; }

        [XmlElement("predefined_rigidity", Order = 5)]
        public GuidListType _predefRigidityRef; // reference_type

        [XmlIgnore]
        public Releases.RigidityDataLibType2 _predefRigidity;

        [XmlIgnore]
        public Releases.RigidityDataLibType2 PredefRigidity
        {
            get
            {
                return this._predefRigidity;
            }
            set
            {
                this._predefRigidity = value;
                this._predefRigidityRef = new GuidListType(value.Guid);
            }
        }

        // choice rigidity group
        // [XmlElement("rigidity_group", Order = 6)]

        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlIgnore]
        public string Instance
        {
            get
            {
                var found = this.Name.IndexOf(".");
                return this.Name.Substring(found + 1);
            }
        }
        public string Identifier => this.Name.Split('.')[0];


        [XmlAttribute("positive_side")]
        public bool PositiveSide { get; set; } = true;

        [XmlAttribute("l")]
        public double L { get; set; }

        [XmlAttribute("h1")]
        public double H1 { get; set; }

        [XmlAttribute("h2")]
        public double H2 { get; set; }

        [XmlAttribute("x")]
        public double X { get; set; }

        [XmlAttribute("base_wall")]
        public System.Guid BaseWall { get; set; }

        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial { get; set; }
    }
}