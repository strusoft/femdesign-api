// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// ANALYSIS
    /// </summary>
    public partial class Analysis
    {
        // elements
        [XmlElement("stage")]
        public Stage Stage { get; set; } // ANALSTAGE
        [XmlElement("comb")]
        public Comb Comb { get; set; } // ANALCOMB
        [XmlIgnore]
        public Stability Stability { get; set; } // STABILITY

        [XmlIgnore]
        public Imperfection Imperfection { get; set; } // IMPERFECTION

        [XmlElement("freq")]
        public Freq Freq { get; set; } // ANALFREQ
        [XmlElement("footfall")]
        public Footfall Footfall { get; set; }

        // attributes
        [XmlAttribute("calcCase")]
        public bool _calcCase; 
        [XmlIgnore]
        public bool CalcCase
        {
            get
            {
                return this._calcCase;
            }
            set
            {
                this._calcCase = value;
            }
        }
        [XmlAttribute("calcCstage")]
        public bool _calcCStage; 
        [XmlIgnore]
        public bool CalcCStage
        {
            get
            {
                return this._calcCStage;
            }
            set
            {
                this._calcCStage = value;
            }
        }
        [XmlAttribute("calcImpf")]
        public bool _calcImpf; 
        [XmlIgnore]
        public bool CalcImpf
        {
            get
            {
                return this._calcImpf;
            }
            set
            {
                this._calcImpf = value;
            }
        }
        [XmlAttribute("calcComb")]
        public bool _calcComb;
        [XmlIgnore]
        public bool CalcComb
        {
            get
            {
                return this._calcComb;
            }
            set
            {
                this._calcComb = value;
            }
        }
        [XmlAttribute("calcGmax")]
        public bool _calcGMax; 
        [XmlIgnore]
        public bool CalcGMax
        {
            get
            {
                return this._calcGMax;
            }
            set
            {
                this._calcGMax = value;
            }
        }
        [XmlAttribute("calcStab")]
        public bool _calcStab;
        [XmlIgnore]
        public bool CalcStab
        {
            get
            {
                return this._calcStab;
            }
            set
            {
                this._calcStab = value;
            }
        }
        [XmlAttribute("calcFreq")]
        public bool _calcFreq;
        [XmlIgnore]
        public bool CalcFreq
        {
            get
            {
                return this._calcFreq;
            }
            set
            {
                this._calcFreq = value;
            }
        }
        [XmlAttribute("calcSeis")]
        public bool _calcSeis;
        [XmlIgnore]
        public bool CalcSeis
        {
            get
            {
                return this._calcSeis;
            }
            set
            {
                this._calcSeis = value;
            }
        }
        [XmlAttribute("calcDesign")]
        public bool _calcDesign;
        [XmlIgnore]
        public bool CalcDesign
        {
            get
            {
                return this._calcDesign;
            }
            set
            {
                this._calcDesign = value;
            }
        }

        [XmlAttribute("calcFootfall")]
        public bool _calcFootfall;
        [XmlIgnore]
        public bool CalcFootfall
        {
            get
            {
                return this._calcFootfall;
            }
            set
            {
                this._calcFootfall = value;
            }
        }


        [XmlAttribute("elemfine")]
        public bool _elemFine;
        [XmlIgnore]
        public bool ElemFine
        {
            get
            {
                return this._elemFine;
            }
            set
            {
                this._elemFine = value;
            }
        }
        [XmlAttribute("diaphragm")]
        public bool _diaphragm;
        [XmlIgnore]
        public bool Diaphragm
        {
            get
            {
                return this._diaphragm;
            }
            set
            {
                this._diaphragm = value;
            }
        }
        [XmlAttribute("peaksmoothing")]
        public bool _peakSmoothing; 
        [XmlIgnore]
        public bool PeakSmoothing
        {
            get
            {
                return this._peakSmoothing;
            }
            set
            {
                this._peakSmoothing = value;
            }
        }
 
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Analysis()
        {
        }

        public Analysis(Calculate.Stage stage = null, Stability stability = null, Imperfection imperfection = null, Comb comb = null, Freq freq = null, Footfall footfall = null, bool calcCase = false, bool calcCStage = false, bool calcImpf = false, bool calcComb = false, bool calcGMax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcDesign = false, bool calcFootfall = false, bool elemFine = true, bool diaphragm = false, bool peakSmoothing = false)
        {
            this.Stage = stage;
            this.Comb = comb ?? Comb.Default();
            this.Stability = stability;
            this.Imperfection = imperfection;
            this.Freq = freq;
            this.Footfall = footfall;

            this.CalcCase = calcCase;
            this.CalcCStage = stage != null ? true : calcCStage;
            this.CalcImpf = imperfection != null ? true : calcImpf;
            this.CalcComb = calcComb;
            this.CalcGMax = calcGMax;
            this.CalcStab = stability != null ? true : calcStab;
            this.CalcFreq = freq != null ? true : calcFreq;
            this.CalcSeis = calcSeis;
            this.CalcDesign = calcDesign;
            this.CalcFootfall = footfall != null ? true : calcFootfall;
            this.ElemFine = elemFine;
            this.Diaphragm = diaphragm;
            this.PeakSmoothing = peakSmoothing;

            _validateAnalysisSettings();
        }

        private void _validateAnalysisSettings()
        {
            if (this.CalcStab == true & this.Stability == null)
                throw new Exception("calcStab == True. Stability must be defined!");

            if (this.CalcImpf == true & this.Imperfection == null)
                throw new Exception("calcImpf == True. Imperfection must be defined!");

            if (this.CalcCStage == true & this.Stage == null)
                throw new Exception("calcCStage == True. Stage must be defined!");

            if (this.CalcFootfall == true & this.Footfall == null)
                throw new Exception("calcFootfall == True. Footfall must be defined!");
        }

        /// <summary>
        /// Define a Static Analysis.
        /// </summary>
        /// <param name="comb"></param>
        /// <param name="calcCase"></param>
        /// <param name="calccomb"></param>
        /// <returns></returns>
        public static Analysis StaticAnalysis(Comb comb = null, bool calcCase = true, bool calccomb = true)
        {
            comb = comb ?? Comb.Default();
            return new Analysis(comb: comb, calcCase: calcCase, calcComb: calccomb);
        }


        /// <summary>
        /// Define an EigenFrequencies Analysis
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
        /// 
        /// </summary>
        /// <param name="ghost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Analysis ConstructionStages(bool ghost = false)
        {
            var stage = ghost ? Stage.GhostMethod() : Stage.TrackingMethod();
            return new Analysis(stage, calcCStage: true);
        }

        // TODO
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
            this.Comb.CombItem.Clear();
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            _setStabilityAnalysis(loadCombination.Values.ToList());
        }

        internal void _setStabilityAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            this.Comb.CombItem.Clear();

            // check if 

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
            this.Comb.CombItem.Clear();
            // ordered load combinations in the model
            var loadCombination = connection.GetLoadCombinations();

            _setImperfectionAnalysis(loadCombination.Values.ToList());
        }

        internal void _setImperfectionAnalysis(List<Loads.LoadCombination> loadCombination)
        {
            this.Comb.CombItem.Clear();

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