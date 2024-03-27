// https://strusoft.com/
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class ConnectedLines: NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _connectedLineInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_connectedLineInstances;

        [XmlElement("edge" , Order = 1)]
        public Geometry.Edge[] Edges { get; set; }

        /// <summary>
        /// This property is optional.
        /// Represents first interface point, i.e Point@Parameter on Line between Edge[0].StartPoint and Edge[1].StartPoint.
        /// </summary>
        [XmlElement("point", Order = 2)]
        public Geometry.Point3d[] Points { get; set; }

        [XmlIgnore]
        private Geometry.Plane _plane;

        [XmlIgnore]
        public Geometry.Plane Plane
        {
            get
            {
                if (this._plane == null)
                {
                    this._plane = new Geometry.Plane(new Geometry.Point3d(0, 0, 0), this.LocalX, this.LocalY);
                    return this._plane;
                }
                else
                {
                    return this._plane;
                }
            }
            set
            {
                this._plane = value;
                this._localX = value.LocalX;
                this._localY = value.LocalY;
            }
        }

        [XmlElement("local_x", Order = 3)]
        public Geometry.Vector3d _localX;

        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this._localX;
            }
            set
            {
                this.Plane.SetXAroundZ(value);
                this._localX = this.Plane.LocalX;
                this._localY = this.Plane.LocalY;
            }
        }

        [XmlElement("local_y", Order = 4)]
        public Geometry.Vector3d _localY;

        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this._localY;
            }
            set
            {
                this.Plane.SetYAroundZ(value);
                this._localX = this.Plane.LocalX;
                this._localY = this.Plane.LocalY;
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
                this._localX = this.Plane.LocalX;
                this._localY = this.Plane.LocalY;
            }
        }

        // simple stiffness choice

        // rigidity data choice
        [XmlElement("rigidity", Order = 5)]
        public Releases.RigidityDataType3 Rigidity { get; set; } 

        [XmlElement("predefined_rigidity", Order = 6)]
        public GuidListType _predefRigidityRef;

        [XmlIgnore]
        public Releases.RigidityDataLibType3 _predefRigidity;

        [XmlIgnore]
        public Releases.RigidityDataLibType3 PredefRigidity
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

        // rigidity group choice

        
        [XmlElement("ref", Order = 7)]
        public GuidListType[] References { get; set; }

        [XmlElement("colouring", Order = 8)]
        public EntityColor Colouring { get; set; }

        [XmlIgnore]
        private bool _movingLocal = false;

        [XmlAttribute("moving_local")]
        public bool MovingLocal 
        {
            get 
            { 
                return this._movingLocal; 
            }
            set 
            { 
                this._movingLocal = value; 
            }
        }

        [XmlIgnore]
        private double[] _interface = new double[2]{ 0.5, 0.5 };

        [XmlAttribute("interface_start")]
        public double InterfaceStart
        {
            get
            {
                return this._interface[0];
            }
            set
            {
                this._interface[0] = RestrictedDouble.NonNegMax_1(value);
            }
        }

        [XmlAttribute("interface_end")]
        public double InterfaceEnd
        {
            get
            {
                return this._interface[1];
            }
            set
            {
                this._interface[1] = RestrictedDouble.NonNegMax_1(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private ConnectedLines()
        {

        }

        /// <summary>
        /// Create a line connection
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Vector3d localX, Geometry.Vector3d localY, Releases.RigidityDataType3 rigidity, GuidListType[] references, string identifier = "CL", bool movingLocal = false, double interfaceStart = 0.5, double interfaceEnd = 0.5)
        {
            this.Initialize(firstEdge, secondEdge, localX, localY, rigidity, references, identifier, movingLocal, interfaceStart, interfaceEnd);
        }

        /// <summary>
        /// Create a line connection. Local coordinate system declared by a plane object.
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Plane localPlane, Releases.RigidityDataType3 rigidity, GuidListType[] references, string identifier = "CL", bool movingLocal = false, double interfaceStart = 0.5, double interfaceEnd = 0.5)
        {
            this.Initialize(firstEdge, secondEdge, localPlane.LocalX, localPlane.LocalY, rigidity, references, identifier, movingLocal, interfaceStart, interfaceEnd);
        }

        /// <summary>
        /// Create a line connection using motions and rotations. Local coordinate system declared by a plane object.
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Plane localPlane, Releases.Motions motions, Releases.Rotations rotations, GuidListType[] references, string identifier = "CL", bool movingLocal = false, double interfaceStart = 0.5, double interfaceEnd = 0.5)
        {
            Releases.RigidityDataType3 rigidity = new Releases.RigidityDataType3(motions, rotations);
            this.Initialize(firstEdge, secondEdge, localPlane.LocalX, localPlane.LocalY, rigidity, references, identifier, movingLocal, interfaceStart, interfaceEnd);
        }

        /// <summary>
        /// Create a line connection using motions, rotations and plastic limits. Local coordinate system declared by a plane object.
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Plane localPlane, Releases.Motions motions, Releases.MotionsPlasticLimits motionsPlasticLimits, Releases.Rotations rotations, Releases.RotationsPlasticLimits rotationPlasticLimits, GuidListType[] references, string identifier = "CL", bool movingLocal = false, double interfaceStart = 0.5, double interfaceEnd = 0.5)
        {
            Releases.RigidityDataType3 rigidity = new Releases.RigidityDataType3(motions, motionsPlasticLimits, rotations, rotationPlasticLimits);
            this.Initialize(firstEdge, secondEdge, localPlane.LocalX, localPlane.LocalY, rigidity, references, identifier, movingLocal, interfaceStart, interfaceEnd);
        }

        /// <summary>
        /// Create a line connection. Local coordinate system declared by a plane object.
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Plane localPlane, Releases.RigidityDataType3 rigidity, IEnumerable<EntityBase> elements, string identifier = "CL", bool movingLocal = false, double interfaceStart = 0.5, double interfaceEnd = 0.5)
        {
            GuidListType[] references = elements.Select(r => new GuidListType(r)).ToArray();
            this.Initialize(firstEdge, secondEdge, localPlane.LocalX, localPlane.LocalY, rigidity, references, identifier, movingLocal, interfaceStart, interfaceEnd);
        }

        /// <summary>
        /// Create a line connection using motions and rotations. Local coordinate system declared by a plane object.
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Plane localPlane, Releases.Motions motions, Releases.Rotations rotations, IEnumerable<EntityBase> elements, string identifier = "CL", bool movingLocal = false, double interfaceStart = 0.5, double interfaceEnd = 0.5)
        {
            GuidListType[] references = elements.Select(r => new GuidListType(r)).ToArray();
            Releases.RigidityDataType3 rigidity = new Releases.RigidityDataType3(motions, rotations);
            this.Initialize(firstEdge, secondEdge, localPlane.LocalX, localPlane.LocalY, rigidity, references, identifier, movingLocal, interfaceStart, interfaceEnd);
        }

        /// <summary>
        /// Create a line connection using motions, rotations and plastic limits. Local coordinate system declared by a plane object.
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Plane localPlane, Releases.Motions motions, Releases.MotionsPlasticLimits motionsPlasticLimits, Releases.Rotations rotations, Releases.RotationsPlasticLimits rotationPlasticLimits, IEnumerable<EntityBase> elements, string identifier = "CL", bool movingLocal = false, double interfaceStart = 0.5, double interfaceEnd = 0.5)
        {
            GuidListType[] references = elements.Select(r => new GuidListType(r)).ToArray();
            Releases.RigidityDataType3 rigidity = new Releases.RigidityDataType3(motions, motionsPlasticLimits, rotations, rotationPlasticLimits);
            this.Initialize(firstEdge, secondEdge, localPlane.LocalX, localPlane.LocalY, rigidity, references, identifier, movingLocal, interfaceStart, interfaceEnd);
        }

        private void Initialize(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Vector3d localX, Geometry.Vector3d localY, Releases.RigidityDataType3 rigidity, GuidListType[] references, string identifier, bool movingLocal, double interfaceStart, double interfaceEnd)
        {
            this.EntityCreated();

            this.Edges = new Geometry.Edge[2]
            {
                firstEdge,
                secondEdge
            };
            this.Plane = new Geometry.Plane(new Geometry.Point3d(0, 0, 0), localX, localY);
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
            this.MovingLocal = movingLocal;
            this.InterfaceStart = interfaceStart;
            this.InterfaceEnd = interfaceEnd;
        }
    }
}
