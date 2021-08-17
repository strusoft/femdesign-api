// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Geometry;
using FemDesign.Releases;

namespace FemDesign.Supports
{
    /// <summary>
    /// point_support_type
    /// </summary>
    [System.Serializable]
    public partial class PointSupport : EntityBase, IStructureElement, ISupportElement
    {
        [XmlIgnore]
        public static int _instance = 0; // used for PointSupports and LineSupports
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier

        [XmlIgnore]
        private Group group;
        [XmlElement("group", Order = 1)]
        public Group Group {
            get => group;
            set {
                group = value;
                if (value != null) directed = null;
            }
        }

        [XmlIgnore]
        private Directed directed;
        [XmlElement("directed", Order = 2)]
        public Directed Directed {
            get => directed;
            set {
                directed = value;
                if (value != null) group = null;
            }
        }

        [XmlElement("position", Order = 3)]
        public FdPoint3d Position { get; set; } // point_type_3d
        public Motions Motions { get { return Group?.Rigidity?.Motions; } }
        public MotionsPlasticLimits MotionsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitForces; } }
        public Rotations Rotations { get { return Group?.Rigidity?.Rotations; } }
        public RotationsPlasticLimits RotationsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitMoments; } }

        private PointSupport()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// Create a 1-dimensional directed point support. Recomended to use 3-dimensional supports. These are constructor with <see cref="Releases.Motions"/> and <see cref="Releases.Rotations"/> parameters.
        /// </summary>
        /// <param name="point">Position of the support. </param>
        /// <param name="direction">Positive direction of the support. </param>
        /// <param name="type">Motion or rotation stiffness. </param>
        /// <param name="pos">Support stiffness in positive direction. [kN/m] or [kNm/�]</param>
        /// <param name="neg">Support stiffness in negative direction. [kN/m] or [kNm/�]</param>
        /// <param name="posPlastic">Plastic limit in positive direction. [kN] or [kNm]</param>
        /// <param name="negPlastic">Plastic limit in negative direction. [kN] or [kNm]</param>
        /// <param name="identifier">Name.</param>
        public PointSupport(FdPoint3d point, FdVector3d direction, MotionType type, double pos, double neg, double posPlastic = 0.0, double negPlastic = 0.0, string identifier = "S")
        {
            Initialize(point, null, identifier);
            this.Directed = new Directed(direction, type, pos, neg, posPlastic, negPlastic);
        }

        /// <summary>
        /// PointSupport at point with rigidity (motions, rotations). Group aligned with global coordinate system.
        /// </summary>
        /// <param name="point">Position of the support. </param>
        /// <param name="motions">Motions stiffnessess. </param>
        /// <param name="rotations">Rotation stiffnessess. </param>
        /// <param name="identifier">Name.</param>
        public PointSupport(FdPoint3d point, Motions motions, Rotations rotations, string identifier = "S")
        {
            var group = new Group(FdVector3d.UnitX(), FdVector3d.UnitY(), motions, rotations);
            Initialize(point, group, identifier);
        }

        /// <summary>
        /// PointSupport at point with rigidity (motions, rotations) and plastic limits (forces, moments). Group aligned with global coordinate system.
        /// </summary>
        /// <param name="point">Position of the support. </param>
        /// <param name="motions">Motions stiffnessess. </param>
        /// <param name="motionsPlasticLimits">Motions plastic limit forces. </param>
        /// <param name="rotations">Rotation stiffnessess. </param>
        /// <param name="rotationsPlasticLimits">Rotation plastic limit moments. </param>
        /// <param name="identifier">Name.</param>
        public PointSupport(FdPoint3d point, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, string identifier = "S")
        {
            var group = new Group(FdVector3d.UnitX(), FdVector3d.UnitY(), motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(point, group, identifier);
        }

        private void Initialize(FdPoint3d point, Group group, string identifier)
        {
            PointSupport._instance++;
            this.EntityCreated();
            this.Name = $"{identifier}.{_instance}";
            this.Group = group;
            this.Position = point;
        }

        /// <summary>
        /// Rigid PointSupport at point.
        /// </summary>
        /// <param name="point">Position of the support. </param>
        /// <param name="identifier">Name.</param>
        public static PointSupport Rigid(FdPoint3d point, string identifier = "S")
        {
            Motions motions = Motions.RigidPoint();
            Rotations rotations = Rotations.RigidPoint();
            return new PointSupport(point, motions, rotations, identifier);
        }

        /// <summary>
        /// Hinged PointSupport at point.
        /// </summary>
        /// <param name="point">Position of the support. </param>
        /// <param name="identifier">Name.</param>
        public static PointSupport Hinged(FdPoint3d point, string identifier = "S")
        {
            Releases.Motions motions = Releases.Motions.RigidPoint();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new PointSupport(point, motions, rotations, identifier);
        }
    }
}