// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// line_load_type
    /// </summary>
    public class LineLoad: ForceLoadBase
    {
        [XmlAttribute("load_dir")]
        public string _loadDirection; // load_dir_type
        [XmlIgnore]
        public string loadDirection
        {
            get {return this._loadDirection;}
            set {this._loadDirection = RestrictedString.LoadDirType(value);}
        }
        [XmlAttribute("load_projection")]
        public bool loadProjection { get; set; } // bool
        
        // elements
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge edge { get; set; } // edge_type
        [XmlElement("direction", Order = 2)]
        public Geometry.FdVector3d direction { get; set; } // point_type_3d
        [XmlElement("normal", Order = 3)]
        public Geometry.FdVector3d normal { get; set; } // point_type_3d
        [XmlElement("load", Order = 4)]
        public List<LoadLocationValue> load = new List<LoadLocationValue>(); // sequence: location_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineLoad()
        {
            
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal LineLoad(Geometry.Edge _edge, Geometry.FdVector3d f0, Geometry.FdVector3d f1, LoadCase loadCase, string comment, string loadDirection, bool loadProjection, string loadType)
        {
            this.EntityCreated();
            this.loadCase = loadCase.guid;
            this.comment = comment;
            this.loadDirection = loadDirection;
            this.loadProjection = loadProjection;
            this.loadType = loadType;
            this.edge = _edge;
            this.direction = f0.Normalize();
            this.normal = _edge.coordinateSystem.localZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.load.Add(new LoadLocationValue(_edge.points[0], f0.Length()));
            this.load.Add(new LoadLocationValue(_edge.points[_edge.points.Count - 1], f1.Length())); 
        }

        
        #region grasshopper
        /// <summary>
        /// Convert LineLoad edge to Rhino curve.
        /// </summary>
        internal Rhino.Geometry.Curve GetRhinoGeometry()
        {
            return this.edge.ToRhino();
        }

        #endregion
    }
}