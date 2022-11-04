using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using FemDesign.GenericClasses;
using FemDesign.Releases;
using FemDesign.Geometry;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class ConnectedPoints : NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _connectedPointInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_connectedPointInstances;

        [XmlElement("point", Order = 1)]
        public Point3d[] _points;

        [XmlIgnore]
        public Point3d[] Points
        {
            get
            {
                return this._points;
            }
            set
            {
                if (value.Length == 2)
                {
                    this._points = value;
                }
                else
                {
                    throw new System.ArgumentException($"Length of points: {value.Length}, should be 2.");
                }
            }
        }

        [XmlElement("local_x", Order = 2)]
        public Vector3d LocalX { get; set; }

        [XmlElement("local_y", Order = 3)]
        public Vector3d LocalY { get; set; }

        // rigidity data choice

        [XmlElement("rigidity", Order = 4)]
        public RigidityDataType2 Rigidity { get; set; }

        [XmlElement("predefined_rigidity", Order = 5)]
        public GuidListType _predefRigidityRef;

        [XmlIgnore]
        public RigidityDataLibType2 _predefRigidity;

        [XmlIgnore]
        public RigidityDataLibType2 PredefRigidity
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


        [XmlElement("ref", Order = 6)]
        public GuidListType[] References { get; set; }

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
        private ConnectedPoints()
        {

        }

        /// <summary>
        /// Create a connected point between 2 points using rigidity. 
        /// </summary>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, RigidityDataType2 rigidity, GuidListType[] references, string identifier = "CP")
        {
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations). 
        /// </summary>
        /// <param name="firstPoint">First connected point</param>
        /// <param name="secondPoint">Second connected point</param>
        /// <param name="motions">Motions rigidity</param>
        /// <param name="rotations">Rotations rigidity</param>
        /// <param name="references">Reference element</param>
        /// <param name="identifier">Name of connected point</param>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, Rotations rotations, IEnumerable<EntityBase> references, string identifier = "CP")
        {
            GuidListType[] refs = references.Select(r => new GuidListType(r)).ToArray();
            RigidityDataType2 rigidity = new RigidityDataType2(motions, rotations);
            Initialize(firstPoint, secondPoint, rigidity, refs, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations). 
        /// </summary>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, Rotations rotations, GuidListType[] references, string identifier = "CP")
        {
            RigidityDataType2 rigidity = new RigidityDataType2(motions, rotations);
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations) and plastic limits (forces, moments). 
        /// </summary>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, IEnumerable<EntityBase> references, string identifier = "CP")
        {
            GuidListType[] refs = references.Select(r => new GuidListType(r)).ToArray();
            RigidityDataType2 rigidity = new RigidityDataType2(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(firstPoint, secondPoint, rigidity, refs, identifier);
        }

        /// <summary>
        /// Create a connected point between 2 points with rigidity (motions, rotations) and plastic limits (forces, moments). 
        /// </summary>
        public ConnectedPoints(Point3d firstPoint, Point3d secondPoint, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, GuidListType[] references, string identifier = "CP")
        {
            RigidityDataType2 rigidity = new RigidityDataType2(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(firstPoint, secondPoint, rigidity, references, identifier);
        }

        private void Initialize(Point3d firstPoint, Point3d secondPoint, RigidityDataType2 rigidity, GuidListType[] references, string identifier)
        {
            this.EntityCreated();
            this.Points = new Point3d[2]
            {
                firstPoint,
                secondPoint
            };
            this.LocalX = Vector3d.UnitX;
            this.LocalY = Vector3d.UnitY;
            this.Rigidity = rigidity;
            this.References = references;
            this.Identifier = identifier;
        }
    }
}
