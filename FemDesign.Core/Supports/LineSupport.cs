// https://strusoft.com/
using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
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
        [XmlElement("group", Order = 1)]
        public Group Group { get; set; } // support_rigidity_data_type
        [XmlElement("edge", Order = 2)]
        public Geometry.Edge Edge { get; set; } // edge_type
        public Motions Motions { get { return Group?.Rigidity?.Motions; } }
        public MotionsPlasticLimits MotionsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitForces; } }
        public Rotations Rotations { get { return Group?.Rigidity?.Rotations; } }
        public RotationsPlasticLimits RotationsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitMoments; } }

        /// <summary>
        /// This property only reflects the edge normal. If this normal is changed arcs may transform.
        /// </summary>
        [XmlElement("normal", Order = 3)]
        public Geometry.FdVector3d EdgeNormal;
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineSupport()
        {
            
        }

        /// <summary>
        /// LineSupport along edge with rigidity (motions, rotations). Group LCS aligned with edge LCS.
        /// </summary>
        public LineSupport(Geometry.Edge edge, Motions motions, Rotations rotations, bool movingLocal, string identifier = "S")
        {
            var group = new Group(edge.CoordinateSystem, motions, rotations);
            Initialize(edge, group, movingLocal, identifier);
        }

        /// <summary>
        /// LineSupport along edge with rigidity (motions, rotations) and plastic limits (forces, moments). Group LCS aligned with edge LCS.
        /// </summary>
        public LineSupport(Geometry.Edge edge, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, bool movingLocal, string identifier = "S")
        {
            var group = new Group(edge.CoordinateSystem, motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(edge, group, movingLocal, identifier);
        }

        private void Initialize(Geometry.Edge edge, Group group, bool movingLocal, string identifier)
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
        public static LineSupport Rigid(Geometry.Edge edge, bool movingLocal, string identifier = "S")
        {
            Motions motions = Motions.RigidLine();
            Rotations rotations = Rotations.RigidLine();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

        /// <summary>
        /// Hinged LineSupport along edge.
        /// </summary>
        public static LineSupport Hinged(Geometry.Edge edge, bool movingLocal, string identifier = "S")
        {
            Motions motions = Motions.RigidLine();
            Rotations rotations = Rotations.Free();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

    }
}