// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// line_load_type
    /// </summary>
    [System.Serializable]
    public class LineLoad: ForceLoadBase
    {
        [XmlAttribute("load_dir")]
        public string _loadDirection; // load_dir_type
        [XmlIgnore]
        public bool constLoadDir
        {
            get
            {
                return RestrictedString.LoadDirTypeToBool(this._loadDirection);
            }
            set
            {
                this._loadDirection = RestrictedString.LoadDirTypeFromBool(value);
            }
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
        public LoadLocationValue[] load = new LoadLocationValue[2];
        // public List<LoadLocationValue> load = new List<LoadLocationValue>(2); // sequence: location_value
        [XmlIgnore]
        public double startLoad
        {
            get
            {
                return this.load[0].val;
            }
            set
            {
                this.load[0] = new LoadLocationValue(this.edge.points[0], value);
            }
        }
        [XmlIgnore]
        public double endLoad
        {
            get
            {
                return this.load[1].val;
            }
            set
            {
                this.load[1] = new LoadLocationValue(this.edge.points[this.edge.points.Count - 1], value);
            }
        }
        [XmlIgnore]
        public Geometry.FdVector3d startForce
        {
            get
            {
                return this.direction.Scale(this.startLoad);
            }
        }
        [XmlIgnore]
        public Geometry.FdVector3d endForce
        {
            get
            {
                return this.direction.Scale(this.endLoad);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineLoad()
        {
            
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal LineLoad(Geometry.Edge _edge, Geometry.FdVector3d f0, Geometry.FdVector3d f1, LoadCase loadCase, string comment, bool constLoadDir, bool loadProjection, string loadType)
        {
            this.EntityCreated();
            this.loadCase = loadCase.guid;
            this.comment = comment;
            this.constLoadDir = constLoadDir;
            this.loadProjection = loadProjection;
            this.loadType = loadType;
            this.edge = _edge;
            this.normal = _edge.coordinateSystem.localZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndForces(f0, f1);
        }

        internal void SetStartAndEndForces(Geometry.FdVector3d startForce, Geometry.FdVector3d endForce)
        {
            if (startForce.IsZero() && !endForce.IsZero())
            {
                this.direction = endForce.Normalize();
                this.startLoad = 0;
                this.endLoad = endForce.Length();
            }

            else if (!startForce.IsZero() && endForce.IsZero())
            {
                this.direction = startForce.Normalize();
                this.startLoad = startForce.Length();
                this.endLoad = 0;
            }

            else if (startForce.IsZero() && endForce.IsZero())
            {
                throw new System.ArgumentException($"Both StartForce and EndForce are zero vectors. Can't set direction of LineLoad.");
            }

            // if no zero vectors - check if vectors are parallel
            else
            {
                Geometry.FdVector3d v0 = startForce.Normalize();
                Geometry.FdVector3d v1 = endForce.Normalize();
                double q0 = startForce.Length();
                double q1 = endForce.Length();

                int par = v0.Parallel(v1);
                if (par != 0)
                {
                    this.direction = v0;
                    this.startLoad = q0;
                    this.endLoad = par * q1;
                }
                else
                {
                    throw new System.ArgumentException($"StartForce and EndForce must be parallel or antiparallel.");
                }
            }
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