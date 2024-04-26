// https://strusoft.com/

using FemDesign.Geometry;
using StruSoft.Interop.StruXml.Data;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    [System.Serializable]
    public partial class LineSupportMotion : SupportMotionBase
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

        // elements
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; } // edge_type
        [XmlElement("direction", Order = 2)]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        [XmlElement("normal", Order = 3)]
        public Geometry.Vector3d Normal { get; set; } // point_type_3d
        [XmlElement("displacement", Order = 4)]
        public LoadLocationValue[] Displacement = new LoadLocationValue[2];
        [XmlIgnore]
        public double StartDisplacementValue
        {
            get
            {
                return this.Displacement[0].Value;
            }
            set
            {
                this.Displacement[0] = new LoadLocationValue(this.Edge.Points[0], value);
            }
        }
        [XmlIgnore]
        public double EndDisplacement
        {
            get
            {
                return this.Displacement[1].Value;
            }
            set
            {
                this.Displacement[1] = new LoadLocationValue(this.Edge.Points[this.Edge.Points.Count - 1], value);
            }
        }
        [XmlIgnore]
        public Geometry.Vector3d StartDisp
        {
            get
            {
                return this.Direction.Scale(this.StartDisplacementValue);
            }
        }
        [XmlIgnore]
        public Geometry.Vector3d EndDisp
        {
            get
            {
                return this.Direction.Scale(this.EndDisplacement);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineSupportMotion()
        {

        }

        public LineSupportMotion(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, SupportMotionType supportMotionType, string comment = "", bool constLoadDir = true)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.SupportMotionType = supportMotionType;
            this.Edge = edge;
            this.Normal = edge.Plane.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndDisplacements(constantForce, constantForce);
        }

        public LineSupportMotion(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, SupportMotionType supportMotionType, string comment = "", bool constLoadDir = true)
        {
            this.EntityCreated();
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.ConstantLoadDirection = constLoadDir;
            this.SupportMotionType = supportMotionType;
            this.Edge = edge;
            this.Normal = edge.Plane.LocalZ; // Note that LineLoad normal and Edge normal are not necessarily the same.
            this.SetStartAndEndDisplacements(startForce, endForce);
        }

        internal void SetStartAndEndDisplacements(Geometry.Vector3d startForce, Geometry.Vector3d endForce)
        {
            if (startForce.IsZero() && !endForce.IsZero())
            {
                this.Direction = endForce.Normalize();
                this.StartDisplacementValue = 0;
                this.EndDisplacement = endForce.Length();
            }

            else if (!startForce.IsZero() && endForce.IsZero())
            {
                this.Direction = startForce.Normalize();
                this.StartDisplacementValue = startForce.Length();
                this.EndDisplacement = 0;
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

                int par = v0.IsParallel(v1);
                if (par != 0)
                {
                    this.Direction = v0;
                    this.StartDisplacementValue = q0;
                    this.EndDisplacement = par * q1;
                }
                else
                {
                    throw new System.ArgumentException($"StartForce and EndForce must be parallel or antiparallel.");
                }
            }
        }


        /// <summary>
        /// Create a Distributed Motion Load to be applied to an Edge [m]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="startForce"></param>
        /// <param name="endForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineSupportMotion VariableMotion(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, startForce, endForce, loadCase, SupportMotionType.Motion, comment, constLoadDir);
        }


        /// <summary>
        /// Create a Distributed Rotation Load to be applied to an Edge [rad]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="startForce"></param>
        /// <param name="endForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineSupportMotion VariableRotation(Geometry.Edge edge, Geometry.Vector3d startForce, Geometry.Vector3d endForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, startForce, endForce, loadCase, SupportMotionType.Rotation, comment, constLoadDir);
        }

        /// <summary>
        /// Create a UniformDistributed Motion Load to be applied to an Edge [m]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="constantForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineSupportMotion UniformMotion(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, constantForce, loadCase, SupportMotionType.Motion, comment, constLoadDir);
        }

        /// <summary>
        /// Create a Uniform Distributed Rotation Load to be applied to an Edge [rad]
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="constantForce"></param>
        /// <param name="loadCase"></param>
        /// <param name="comment"></param>
        /// <param name="constLoadDir"></param>
        /// <param name="loadProjection"></param>
        /// <returns></returns>
        public static LineSupportMotion UniformRotation(Geometry.Edge edge, Geometry.Vector3d constantForce, LoadCase loadCase, string comment = "", bool constLoadDir = true)
        {
            return new LineSupportMotion(edge, constantForce, loadCase, SupportMotionType.Rotation, comment, constLoadDir);
        }

        public override string ToString()
        {
            var units = this.SupportMotionType == SupportMotionType.Motion ? "m" : "rad";
            var text = $"{this.GetType().Name} Start: {this.StartDisp} {units}, End: {this.EndDisp} {units}";
            if (LoadCase != null)
                return text + $", LoadCase: {this.LoadCase.Name}";
            else
                return text;
        }
    }
}