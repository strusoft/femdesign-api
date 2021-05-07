using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// timber_factors_type
    /// </summary>
    [System.Serializable]
    public partial class TimberFactors
    {
        /// <summary>
        /// gamma_m_u
        /// </summary>
        [XmlAttribute("gamma_m_u")]
        public double _gammaMU;
        [XmlIgnore]
        public double GammaMU
        {
            get
            {
                return this._gammaMU;
            }
            set
            {
                this._gammaMU = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// gamma_m_as
        /// </summary>
        [XmlAttribute("gamma_m_as")]
        public double _gammaMAs;
        [XmlIgnore]
        public double GammaMAs
        {
            get
            {
                return this._gammaMAs;
            }
            set
            {
                this._gammaMAs = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_U
        /// </summary>
        [XmlAttribute("kdef_U")]
        public double _kdefU;
        [XmlIgnore]
        public double KdefU
        {
            get
            {
                return this._kdefU;
            }
            set
            {
                this._kdefU = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_Sq
        /// </summary>
        [XmlAttribute("kdef_Sq")]
        public double _kdefSq;
        [XmlIgnore]
        public double KdefSq
        {
            get
            {
                return this._kdefSq;
            }
            set
            {
                this._kdefSq = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_Sf
        /// </summary>
        [XmlAttribute("kdef_Sf")]
        public double _kdefSf;
        [XmlIgnore]
        public double KdefSf
        {
            get
            {
                return this._kdefSf;
            }
            set
            {
                this._kdefSf = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// kdef_Sc
        /// </summary>
        [XmlAttribute("kdef_Sc")]
        public double _kdefSc;
        [XmlIgnore]
        public double KdefSc
        {
            get
            {
                return this._kdefSc;
            }
            set
            {
                this._kdefSc = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        /// <summary>
        /// service_class
        /// </summary>
        [XmlAttribute("service_class")]
        public int _serviceClass;
        [XmlIgnore]
        public int ServiceClass
        {
            get
            {
                return this._serviceClass;
            }
            set
            {
                this._serviceClass = RestrictedInteger.TimberServiceClass(value);
            }
        }

        /// <summary>
        /// system_factor
        /// </summary>
        [XmlAttribute("system_factor")]
        public double _systemFactor;
        [XmlIgnore]
        public double SystemFactor
        {
            get
            {
                return this._systemFactor;
            }
            set
            {
                this._systemFactor = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
    }
}