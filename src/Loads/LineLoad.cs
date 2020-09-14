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
        public string _constantLoadDirection; // load_dir_type
        [XmlIgnore]
        public bool ConstantLoadDirection
        {
            get
            {
                return RestrictedString.LoadDirTypeToBool(this._constantLoadDirection);
            }
            set
            {
                this._constantLoadDirection = RestrictedString.LoadDirTypeFromBool(value);
            }
        }
        [XmlAttribute("load_projection")]
        public bool LoadProjection { get; set; } // bool
        
        // elements
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; } // edge_type
        [XmlElement("direction", Order = 2)]
        public Geometry.FdVector3d Direction { get; set; } // point_type_3d
        [XmlElement("normal", Order = 3)]
        public Geometry.FdVector3d Normal { get; set; } // point_type_3d
        [XmlElement("load", Order = 4)]
        public LoadLocationValue[] Load = new LoadLocationValue[2];
        [XmlIgnore]
        public double StartLoad
        {
            get
            {
                return this.Load[0].Value;
            }
            set
            {
                this.Load[0] = new LoadLocationValue(this.Edge.Points[0], value);
            }
        }
        [XmlIgnore]
        public double EndLoad
        {
            get
            {
                return this.Load[1].Value;
            }
            set
            {
                this.Load[1] = new LoadLocationValue(this.Edge.Points[this.Edge.Points.Count - 1], value);
            }
        }
        [XmlIgnore]
        public Geometry.FdVector3d StartForce
        {
            get
            {
                return this.Direction.Scale(this.StartLoad);
            }
        }
        [XmlIgnore]
        public Geometry.FdVector3d EndForce
        {
            get
            {
                return this.Direction.Scale(this.EndLoad);
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
        internal LineLoad(Geometry.Edge edge, Geometry.FdVector3d f0, Geometry.FdVector3d f1, LoadCase loadCase, string comment, bool constLoadDir, bool loadProjection, string loadType)
        {
            this.EntityCreated();
            this.LoadCase = loadCase.Guid;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.LoadProjection = loadProjection;
            this.LoadType = loadType;
            this.Edge = edge;
            this.Normal = edge.CoordinateSystem.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndForces(f0, f1);
        }

        internal void SetStartAndEndForces(Geometry.FdVector3d startForce, Geometry.FdVector3d endForce)
        {
            if (startForce.IsZero() && !endForce.IsZero())
            {
                this.Direction = endForce.Normalize();
                this.StartLoad = 0;
                this.EndLoad = endForce.Length();
            }

            else if (!startForce.IsZero() && endForce.IsZero())
            {
                this.Direction = startForce.Normalize();
                this.StartLoad = startForce.Length();
                this.EndLoad = 0;
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
                    this.Direction = v0;
                    this.StartLoad = q0;
                    this.EndLoad = par * q1;
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
            return this.Edge.ToRhino();
        }

        #endregion
    }
}