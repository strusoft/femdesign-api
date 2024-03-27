// https://strusoft.com/
using System;
using System.Linq;
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

        [XmlElement("local_system", Order = 5)]
        public Geometry.Plane _plane;

        [XmlElement("colouring", Order = 6)]
        public EntityColor Colouring { get; set; }

        [XmlIgnore]
        public Geometry.Plane Plane
        {
            get
            {
                return this._plane;
            }
            set
            {
                this._plane = value;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this.Plane.LocalX;
            }
            set
            {
                this.Plane.SetXAroundZ(value);
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this.Plane.LocalY;
            }
            set
            {
                this.Plane.SetYAroundZ(value);
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.Plane.LocalZ;
            }
            set
            {
                this.Plane.SetZAroundX(value);
            }
        }
        
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
                this._distance = RestrictedDouble.AbsMax_10000(value);
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
        public SurfaceConnection()
        {

        }

        /// <summary>
        /// Create a surface connection between surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity.
        /// </summary>
        public SurfaceConnection(Region region, RigidityDataType1 rigidity, GuidListType[] references, string identifier = "CS", double distance = 0, double interfaceAttribute = 0)
        {
            this.Initialize(region, rigidity, references, identifier, distance, interfaceAttribute);
        }

        /// <summary>
        /// Create a surface connection between surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity (motions).
        /// </summary>
        public SurfaceConnection(Region region, Motions motions, GuidListType[] references, string identifier = "CS", double distance = 0, double interfaceAttribute = 0)
        {
            RigidityDataType1 rigidity = new RigidityDataType1(motions);
            this.Initialize(region, rigidity, references, identifier, distance, interfaceAttribute);
        }

        /// <summary>
        /// Create a surface connection between surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity (motions and platic limits).
        /// </summary>
        public SurfaceConnection(Region region, Motions motions, MotionsPlasticLimits motionsPlasticLimits, GuidListType[] references, string identifier = "CS", double distance = 0, double interfaceAttribute = 0)
        {
            RigidityDataType1 rigidity = new RigidityDataType1(motions, motionsPlasticLimits);
            this.Initialize(region, rigidity, references, identifier, distance, interfaceAttribute);
        }

        /// <summary>
        /// Create a surface connection between surface structural elements (e.g. slabs, surface supports, etc.) using elements and rigidity.
        /// </summary>
        public SurfaceConnection(Region region, RigidityDataType1 rigidity, IEnumerable<EntityBase> elements, string identifier = "CS", double distance = 0, double interfaceAttribute = 0)
        {
            GuidListType[] references = elements.Select(r => new GuidListType(r)).ToArray();
            this.Initialize(region, rigidity, references, identifier, distance, interfaceAttribute);
        }

        /// <summary>
        /// Create a surface connection between surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity (motions).
        /// </summary>
        public SurfaceConnection(Region region, Motions motions, IEnumerable<EntityBase> elements, string identifier = "CS", double distance = 0, double interfaceAttribute = 0)
        {
            RigidityDataType1 rigidity = new RigidityDataType1(motions);
            GuidListType[] references = elements.Select(r => new GuidListType(r)).ToArray();
            this.Initialize(region, rigidity, references, identifier, distance, interfaceAttribute);
        }

        /// <summary>
        /// Create a surface connection between surface structural elements (e.g. slabs, surface supports, etc.) using their GUIDs and rigidity (motions and platic limits).
        /// </summary>
        public SurfaceConnection(Region region, Motions motions, MotionsPlasticLimits motionsPlasticLimits, IEnumerable<EntityBase> elements, string identifier = "CS", double distance = 0, double interfaceAttribute = 0)
        {
            RigidityDataType1 rigidity = new RigidityDataType1(motions, motionsPlasticLimits);
            GuidListType[] references = elements.Select(r => new GuidListType(r)).ToArray();
            this.Initialize(region, rigidity, references, identifier, distance, interfaceAttribute);
        }

        private void Initialize(Region region, RigidityDataType1 rigidity, GuidListType[] references, string identifier, double distance, double interfaceAttribute)
        {
            this.EntityCreated();

            this.Region = region;
            this.Plane = region.Plane;
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
            this.Distance = distance;
            this.Interface = interfaceAttribute;
        }
    }
}