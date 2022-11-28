// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// line_load_type
    /// </summary>
    [System.Serializable]
    public partial class LineLoad : ForceLoadBase
    {
        [XmlAttribute("load_dir")]
        public LoadDirType _constantLoadDirection; // load_dir_type
        [XmlIgnore]
        public bool ConstantLoadDirection
        {
            get
            {
                return this._constantLoadDirection == LoadDirType.Constant;
            }
            set
            {
                this._constantLoadDirection = value ? LoadDirType.Constant : LoadDirType.Changing;
            }
        }
        [XmlAttribute("load_projection")]
        public bool LoadProjection { get; set; } // bool

        // elements
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; } // edge_type
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        [XmlElement("normal", Order = 3)]
        public Geometry.Vector3d Normal { get; set; } // point_type_3d
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
        public Geometry.Vector3d StartForce
        {
            get
            {
                return this.Direction.Scale(this.StartLoad);
            }
        }
        [XmlIgnore]
        public Geometry.Vector3d EndForce
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

        public LineLoad(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, ForceLoadType loadType, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            this.EntityCreated();
            this.LoadCaseGuid = loadCase.Guid;
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.LoadProjection = loadProjection;
            this.LoadType = loadType;
            this.Edge = edge;
            this.Normal = edge.CoordinateSystem.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndForces(constantForce, constantForce);
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        public LineLoad(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, ForceLoadType loadType, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            this.EntityCreated();
            this.LoadCaseGuid = loadCase.Guid;
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.LoadProjection = loadProjection;
            this.LoadType = loadType;
            this.Edge = edge;
            this.Normal = edge.CoordinateSystem.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndForces(startForce, endForce);
        }

        internal void SetStartAndEndForces(Geometry.Vector3d startForce, Geometry.Vector3d endForce)
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
                Geometry.Vector3d v0 = startForce.Normalize();
                Geometry.Vector3d v1 = endForce.Normalize();
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


        /// <summary>
        /// Create a Distributed Force Load to be applied to an Edge [kNm/m]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="startForce"></param>
        /// <param name="endForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineLoad VariableForce(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
		{
            return new LineLoad(edge, startForce, endForce, loadCase, ForceLoadType.Force, comment, constLoadDir, loadProjection);
        }


        /// <summary>
        /// Create a Distributed Moment Load to be applied to an Edge [kN/m]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="startForce"></param>
        /// <param name="endForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineLoad VariableMoment(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            return new LineLoad(edge, startForce, endForce, loadCase, ForceLoadType.Moment, comment, constLoadDir, loadProjection);
        }

        /// <summary>
        /// Create a UniformDistributed Force Load to be applied to an Edge [kNm/m]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="constantForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineLoad UniformForce(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            return new LineLoad(edge, constantForce, loadCase, ForceLoadType.Force, comment, constLoadDir, loadProjection);
        }

        /// <summary>
        /// Create a Uniform Distributed Moment Load to be applied to an Edge [kNm/m]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="constantForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineLoad UniformMoment(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true, bool loadProjection = false)
        {
            return new LineLoad(edge, constantForce, loadCase, ForceLoadType.Moment, comment, constLoadDir, loadProjection);
        }

        public override string ToString()
        {
            var units = this.LoadType == ForceLoadType.Force ? "kN" : "kNm";
            return $"{this.GetType().Name} Start: {this.StartForce} {units}, End: {this.EndForce} {units}, Projected: {this.LoadProjection}, LoadCase: {this.LoadCase.Name}";
        }
    }
}