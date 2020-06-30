// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// ANALYSIS
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Analysis
    {
        // elements
        [XmlElement("stage")]
        public Stage Stage { get; set; } // ANALSTAGE
        [XmlElement("comb")]
        public Comb Comb { get; set; } // ANALCOMB
        [XmlElement("freq")]
        public Freq Freq { get; set; } // ANALFREQ

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
        [XmlAttribute("calcCStage")]
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
        [XmlAttribute("calcCImpf")]
        public int _calcCImpf; // bool as int
        [XmlIgnore]
        public bool CalcCImpf
        {
            get
            {
                return Convert.ToBoolean(this._calcCImpf);
            }
            set
            {
                this._calcCImpf = Convert.ToInt32(value);
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
        public Analysis(Stage stage, Comb comb, Freq freq, bool calcCase, bool calcCStage, bool calcImpf, bool calcComb, bool calcGMax, bool calcStab, bool calcFreq, bool calcSeis, bool calcDesign, bool elemFine, bool diaphragm, bool peakSmoothing)
        {
            this.Stage = stage;
            this.Comb = comb;
            this.Freq = freq;
            this.CalcCase = calcCase;
            this.CalcCStage = calcCStage;
            this.CalcCImpf = calcImpf;
            this.CalcComb = calcComb;
            this.CalcGMax = calcGMax;
            this.CalcStab = calcStab;
            this.CalcFreq = calcFreq;
            this.CalcSeis = calcSeis;
            this.CalcDesign = calcDesign;
            this.ElemFine = elemFine;
            this.Diaphragm = diaphragm;
            this.PeakSmoothing = peakSmoothing;
        }

        /// <summary>
        /// Internal method to transfer load combination calculation parameters from LoadCombination to Analysis.
        /// </summary>
        /// <param name="model"></param>
        public void SetLoadCombinationCalculationParameters(FemDesign.Model model)
        {
            List<Loads.LoadCombination> loadCombinations = model.entities.loads.loadCombination;
            foreach(Loads.LoadCombination _loadCombination in loadCombinations)
            {
                this.Comb.AddLoadCombinationParameters(_loadCombination);
            }
        }

        #region dynamo
        /// <summary>Set parameters for analysis.</summary>
        /// <remarks>Create</remarks>
        /// <param name="stage">Definition for construction stage calculation method. Optional. If undefined default values will be used - for reference please see default values of Stage.Define node.</param>
        /// <param name="comb">Load combination calculation options. Optional. If undefined default values will be used - for reference please see default values of Comb.Define node.</param>
        /// <param name="freq">Eigienfrequency calculation options. Optional. If undefined default values will be used - for reference please see default values of Freq.Define node.</param>
        /// <param name="calcCase">Load cases.</param>
        /// <param name="calcCStage">Construction stages.</param>
        /// <param name="calcImpf">Imperfections.</param>
        /// <param name="calcComb">Load combinations.</param>
        /// <param name="calcGmax">Maximum of load groups.</param>
        /// <param name="calcStab">Stability analysis</param>
        /// <param name="calcFreq">Eigenfrequencies.</param>
        /// <param name="calcSeis">Seismic analysis.</param>
        /// <param name="calcDesign">Design calculations.</param>
        /// <param name="elemfine">Fine or standard elements.</param>
        /// <param name="diaphragm">Diaphragm calculation</param>
        /// <param name="peaksmoothing">Peak smoothing of internal forces</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Analysis Define([DefaultArgument("Stage.Default()")] Stage stage, [DefaultArgument("Comb.Default()")] Comb comb, [DefaultArgument("Freq.Default()")] Freq freq, bool calcCase = false, bool calcCStage = false, bool calcImpf = false, bool calcComb = false, bool calcGmax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcDesign = false, bool elemfine = false, bool diaphragm = false, bool peaksmoothing = false)
        {
            return new Analysis(stage, comb, freq, calcCase, calcCStage, calcImpf, calcComb, calcGmax, calcStab, calcFreq, calcSeis, calcDesign, elemfine, diaphragm, peaksmoothing);
        }
        #endregion
    }
}