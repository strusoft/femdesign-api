// https://strusoft.com/
using FemDesign.GenericClasses;
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class ExcitationForce
    {
        [XmlAttribute("nres")]
        public int _timeStep = 5;
        [XmlIgnore]
        public int TimeStep
        {
            get
            {
                return this._timeStep;
            }
            set
            {
                this._timeStep = value;
            }
        }

        [XmlAttribute("tcend")]
        public double _lastMomentOfThCalc = 20.0;
        [XmlIgnore]
        public double LastMomentOfThCalc
        {
            get
            {
                return this._lastMomentOfThCalc;
            }
            set
            {
                this._lastMomentOfThCalc = value;
            }
        }

        [XmlAttribute("method")]
        public int _method = 0;
        [XmlIgnore]
        public IntegrationSchemeMethod Method
        {
            get
            {
                return (IntegrationSchemeMethod)_method;
            }
            set
            {
                this._method = (int)value;
            }
        }

        [XmlAttribute("alpha")]
        public double _alpha = 0.0;
        [XmlIgnore]
        public double Alpha
        {
            get
            {
                return this._alpha;
            }
            set
            {
                this._alpha = value;
            }
        }

        [XmlAttribute("beta")]
        public double _beta = 0.0;
        [XmlIgnore]
        public double Beta
        {
            get
            {
                return this._beta;
            }
            set
            {
                this._beta = value;
            }
        }

        [XmlAttribute("ksi")]
        public double _dmpFactor = 5.0;
        [XmlIgnore]
        public double DmpFactor
        {
            get
            {
                return this._dmpFactor;
            }
            set
            {
                this._dmpFactor = value;
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public ExcitationForce()
        {

        }

        /// <summary>
        /// Define calculation parameters for excitation force calculation.
        /// </summary>
        /// <param name="step">The number of every nth time steps when results are saved during the calculation.</param>
        /// <param name="lastMoment">Last time moment of the time history calculation.</param>
        /// <param name="method">Integration scheme method type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dampingFactor">'ksi' damping factor.</param>
        public ExcitationForce(int step = 5, double lastMoment = 20.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0, double beta = 0, double dampingFactor = 5.0)
        {
            this.TimeStep = step;
            this.LastMomentOfThCalc = lastMoment;
            this.Method = method;
            this.Alpha = alpha;
            this.Beta = beta;
            this.DmpFactor = dampingFactor;
        }

        public static ExcitationForce Default()
        {
            return new ExcitationForce(5, 20.0, IntegrationSchemeMethod.Newmark, 0, 0, 5.0); 
        }
    }
}
