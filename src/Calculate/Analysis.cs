// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// ANALYSIS
    /// </summary>
    public class Analysis
    {
        // elements
        [XmlElement("stage")]
        public Stage stage { get; set; } // ANALSTAGE
        [XmlElement("comb")]
        public Comb comb { get; set; } // ANALCOMB
        [XmlElement("freq")]
        public Freq freq { get; set; } // ANALFREQ

        // attributes
        [XmlAttribute("calcCase")]
        public int calcCase { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcCStage")]
        public int calcCStage { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcCImpf")]
        public int calcCImpf { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcComb")]
        public int calcComb { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcGmax")]
        public int calcGmax { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcStab")]
        public int calcStab { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcFreq")]
        public int calcFreq { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcSeis")]
        public int calcSeis { get; set; } // bool // int (0/1)?
        [XmlAttribute("calcDesign")]
        public int calcDesign { get; set; } // bool // int (0/1)?
        [XmlAttribute("elemfine")]
        public int elemfine { get; set; } // bool // int (0/1)?
        [XmlAttribute("diaphragm")]
        public int diaphragm { get; set; } // bool // int (0/1)?
        [XmlAttribute("peaksmoothing")]
        public int peaksmoothing { get; set; } // bool // int (0/1)?
 
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Analysis()
        {
            
        }
        private Analysis(Stage stage, Comb comb, bool calcCase = false, bool calcCstage = false, bool calcImpf = false, bool calcComb = false, bool calcGmax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcDesign = false, bool elemfine = false, bool diaphragm = false, bool peaksmoothing = false)
        {
            this.stage = stage;
            this.comb = comb;
            this.calcCase = Convert.ToInt32(calcCase);
            this.calcCStage = Convert.ToInt32(calcCStage);
            this.calcCImpf = Convert.ToInt32(calcImpf);
            this.calcComb = Convert.ToInt32(calcComb);
            this.calcGmax = Convert.ToInt32(calcGmax);
            this.calcStab = Convert.ToInt32(calcStab);
            this.calcFreq = Convert.ToInt32(calcFreq);
            this.calcSeis = Convert.ToInt32(calcSeis);
            this.calcDesign = Convert.ToInt32(calcDesign);
            this.elemfine = Convert.ToInt32(elemfine);
            this.diaphragm = Convert.ToInt32(diaphragm);
            this.peaksmoothing = Convert.ToInt32(peaksmoothing);
        }
        public void SetLoadCombinationCalculationParameters(FemDesign.Model model)
        {
            List<Loads.LoadCombination> loadCombinations = model.entities.loads.loadCombination;
            foreach(Loads.LoadCombination _loadCombination in loadCombinations)
            {
                this.comb.AddLoadCombinationParameters(_loadCombination);
            }
        }
        /// <summary>Set parameters for analysis.</summary>
        /// <remarks>Create</remarks>
        /// <param name="calcCase">Load cases.</param>
        /// <param name="calcCstage">Construction stages.</param>
        /// <param name="calcImpf">Imperfections.</param>
        /// <param name="calcComb">Load combinations.</param>
        /// <param name="calcGmax">Maximum of load groups.</param>
        /// <param name="calcStab">Stability analysis</param>
        /// <param name="calcFreq">Eigenfrequencies.</param>
        /// <param name="calcSeis">Seismic analysis.</param>
        /// <param name="calcDesign">Design calculations.</param>
        /// <param name="elemfine">Fine or standard finite elements.</param>
        /// <param name="diaphragm">Diaphragm calculation</param>
        /// <param name="peaksmoothing">Peak smoothing of internal forces</param>
        public static Analysis Default(bool calcCase = false, bool calcCstage = false, bool calcImpf = false, bool calcComb = false, bool calcGmax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcDesign = false, bool elemfine = false, bool diaphragm = false, bool peaksmoothing = false)
        {
            return new Analysis(Stage.Default(), Comb.Default(), calcCase, calcCstage, calcImpf, calcComb, calcGmax, calcStab, calcFreq, calcSeis, calcDesign, elemfine, diaphragm, peaksmoothing);
        }
        /// <summary>Set parameters for analysis.</summary>
        /// <remarks>Create</remarks>
        /// <param name="stage">Definition for construction stage calculation method.</param>
        /// <param name="comb">Load combination calculation options.</param>
        /// <param name="calcCase">Load cases.</param>
        /// <param name="calcCstage">Construction stages.</param>
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
        public static Analysis Define(Stage stage, Comb comb, bool calcCase = false, bool calcCstage = false, bool calcImpf = false, bool calcComb = false, bool calcGmax = false, bool calcStab = false, bool calcFreq = false, bool calcSeis = false, bool calcDesign = false, bool elemfine = false, bool diaphragm = false, bool peaksmoothing = false)
        {
            return new Analysis(stage, comb, calcCase, calcCstage, calcImpf, calcComb, calcGmax, calcStab, calcFreq, calcSeis, calcDesign, elemfine, diaphragm, peaksmoothing);
        }
    }
}