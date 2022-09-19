// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Releases;

namespace FemDesign.Supports
{
    [System.Serializable]
    public partial class SimpleRigidityGroup
    {
        [XmlAttribute("type")]
        public MotionType MotionType;

        [XmlElement("springs")]
        public List<StiffBaseType> Stiffnesses;
        [XmlElement("plastic_limits")]
        public List<PlasticityType> PlasticLimits;
        [XmlElement("plastic_limits2")]
        public List<PlasticityType2> PlasticLimits2;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SimpleRigidityGroup()
        {

        }
    }

    /// <summary>
    /// support_rigidity_data_type --> group
    /// </summary>
    [System.Serializable]
    public partial class Directed
    {
        [XmlElement("direction", Order = 1)]
        public Vector3d Direction;

        [XmlIgnore]
        private StiffBaseType movement;
        [XmlElement("mov", Order = 2)]
        public StiffBaseType Movement { 
            get => movement; 
            set { 
                movement = value;
                if (value != null && (value.Pos != 0.0 || value.Neg != 0.0))
                {
                    rotation = new StiffBaseType() { Pos = 0, Neg = 0 };
                    rigidityGroup = null;
                    plasticLimitForces = new PlasticityType();
                    plasticLimitMoments = null;
                }
            } 
        }

        [XmlIgnore]
        private StiffBaseType rotation;
        [XmlElement("rot", Order = 3)]
        public StiffBaseType Rotation { 
            get => rotation; 
            set { 
                rotation = value;
                if (value != null && (value.Pos != 0.0 || value.Neg != 0.0))
                {
                    movement = new StiffBaseType() { Pos = 0, Neg = 0 };
                    rigidityGroup = null;
                    plasticLimitForces = null;
                    plasticLimitMoments = new PlasticityType();
                }
            }
        }

        [XmlIgnore]
        private PlasticityType plasticLimitForces;
        [XmlElement("plastic_limit_forces", Order = 4)]
        public PlasticityType PlasticLimitForces
        {
            get => plasticLimitForces;
            set {
                plasticLimitForces = value;
                if (value != null)
                    rigidityGroup = null;
            }
        }

        [XmlIgnore]
        private PlasticityType plasticLimitMoments;
        [XmlElement("plastic_limit_moments", Order = 5)]
        public PlasticityType PlasticLimitMoments { 
            get => plasticLimitMoments; 
            set { 
                plasticLimitMoments = value;
                if (value != null)
                    rigidityGroup = null; 
            } 
        }

        [XmlIgnore]
        private SimpleRigidityGroup rigidityGroup;
        [XmlElement("rigidity_group", Order = 6)]
        public SimpleRigidityGroup RigidityGroup { 
            get => rigidityGroup; 
            set { 
                rigidityGroup = value;
                if (value != null)
                {
                    movement = null;
                    rotation = null;
                    plasticLimitForces = null;
                    plasticLimitMoments = null;
                }
            } 
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Directed()
        {

        }

        internal Directed(Vector3d direction, MotionType type, double pos, double neg, double posPlastic = 0.0, double negPlastic = 0.0)
        {
            Direction = direction;
            if (type == MotionType.Motion)
            {
                Movement = new StiffBaseType() { Pos = pos, Neg = neg };
                PlasticLimitForces = new PlasticityType() { Pos = posPlastic, Neg = negPlastic };
            }
            else if (type == MotionType.Rotation)
            {
                Rotation = new StiffBaseType() { Pos = pos, Neg = neg };
                PlasticLimitMoments = new PlasticityType() { Pos = posPlastic, Neg = negPlastic };
            }
        }

        internal Directed(Vector3d direction, SimpleRigidityGroup rigidityGroup)
        {
            Direction = direction;
            RigidityGroup = rigidityGroup;
        }
    }
}