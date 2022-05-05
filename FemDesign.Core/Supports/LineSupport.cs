// https://strusoft.com/
using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Geometry;
using FemDesign.Releases;


namespace FemDesign.Supports
{
    /// <summary>
    /// line_support_type
    /// </summary>
    [System.Serializable]
    public partial class LineSupport: EntityBase, IStructureElement, ISupportElement
    {
        // serialization properties
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier. Default = S
        [XmlAttribute("moving_local")]
        public bool MovingLocal { get; set; } // bool
        
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
        
        [XmlElement("edge", Order = 3)]
        public Edge Edge { get; set; } // edge_type
        public Motions Motions { get { return Group?.Rigidity?.Motions; } }
        public MotionsPlasticLimits MotionsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitForces; } }
        public Rotations Rotations { get { return Group?.Rigidity?.Rotations; } }
        public RotationsPlasticLimits RotationsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitMoments; } }

        /// <summary>
        /// This property only reflects the edge normal. If this normal is changed arcs may transform.
        /// </summary>
        [XmlElement("normal", Order = 4)]
        public FdVector3d EdgeNormal;
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineSupport()
        {
            
        }

        /// <summary>
        /// Create a 1-dimensionally directed line support. Recomended to use 3-dimensional supports. These are constructor with <see cref="Releases.Motions"/> and <see cref="Releases.Rotations"/> parameters.
        /// </summary>
        /// <param name="edge">Position of the support. </param>
        /// <param name="direction">Positive direction of the support. </param>
        /// <param name="movingLocal">Keep direction along line (false) or Direction changes along line (true). </param>
        /// <param name="type">Motion or rotation stiffness. </param>
        /// <param name="pos">Support stiffness in positive direction. [kN/m] or [kNm/°]</param>
        /// <param name="neg">Support stiffness in negative direction. [kN/m] or [kNm/°]</param>
        /// <param name="posPlastic">Plastic limit in positive direction. [kN] or [kNm]</param>
        /// <param name="negPlastic">Plastic limit in negative direction. [kN] or [kNm]</param>
        /// <param name="identifier">Name.</param>
        public LineSupport(Edge edge, FdVector3d direction, bool movingLocal, MotionType type, double pos, double neg, double posPlastic = 0.0, double negPlastic = 0.0, string identifier = "S")
        {
            Initialize(edge, null, movingLocal, identifier);
            this.Directed = new Directed(direction, type, pos, neg, posPlastic, negPlastic);
        }

        /// <summary>
        /// LineSupport along edge with rigidity (motions, rotations). Group LCS aligned with edge LCS.
        /// </summary>
        /// <param name="edge">Position of the support. </param>
        /// <param name="motions">Motions stiffnessess. </param>
        /// <param name="rotations">Rotation stiffnessess. </param>
        /// <param name="movingLocal">Keep direction along line (false) or Direction changes along line (true). </param>
        /// <param name="identifier">Name.</param>
        public LineSupport(Edge edge, Motions motions, Rotations rotations, bool movingLocal, string identifier = "S")
        {
            var group = new Group(edge.CoordinateSystem, motions, rotations);
            Initialize(edge, group, movingLocal, identifier);
        }

        /// <summary>
        /// LineSupport along edge with rigidity (motions, rotations) and plastic limits (forces, moments). Group LCS aligned with edge LCS.
        /// </summary>
        /// <param name="edge">Position of the support. </param>
        /// <param name="motions">Motions stiffnessess. </param>
        /// <param name="motionsPlasticLimits">Motions plastic limit forces. </param>
        /// <param name="rotations">Rotation stiffnessess. </param>
        /// <param name="rotationsPlasticLimits">Rotation plastic limit moments. </param>
        /// <param name="movingLocal">Keep direction along line (false) or Direction changes along line (true). </param>
        /// <param name="identifier">Name.</param>
        public LineSupport(Edge edge, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, bool movingLocal, string identifier = "S")
        {
            var group = new Group(edge.CoordinateSystem, motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(edge, group, movingLocal, identifier);
        }

        private void Initialize(Edge edge, Group group, bool movingLocal, string identifier)
        {
            PointSupport._instance++; // PointSupport and LineSupport share the same instance counter.
            this.EntityCreated();
            this.Name = identifier + "." + PointSupport._instance.ToString();
            this.MovingLocal = movingLocal;

            // set edge specific properties
            this.Group = group;
            this.Edge = edge;
            this.EdgeNormal = edge.Normal;
        }

        /// <summary>
        /// Rigid LineSupport along edge.
        /// </summary>
        /// <param name="edge">Position of the support. </param>
        /// <param name="movingLocal">Keep direction along line (false) or Direction changes along line (true). </param>
        /// <param name="identifier">Name.</param>
        public static LineSupport Rigid(Edge edge, bool movingLocal, string identifier = "S")
        {
            Motions motions = Motions.RigidLine();
            Rotations rotations = Rotations.RigidLine();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

        /// <summary>
        /// Hinged LineSupport along edge.
        /// </summary>
        /// <param name="edge">Position of the support. </param>
        /// <param name="movingLocal">Keep direction along line (false) or Direction changes along line (true). </param>
        /// <param name="identifier">Name.</param>
        public static LineSupport Hinged(Edge edge, bool movingLocal, string identifier = "S")
        {
            Motions motions = Motions.RigidLine();
            Rotations rotations = Rotations.Free();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

    }
}