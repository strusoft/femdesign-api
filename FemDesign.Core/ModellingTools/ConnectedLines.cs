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

        [XmlElement("local_x", Order = 3)]
        public Geometry.Vector3d LocalX { get; set; }

        [XmlElement("local_y", Order = 4)]
        public Geometry.Vector3d LocalY { get; set; }

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

        [XmlAttribute("moving_local")]
        public bool MovingLocal { get; set; }

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
        /// Constructor
        /// </summary>
        public ConnectedLines(Geometry.Edge firstEdge, Geometry.Edge secondEdge, Geometry.Vector3d localX, Geometry.Vector3d localY, Releases.RigidityDataType3 rigidity, GuidListType[] references, string identifier, bool movingLocal, double interfaceStart, double interfaceEnd)
        {
            this.EntityCreated();
            this.Edges = new Geometry.Edge[2]
            {
                firstEdge,
                secondEdge
            };
            this.LocalX = localX;
            this.LocalY = localY;
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
            this.MovingLocal = movingLocal;
            this.InterfaceStart = interfaceStart;
            this.InterfaceEnd = interfaceEnd;
        }
    }
}
