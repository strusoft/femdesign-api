// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using FemDesign.GenericClasses;
using FemDesign.Geometry;
using FemDesign.Releases;



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
        public Releases.RigidityDataType1 Rigidity { get; set; } 

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

        [XmlIgnore]
        [Obsolete("Use Plane", true)]
        private Geometry.CoordinateSystem CoordinateSystem;

        [XmlElement("local_system", Order = 5)]
        public Geometry.Plane Plane { get; set; }

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

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private SurfaceConnection()
        {

        }

        /// <summary>
        /// Create a surface connection between 2 or more surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity.
        /// </summary>
        public SurfaceConnection(Region region, RigidityDataType1 rigidity, GuidListType[] references, string identifier = "CS")
        {
            this.Initialize(region, rigidity, references, identifier);
        }

        /// <summary>
        /// Create a surface connection between 2 or more surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity.
        /// </summary>
        public SurfaceConnection(Region region, RigidityDataType1 rigidity, GuidListType[] references, double distance = 0, string identifier = "CS")
        {
            this.Initialize(region, rigidity, references, identifier);
            this.Distance = distance;
        }

        /// <summary>
        /// Create a surface connection between 2 or more surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity.
        /// </summary>
        public SurfaceConnection(Region region, RigidityDataType1 rigidity, GuidListType[] references, double interfaceAttribute = 0, double distance = 0, string identifier = "CS")
        {
            this.Initialize(region, rigidity, references, identifier);
            this.Interface = interfaceAttribute;
            this.Distance = distance;
        }

        /// <summary>
        /// Create a surface connection between 2 or more surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity (motions).
        /// </summary>
        public SurfaceConnection(Region region, Motions motions, GuidListType[] references, string identifier = "CS")
        {
            RigidityDataType1 rigidity = new RigidityDataType1(motions);
            this.Initialize(region, rigidity, references, identifier);
        }

        /// <summary>
        /// Create a surface connection between 2 or more surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity (motions and platic limits).
        /// </summary>
        public SurfaceConnection(Region region, Motions motions, MotionsPlasticLimits motionsPlasticLimits, GuidListType[] references, string identifier = "CS")
        {
            RigidityDataType1 rigidity = new RigidityDataType1(motions, motionsPlasticLimits);
            this.Initialize(region, rigidity, references, identifier);
        }

        private void Initialize(Region region, RigidityDataType1 rigidity, GuidListType[] references, string identifier)
        {
            this.EntityCreated();
            this.Region = region;
            this.Plane = region.Plane;
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
        }
    }
}