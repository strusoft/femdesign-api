// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// </summary>
    public partial class Analysis
    {
        // elements
        [XmlElement("stage")]
        public Stage Stage { get; set; }

        [XmlElement("comb")]
        public Comb Comb { get; set; }

        [XmlIgnore]
        public Stability Stability { get; set; }

        [XmlIgnore]
        public Imperfection Imperfection { get; set; }

        [XmlElement("freq")]
        public Freq Freq { get; set; }

        [XmlElement("footfall")]
        public Footfall Footfall { get; set; }

        [XmlElement("thgroundacc")]
        public GroundAcc GroundAcc { get; set; }

        [XmlElement("thexforce")]
        public ExcitationForce ExForce { get; set; }

        [XmlElement("periodicexc")]
        public PeriodicExcitation PeriodicEx { get; set; }


        // attributes
        [XmlAttribute("calcCase")]
        public int _calcCase; 
        [XmlIgnore]
        public bool CalcCase
        {
            get
            {
                return Convert.ToBoolean(this._calcCase);
            }
            set
            {
                this._calcCase = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcCstage")]
        public int _calcCStage; 
        [XmlIgnore]
        public bool CalcCStage
        {
            get
            {
                return Convert.ToBoolean(this._calcCStage);
            }
            set
            {
                this._calcCStage = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcImpf")]
        public int _calcImpf; 
        [XmlIgnore]
        public bool CalcImpf
        {
            get
            {
                return Convert.ToBoolean(this._calcImpf);
            }
            set
            {
                this._calcImpf = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcComb")]
        public int _calcComb;
        [XmlIgnore]
        public bool CalcComb
        {
            get
            {
                return Convert.ToBoolean(this._calcComb);
            }
            set
            {
                this._calcComb = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcGmax")]
        public int _calcGMax; 
        [XmlIgnore]
        public bool CalcGMax
        {
            get
            {
                return Convert.ToBoolean(this._calcGMax);
            }
            set
            {
                this._calcGMax = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcStab")]
        public int _calcStab;
        [XmlIgnore]
        public bool CalcStab
        {
            get
            {
                return Convert.ToBoolean(this._calcStab);
            }
            set
            {
                this._calcStab = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcFreq")]
        public int _calcFreq;
        [XmlIgnore]
        public bool CalcFreq
        {
            get
            {
                return Convert.ToBoolean(this._calcFreq);
            }
            set
            {
                this._calcFreq = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcSeis")]
        public int _calcSeis;
        [XmlIgnore]
        public bool CalcSeis
        {
            get
            {
                return Convert.ToBoolean(this._calcSeis);
            }
            set
            {
                this._calcSeis = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcFootfall")]
        public int _calcFootfall;
        [XmlIgnore]
        public bool CalcFootfall
        {
            get
            {
                return Convert.ToBoolean(this._calcFootfall);
            }
            set
            {
                this._calcFootfall = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcThGroundAcc")]
        public int _calcGroundAcc;
        [XmlIgnore]
        public bool CalcGroundAcc
        {
            get
            {
                return Convert.ToBoolean(this._calcGroundAcc);
            }
            set
            {
                this._calcGroundAcc = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcThExforce")]
        public int _calcExcitationForce;
        [XmlIgnore]
        public bool CalcExcitationForce
        {
            get
            {
                return Convert.ToBoolean(this._calcExcitationForce);
            }
            set
            {
                this._calcExcitationForce = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcPeriodicExc")]
        public int _calcPeriodicExcitation;
        [XmlIgnore]
        public bool CalcPeriodicExcitation
        {
            get
            {
                return Convert.ToBoolean(this._calcPeriodicExcitation);
            }
            set
            {
                this._calcPeriodicExcitation = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("calcDesign")]
        public int _calcDesign;
        [XmlIgnore]
        public bool CalcDesign
        {
            get
            {
                return Convert.ToBoolean(this._calcDesign);
            }
            set
            {
                this._calcDesign = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("elemfine")]
        public int _elemFine;
        [XmlIgnore]
        public bool ElemFine
        {
            get
            {
                return Convert.ToBoolean(this._elemFine);
            }
            set
            {
                this._elemFine = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("diaphragm")]
        public int _diaphragm;
        [XmlIgnore]
        public int Diaphragm
        {
            get
            {
                return this._diaphragm;
            }
            set
            {
                if (value < 0 || value > 2)
                    throw new ArgumentException($"Diaphragm is set to {value}. Value must be '0'= None , '1'= Rigid membrane or '2'= Fully rigid.");

                this._diaphragm = value;
            }
        }

        [XmlAttribute("peaksmoothing")]
        public int _peakSmoothing; 
        [XmlIgnore]
        public bool PeakSmoothing
        {
            get
            {
                return Convert.ToBoolean(this._peakSmoothing);
            }
            set
            {
                this._peakSmoothing = Convert.ToInt32(value);
            }
        }
 
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Analysis()
        {
        }

        public Analysis(Calculate.Stage stage = null, Stability stability = null, Imperfection imperfection = null, Comb comb = null, Freq freq = null, Footfall footfall = null, GroundAcc groundAcc = null, ExcitationForce exForce = null, PeriodicExcitation periodicEx = null, bool calcCase = false, bool calcCStage = false, bool calcImpf = false, bool calcComb = false, bool calcGMax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcFootfall = false, bool calcGroundAcc = false, bool calcExForce = false, bool calcPeriodicEx = false, bool calcDesign = false, bool elemFine = true, int diaphragm = 0, bool peakSmoothing = false)
        {
            this.Stage = stage;
            this.Comb = comb;
            this.Stability = stability;
            this.Imperfection = imperfection;
            this.Freq = freq;
            this.Footfall = footfall;
            this.GroundAcc = groundAcc;
            this.ExForce = exForce;
            this.PeriodicEx = periodicEx;

            this.CalcCase = calcCase;
            this.CalcCStage = stage != null ? true : calcCStage;
            this.CalcImpf = imperfection != null ? true : calcImpf;
            this.CalcComb = calcComb;
            this.CalcGMax = calcGMax;
            this.CalcStab = stability != null ? true : calcStab;
            this.CalcFreq = freq != null ? true : calcFreq;
            this.CalcSeis = calcSeis;
            this.CalcFootfall = footfall != null ? true : calcFootfall;
            this.CalcGroundAcc = groundAcc != null ? true : calcGroundAcc;
            this.CalcExcitationForce = exForce != null ? true : calcExForce;
            this.CalcPeriodicExcitation = periodicEx != null ? true : calcPeriodicEx;
            this.CalcDesign = calcDesign;
            this.ElemFine = elemFine;
            this.Diaphragm = diaphragm;
            this.PeakSmoothing = peakSmoothing;

            _validateAnalysisSettings();
        }

        private void _validateAnalysisSettings()
        {
            if (this.CalcStab == true && this.Stability == null)
                throw new Exception("calcStab == True. Stability must be defined!");

            if (this.CalcImpf == true && this.Imperfection == null)
                throw new Exception("calcImpf == True. Imperfection must be defined!");
            
            if (this.Stability != null && this.Imperfection != null)
                throw new Exception("Stability and Imperfection can not be run simultaneously. Consider running only Imperfection!");

            if (this.CalcCStage == true && this.Stage == null)
                throw new Exception("calcCStage == True. Stage must be defined!");

            if (this.CalcFootfall == true && this.Footfall == null)
                throw new Exception("calcFootfall == True. Footfall must be defined!");

            if (this.CalcGroundAcc == true && this.GroundAcc == null)
                throw new Exception("calcGroundAcc == True. GroundAcc must be defined!");

            if (this.CalcExcitationForce == true && this.ExForce == null)
                throw new Exception("calcExForce == True. ExForce must be defined!");

            if (this.CalcPeriodicExcitation == true && this.PeriodicEx == null)
                throw new Exception("calcPeriodicEx == True. PeriodicEx must be defined!");
        }

        /// <summary>
        /// Define a static analysis.
        /// </summary>
        /// <param name="comb">Calculation parameters for load combinations.</param>
        /// <param name="calcCase">If true, the load case calculation will be executed.</param>
        /// <param name="calccomb">If true, the load combination calculation will be executed.</param>
        /// <returns></returns>
        public static Analysis StaticAnalysis(Comb comb = null, bool calcCase = true, bool calccomb = true)
        {
            comb = comb ?? Comb.Default();
            return new Analysis(comb: comb, calcCase: calcCase, calcComb: calccomb);
        }


        /// <summary>
        /// Define an eigenfrequencies analysis.
        /// </summary>
        /// <param name="numShapes">Number of shapes.</param>
        /// <param name="maxSturm">Max number of Sturm check steps (checking missing eigenvalues).</param>
        /// <param name="x">Consider masses in global x-direction.</param>
        /// <param name="y">Consider masses in global y-direction.</param>
        /// <param name="z">Consider masses in global z-direction.</param>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Eigenfrequency</param>
        /// <returns></returns>
        public static Analysis Eigenfrequencies(int numShapes = 3, int maxSturm = 0, bool x = true, bool y = true, bool z = true, double top = -0.01)
        {
            var freqSettings = new Freq(numShapes, maxSturm, x, y, z, top);
            return new Analysis(freq: freqSettings, calcFreq: true);
        }

        /// <summary>
        /// Define an eigenfrequencies analysis.
        /// </summary>
        /// <param name="numShapes">Number of shapes.</param>
        /// <param name="autoIteration"></param>
        /// <param name="shapeNormalisation">Mode shape normalisation type.</param>
        /// <param name="maxSturm">Max number of Sturm check steps (checking missing eigenvalues).</param>
        /// <param name="x">Consider masses in global x-direction.</param>
        /// <param name="y">Consider masses in global y-direction.</param>
        /// <param name="z">Consider masses in global z-direction.</param>
        /// <param name="top">Top of substructure. Masses on this level and below are not considered in Eigenfrequency</param>
        /// <returns></returns>
        public static Analysis Eigenfrequencies(int numShapes = 3, int autoIteration = 0, ShapeNormalisation shapeNormalisation = ShapeNormalisation.Unit, int maxSturm = 0, bool x = true, bool y = true, bool z = true, double top = -0.01)
        {
            var freqSettings = new Freq(numShapes, autoIteration, shapeNormalisation, x, y, z, maxSturm, top);
            return new Analysis(freq: freqSettings, calcFreq: true);
        }

        /// <summary>
        /// Define ground acceleration analysis.
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
        /// <param name="dmpFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static Analysis GroundAcceleration(bool levelAccSpectra = true, double deltaT = 0.2, double tEnd = 5.0, double q = 1.0, bool timeHistory = true, int step = 5, double lastMoment = 20.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0, double beta = 0, double dmpFactor = 5.0)
        {
            GroundAcc grAccSettings = new GroundAcc(levelAccSpectra, deltaT, tEnd, q, timeHistory, step, lastMoment, method, alpha, beta, dmpFactor);
            return new Analysis(groundAcc: grAccSettings, calcGroundAcc: true);
        }

        /// <summary>
        /// Define excitation force analysis.
        /// </summary>
        /// <param name="step">The number of every nth time steps when results are saved during the calculation.</param>
        /// <param name="lastMoment">Last time moment of the time history calculation.</param>
        /// <param name="method">Integration scheme method type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dmpFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static Analysis ExcitationForce(int step = 5, double lastMoment = 20.0, IntegrationSchemeMethod method = IntegrationSchemeMethod.Newmark, double alpha = 0, double beta = 0, double dmpFactor = 5.0)
        {
            ExcitationForce grAccSettings = new ExcitationForce(step, lastMoment, method, alpha, beta, dmpFactor);
            return new Analysis(exForce: grAccSettings, calcExForce: true);
        }

        /// <summary>
        /// Define periodic excitation analysis.
        /// </summary>
        /// <param name="deltaT">Calculation parameter.</param>
        /// <param name="timeEnd">Calculation parameter.</param>
        /// <param name="dmpType">Damping type.</param>
        /// <param name="alpha">'alpha' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="beta">'beta' coefficient in the Rayleigh damping matrix.</param>
        /// <param name="dmpFactor">'ksi' damping factor.</param>
        /// <returns></returns>
        public static Analysis PeriodicExcitation(double deltaT = 0.01, double timeEnd = 5.0, DampingType dmpType = DampingType.Rayleigh, double alpha = 0, double beta = 0, double dmpFactor = 5.0)
        {
            PeriodicExcitation perExSettings = new PeriodicExcitation(deltaT, timeEnd, dmpType, alpha, beta, dmpFactor);
            return new Analysis(periodicEx: perExSettings, calcPeriodicEx: true);
        }

        /// <summary>
        /// Define an construction stages analysis.
        /// </summary>
        /// <param name="ghost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Analysis ConstructionStages(bool ghost = false)
        {
            var stage = ghost ? Stage.GhostMethod() : Stage.TrackingMethod();
            return new Analysis(stage: stage, calcCStage: true);
        }

        // TODO (missing calculation parameters in .fdscript)
        private static Analysis FootFall(Footfall footfall)
        {
            var analisys = new Analysis(footfall: footfall, calcFootfall: true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Internal method to transfer load combination calculation parameters from LoadCombination to Analysis.
        /// </summary>
        /// <param name="model"></param>
        public void SetLoadCombinationCalculationParameters(FemDesign.Model model)
        {
            this.Comb.CombItem.Clear();
            foreach(var loadComb in model.Entities.Loads.LoadCombinations)
            {
                var combItem = loadComb.CombItem ?? Calculate.CombItem.Default();
                this.Comb.CombItem.Add(combItem);
            }
            //this.Comb.CombItem.AddRange(model.Entities.Loads.LoadCombinations.Select(x => x.CombItem));
        }


        public void SetCombAnalysis(FemDesignConnection connection)
        {
            //this.Comb.CombItem.Clear();
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            _setCombAnalysis(loadCombination.Values.ToList());
        }

        internal void _setCombAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            var combItems = this.Comb.CombItem.DeepClone();
            this.Comb.CombItem.Clear();

            foreach (var element in loadCombination)
            {

                bool isFound = false;
                int i = 0;
                foreach (var combItem in combItems)
                {
                    var combName = combItem.CombName;
                    if (combName == element.Name)
                    {
                        var indexOf = loadCombination.Select(x => x.Name).ToList().IndexOf(combName);
                        this.Comb.CombItem.Add(combItem);
                        isFound = true;
                        break;
                    }
                    i++;
                }
                if (isFound == true)
                    continue;
                else
                    this.Comb.CombItem.Add(CombItem.Default());
            }
        }


        public void SetStabilityAnalysis(FemDesignConnection connection)
        {
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            _setStabilityAnalysis(loadCombination.Values.ToList());
        }

        internal void _setStabilityAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            // check if comb is defined. if not, create a new one. if exist, clear it
            if (this.Comb == null)
                this.Comb = new Comb();
            else
                this.Comb.CombItem.Clear();



            foreach (var element in loadCombination)
            {
                bool isFound = false;
                int i = 0;
                foreach (var combName in this.Stability.CombNames)
                {
                    if (combName == element.Name)
                    {
                        var indexOf = loadCombination.Select(x => x.Name).ToList().IndexOf(combName);
                        this.Comb.CombItem.Add(CombItem.Stability(this.Stability.NumShapes[i]));
                        isFound = true;
                        break;
                    }
                    i++;
                }
                if (isFound == true)
                    continue;
                else
                    this.Comb.CombItem.Add(CombItem.Default());
            }
        }

        public void SetImperfectionAnalysis(FemDesignConnection connection)
        {
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            _setImperfectionAnalysis(loadCombination.Values.ToList());
        }

        internal void _setImperfectionAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            //this.Comb.CombItem.Clear();

            // check if 

            foreach (var element in loadCombination)
            {
                bool isFound = false;
                int i = 0;
                foreach (var combName in this.Imperfection.CombNames)
                {
                    if (combName == element.Name)
                    {
                        var indexOf = loadCombination.Select(x => x.Name).ToList().IndexOf(combName);
                        this.Comb.CombItem.Add(CombItem.Imperfection(this.Imperfection.NumShapes[i]));
                        isFound = true;
                        break;
                    }
                    i++;
                }
                if (isFound == true)
                    continue;
                else
                    this.Comb.CombItem.Add(CombItem.Default());
            }
        }
    }
}