// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Supports
{
    /// <summary>
    /// line_support_type
    /// </summary>
    public class LineSupport: EntityBase
    {

        // serialization properties
        [XmlAttribute("name")]
        public string name { get; set; } // identifier. Default = S
        [XmlAttribute("moving_local")]
        public bool movingLocal { get; set; } // bool
        [XmlElement("group", Order = 1)]
        public Group group { get; set; } // support_rigidity_data_type
        [XmlElement("edge", Order = 2)]
        public Geometry.Edge edge { get; set; } // edge_type
        [XmlElement("normal", Order = 3)]
        public Geometry.FdVector3d normal { get; set; } // point_type_3d
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineSupport()
        {
            
        }

        /// <summary>
        /// LineSupport along edge with rigidity (motions, rotations). Group LCS aligned with edge LCS.
        /// </summary>
        internal LineSupport(Geometry.Edge _edge, Releases.Motions motions, Releases.Rotations rotations, bool movingLocal)
        {
            PointSupport.instance++; // PointSupport and LineSupport share the same instance counter.
            this.EntityCreated();
            this.name = "S." + PointSupport.instance.ToString();
            this.movingLocal = movingLocal;
            this.group = new Group(_edge, motions, rotations);
            this.edge = _edge;
            this.normal = _edge.coordinateSystem.localZ;
        }

        /// <summary>
        /// Rigid LineSupport along edge.
        /// </summary>
        internal static LineSupport Rigid(Geometry.Edge edge, bool movingLocal = false)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.RigidLine();
            return new LineSupport(edge, motions, rotations, movingLocal);
        }

        /// <summary>
        /// Hinged LineSupport along edge.
        /// </summary>
        internal static LineSupport Hinged(Geometry.Edge edge, bool movingLocal = false)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new LineSupport(edge, motions, rotations, movingLocal);
        }

        #region grasshopper
        internal Rhino.Geometry.Curve GetRhinoGeometry()
        {
            return this.edge.ToRhino();
        }
        #endregion
    }
}