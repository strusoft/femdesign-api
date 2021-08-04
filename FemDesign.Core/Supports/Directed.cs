// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Releases;

namespace FemDesign.Supports
{
    [System.Serializable]
    public class SimpleRegidityGroup
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
        private SimpleRegidityGroup()
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
        public Geometry.FdVector3d Direction;

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
                    regidityGroup = null; 
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
                    regidityGroup = null;
                }
            }
        }

        [XmlIgnore]
        private PlasticityType plasticLimitForces = new PlasticityType();
        [XmlElement("plastic_limit_forces", Order = 4)]
        public PlasticityType PlasticLimitForces
        {
            get => plasticLimitForces;
            set {
                plasticLimitForces = value;
                if (value != null)
                    regidityGroup = null;
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
                    regidityGroup = null; 
            } 
        }

        [XmlIgnore]
        private SimpleRegidityGroup regidityGroup;
        [XmlElement("rigidity_group", Order = 6)]
        public SimpleRegidityGroup RegidityGroup { 
            get => regidityGroup; 
            set { 
                regidityGroup = value;
                if (value != null)
                {
                    movement = null;
                    rotation = null;
                    plasticLimitForces = null;
                    plasticLimitMoments = null;
                }
            } 
        }
    }
}