// https://strusoft.com/
using System;
using System.Xml.Serialization;


namespace FemDesign.Supports
{
    /// <summary>
    /// line_support_type
    /// </summary>
    [System.Serializable]
    public class LineSupport: EntityBase
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
        internal LineSupport(Geometry.Edge edge, Releases.Motions motions, Releases.Rotations rotations, bool movingLocal, string identifier)
        {
            PointSupport._instance++; // PointSupport and LineSupport share the same instance counter.
            this.EntityCreated();
            this.Name =  identifier + "." + PointSupport._instance.ToString();
            this.MovingLocal = movingLocal;

            // set edge specific properties
            this.Group = new Group(edge.CoordinateSystem, motions, rotations);
            this.Edge = edge;
            this.EdgeNormal = edge.Normal;
        }

        /// <summary>
        /// Rigid LineSupport along edge.
        /// </summary>
        internal static LineSupport Rigid(Geometry.Edge edge, bool movingLocal, string identifier)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.RigidLine();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

        /// <summary>
        /// Hinged LineSupport along edge.
        /// </summary>
        internal static LineSupport Hinged(Geometry.Edge edge, bool movingLocal, string identifier)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

    }
}