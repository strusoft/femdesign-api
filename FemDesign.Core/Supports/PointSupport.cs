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
    public partial class PointSupport : EntityBase, IStructureElement, ISupportElement, IStageElement
    {
        [XmlIgnore]
        public static int _instance = 0; // used for PointSupports and LineSupports
        [XmlAttribute("name")]
        public string Identifier { get; set; } // identifier

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

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
        /// Create a Support Point oriented along defined FdCoordinateSystem provided.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="motions"></param>
        /// <param name="rotations"></param>
        /// <param name="identifier"></param>
        public PointSupport(FdCoordinateSystem plane, Motions motions, Rotations rotations, string identifier = "S")
        {
            var group = new Group(plane.LocalX, plane.LocalY, motions, rotations);
            Initialize(plane.Origin, group, identifier);
        }

        /// <summary>
        /// Create a Point Support oriented along the Global Axis X and Y.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="motions"></param>
        /// <param name="rotations"></param>
        /// <param name="identifier"></param>
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
        public PointSupport(FdCoordinateSystem plane, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, string identifier = "S")
        {
            var group = new Group(FdVector3d.UnitX(), FdVector3d.UnitY(), motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(plane, group, identifier);
        }

        /// <summary>
        /// Define a Point Support. The method automatically set the motion values to both negative and positive value.
        /// True = Fixed (1e10 kN/m, 1e10 kNm/rad) False = Free (0.00 kN/m, 0.00 kNm/rad). 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="tz"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        /// <param name="identifier"></param>
        public PointSupport(FdCoordinateSystem plane, bool tx, bool ty, bool tz, bool rx, bool ry, bool rz, string identifier = "S")
        {
            double x = tx == true ? Motions.ValueRigidPoint : 0;
            double y = ty == true ? Motions.ValueRigidPoint : 0;
            double z = tz == true ? Motions.ValueRigidPoint : 0;
            double xx = rx == true ? Rotations.ValueRigidPoint : 0;
            double yy = ry == true ? Rotations.ValueRigidPoint : 0;
            double zz = rz == true ? Rotations.ValueRigidPoint : 0;

            var motions = new Motions(x, x, y, y, z, z);
            var rotations = new Rotations(xx, xx, yy, yy, zz, zz);

            var group = new Group(plane.LocalX, plane.LocalY, motions, rotations);
            Initialize(plane.Origin, group, identifier);
        }

        private void Initialize(FdCoordinateSystem point, Group group, string identifier)
        {
            PointSupport._instance++;
            this.EntityCreated();
            this.Identifier = $"{identifier}.{_instance}";
            this.Group = group;
            this.Position = point;
        }

        /// <summary>
        /// Rigid PointSupport at point.
        /// </summary>
        /// <param name="plane">Position of the support. </param>
        /// <param name="identifier">Name.</param>
        public static PointSupport Rigid(FdCoordinateSystem plane, string identifier = "S")
        {
            Motions motions = Motions.RigidPoint();
            Rotations rotations = Rotations.RigidPoint();
            return new PointSupport(plane, motions, rotations, identifier);
        }

        /// <summary>
        /// Hinged PointSupport at point.
        /// </summary>
        /// <param name="plane">Position of the support. </param>
        /// <param name="identifier">Name.</param>
        public static PointSupport Hinged(FdCoordinateSystem plane, string identifier = "S")
        {
            Releases.Motions motions = Releases.Motions.RigidPoint();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new PointSupport(plane, motions, rotations, identifier);
        }

        public override string ToString()
        {
            bool hasPlasticLimit = false;
            if (this.Group.Rigidity != null)
            {
                if (this.Group.Rigidity.PlasticLimitForces != null || this.Group.Rigidity.PlasticLimitMoments != null)
                    hasPlasticLimit = true;
                return $"{this.GetType().Name} Pos: {this.Position}, Motion: {this.Group.Rigidity.Motions}, Rotation: {this.Group.Rigidity.Rotations}, PlasticLimit: {hasPlasticLimit}";
            }
            else
                return $"{this.GetType().Name} with RigidityGroup";
        }
    }
}