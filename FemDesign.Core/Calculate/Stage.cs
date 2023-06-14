// https://strusoft.com/
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>    
    /// fdscript.xsd
    /// ANALSTAGE
    /// </summary>
    public partial class Stage
    {
        [XmlAttribute("ghost")]
        public int _ghost { get; set; }

        [XmlIgnore]
        public bool Ghost
        {
            get { return Convert.ToBoolean(_ghost); }
            set { _ghost = Convert.ToInt32(value); }
        }

        [XmlAttribute("tda")]
        public int _timeDependentAnalysis { get; set; }
        [XmlIgnore]
        public bool TimeDependentAnalysis
        {
            get { return Convert.ToBoolean(_timeDependentAnalysis); }
            set { _timeDependentAnalysis = Convert.ToInt32(value); }
        }

        [XmlAttribute("creepincrementlimit")]
        public double _creepIncrementLimit { get; set; } = 0.25;

        /// <summary>
        /// creep_strain_increment_limit [thousand percent]
        /// </summary>
        [XmlIgnore]
        public double? CreepIncrementLimit
        {
            get
            {
                return _creepIncrementLimit;
            }
            set
            {
                var creep = (double)value;
                _creepIncrementLimit = FemDesign.RestrictedDouble.ValueInHalfClosedInterval(creep, 0.0, 10.0);
            }
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Stage()
        {
            
        }

        public Stage(bool ghost = false, bool timeDependent = false, double? creepLimit = null)
        {
            this.Ghost = ghost;
            this.CreepIncrementLimit = creepLimit == null ? 2.5 : creepLimit;
            this.TimeDependentAnalysis = timeDependent;
        }

        /// <summary>
        /// Default Construction stages method (incremental).
        /// </summary>
        public static Stage Default()
        {
            return new Stage(false);
        }
        /// <summary>
        /// Construction stages method.
        /// </summary>
        /// <param name="ghost">Ghost construction method. True/false. If false incremental method is used.</param>
        public static Stage Define(bool ghost = false)
        {
            return new Stage(ghost);
        }

        /// <summary>
        /// Incremental construction stage method.
        /// </summary>
        /// <returns></returns>
        public static Stage GhostMethod()
        {
            return new Stage(true);
        }

        /// <summary>
        /// Ghost construction stage method.
        /// </summary>
        /// <returns></returns>
        public static Stage TrackingMethod()
        {
            return new Stage(false);
        }
    }
}