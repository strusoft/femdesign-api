// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    /// <summary>
    /// line_load_type
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
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

        #region dynamo
        /// <summary>
        /// Creates a uniform force line load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve defining the line load.</param>
        /// <param name="force">Force.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LineLoad ForceUniform(Autodesk.DesignScript.Geometry.Curve curve, Autodesk.DesignScript.Geometry.Vector force, LoadCase loadCase, string comment = "")
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            Geometry.FdVector3d _force = Geometry.FdVector3d.FromDynamo(force);
            return new LineLoad(edge, _force, _force, loadCase, comment, "constant", false, "force");
        }

        /// <summary>
        /// Creates a uniform moment line load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve defining the line load.</param>
        /// <param name="moment">Moment.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LineLoad MomentUniform(Autodesk.DesignScript.Geometry.Curve curve, Autodesk.DesignScript.Geometry.Vector moment, LoadCase loadCase, string comment = "")
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            Geometry.FdVector3d _moment = Geometry.FdVector3d.FromDynamo(moment);
            return new LineLoad(edge, _moment, _moment, loadCase, comment, "constant", false, "moment");
        }

        /// <summary>
        /// Convert LineLoad edge to Dynamo curve.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Curve GetDynamoGeometry()
        {
            return this.edge.ToDynamo();
        }

        #endregion
        
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