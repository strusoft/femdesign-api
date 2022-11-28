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
        [XmlElement("freq")]
        public Freq Freq { get; set; } // ANALFREQ
        [XmlElement("footfall")]
        public Footfall Footfall { get; set; }

        // attributes
        [XmlAttribute("calcCase")]
        public int _calcCase; // bool as int
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
        public int _calcCStage; // bool as int
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
        public int _calcImpf; // bool as int
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
        public int _calcComb; // bool as int
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
        public int _calcGMax; // bool as int
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
        public int _calcStab; // bool as int
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
        public int _calcFreq; // bool as int
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
        public int _calcSeis; // bool as int
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
        [XmlAttribute("calcDesign")]
        public int _calcDesign; // bool as int
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

        [XmlAttribute("calcFootfall")]
        public int _calcFootfall; // bool as int
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


        [XmlAttribute("elemfine")]
        public int _elemFine; // bool as int
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
        public int _diaphragm; // bool as int
        [XmlIgnore]
        public bool Diaphragm
        {
            get
            {
                return Convert.ToBoolean(this._diaphragm);
            }
            set
            {
                this._diaphragm = Convert.ToInt32(value);
            }
        }
        [XmlAttribute("peaksmoothing")]
        public int _peakSmoothing; // bool as int
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
        public Analysis(Calculate.Stage stage = null, Comb comb = null, Freq freq = null, Footfall footfall = null, bool calcCase = false, bool calcCStage = false, bool calcImpf = false, bool calcComb = false, bool calcGMax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcDesign = false, bool calcFootfall = false, bool elemFine = false, bool diaphragm = false, bool peakSmoothing = false)
        {
            this.Stage = stage;
            this.Comb = comb ?? Comb.Default();
            this.Freq = freq;
            this.Footfall = footfall;
            this.CalcCase = calcCase;
            this.CalcCStage = stage != null ? true : calcCStage;
            this.CalcImpf = calcImpf;
            this.CalcComb = calcComb;
            this.CalcGMax = calcGMax;
            this.CalcStab = calcStab;
            this.CalcFreq = freq != null ? true : calcFreq;
            this.CalcSeis = calcSeis;
            this.CalcDesign = calcDesign;
            this.CalcFootfall = footfall != null ? true : calcFootfall;
            this.ElemFine = elemFine;
            this.Diaphragm = diaphragm;
            this.PeakSmoothing = peakSmoothing;
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
            var stage = ghost ? Stage.Ghost() : Stage.Tracking();
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
    }
}