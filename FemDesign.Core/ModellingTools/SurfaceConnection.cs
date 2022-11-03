using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class SurfaceConnection: NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _surfaceConnectionInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_surfaceConnectionInstances;

        [XmlElement("region", Order = 1)]
        public Geometry.Region Region { get; set; }
        
        // choice rigidity data
        [XmlElement("rigidity", Order = 2)]
        public Releases.RigidityDataType2 Rigidity { get; set; } 

        [XmlElement("predefined_rigidity", Order = 3)]
        public GuidListType _predefRigidityRef;

        [XmlIgnore]
        public Releases.RigidityDataLibType1 _predefRigidity;

        [XmlIgnore]
        public Releases.RigidityDataLibType1 PredefRigidity
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

        [XmlElement("ref", Order = 4)]
        public GuidListType[] References { get; set; }

        [XmlElement("local_system", Order = 5)]
        public Geometry.CoordinateSystem CoordinateSystem { get; set; }

        [XmlAttribute("distance")]
        public double _distance;

        [XmlIgnore]
        public double Distance
        {
            get
            {
                return this._distance;
            }
            set
            {
                this.Distance = RestrictedDouble.AbsMax_10000(value);
            }
        }

        [XmlAttribute("interface")]
        public double _interface;

        [XmlIgnore]
        public double Interface
        {
            get
            {
                return this._interface;
            }
            set
            {
                this._interface = RestrictedDouble.NonNegMax_1(value);
            }
        }
        
    }
}