// https://strusoft.com/
using FemDesign.GenericClasses;
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class GroundAcc
    {
        [XmlAttribute("flevelspectra")]
        public int _levelAcceleratonSpectra = 1;
        [XmlIgnore]
        public bool LevelAcceleratonSpectra
        {
            get
            {
                return Convert.ToBoolean(this._levelAcceleratonSpectra);
            }
            set
            {
                this._levelAcceleratonSpectra = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("dts")]
        public double _deltaT = 0.20;
        [XmlIgnore]
        public double DeltaT
        {
            get
            {
                return this._deltaT;
            }
            set
            {
                this._deltaT = value;
            }
        }

        [XmlAttribute("tsend")]
        public double _tEnd = 5.0;
        [XmlIgnore]
        public double TEnd
        {
            get
            {
                return this._tEnd;
            }
            set
            {
                this._tEnd = value;
            }
        }

        [XmlAttribute("q")]
        public double _q = 1.0;
        [XmlIgnore]
        public double q
        {
            get
            {
                return this._q;
            }
            set
            {
                this._q = value;
            }
        }

        [XmlAttribute("facc")]
        public int _timeHistory = 1;
        [XmlIgnore]
        public bool TimeHistory
        {
            get
            {
                return Convert.ToBoolean(this._timeHistory);
            }
            set
            {
                this._timeHistory = Convert.ToInt32(value);
            }
        }

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
        public GroundAcc()
        {

        }

        /// <summary>
        /// Define calculation parameters for ground acceleration calculation.
        /// </summary>
        /// <param name="levelAccSpectra">If true, the level acceleration response spectra calculation will be executed.</param>
        /// <param name="deltaT">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="tEnd">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="q">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="timeHistory">If true, the time history calculation will be executed.</param>
        /// <param name="step">The number of every nth time steps when results are saved during the calculation.</param>
        /// <param name="lastMoment">Last time moment of the time history calculation.</param>
        /// <param name="method">Integration scheme method type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dampingFactor">'ksi' damping factor.</param>
        public GroundAcc(bool levelAccSpectra = true, double deltaT = 0.2, double tEnd = 5.0, double q = 1.0, bool timeHistory = true, int step = 5, double lastMoment = 20.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0, double beta = 0, double dampingFactor = 5.0)
        {
            this.LevelAcceleratonSpectra = levelAccSpectra;
            this.DeltaT = deltaT;
            this.TEnd = tEnd;
            this.q = q;
            this.TimeHistory = timeHistory;
            this.TimeStep = step;
            this.LastMomentOfThCalc = lastMoment;
            this.Method = method;
            this.Alpha = alpha;
            this.Beta = beta;
            this.DmpFactor = dampingFactor;
        }

        /// <summary>
        /// Define calculation parameters for time history analysis (ground acceleration calculation).
        /// </summary>
        /// <param name="step">The number of every nth time steps when results are saved during the calculation.</param>
        /// <param name="lastMoment">Last time moment of the time history calculation.</param>
        /// <param name="method">Integration scheme method type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dampingFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static GroundAcc TimeHistoryCalc(int step = 5, double lastMoment = 20.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0, double beta = 0, double dampingFactor = 5.0)
        {
            return new GroundAcc(levelAccSpectra: false, timeHistory: true, step: step, lastMoment: lastMoment, method: method, alpha: alpha, beta: beta, dampingFactor: dampingFactor);
        }

        /// <summary>
        /// Define calculation parameters for level acceleration response spectra (ground acceleration calculation).
        /// </summary>
        /// <param name="deltaT">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="tEnd">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="q">Calculation parameter for Level acceleration spectra analysis.</param>
        /// <param name="method">Integration scheme method type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dampingFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static GroundAcc LevelAccResponseSpectraCalc(double deltaT = 0.2, double tEnd = 5.0, double q = 1.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0, double beta = 0, double dampingFactor = 5.0)
        {
            return new GroundAcc(levelAccSpectra: true, deltaT: deltaT, tEnd: tEnd, q: q, timeHistory: false, method: method, alpha: alpha, beta: beta, dampingFactor: dampingFactor);
        }

        /// <summary>
        /// Define default calculation parameters for ground acceleration.
        /// </summary>
        /// <returns></returns>
        public static GroundAcc Default()
        { 
            return new GroundAcc(true, 0.2, 5.0, 1.0, true, 5, 20.0, IntegrationSchemeMethod.Newmark, 0, 0, 5.0); 
        }
    }

    public enum IntegrationSchemeMethod
    {
        [Parseable("Newmark")]
        Newmark = 0,
        [Parseable("WilsonTheta")]
        WilsonTheta = 1
    }
}
