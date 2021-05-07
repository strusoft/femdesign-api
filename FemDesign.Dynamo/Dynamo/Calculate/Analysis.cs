using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Calculate
{
    public partial class Analysis
    {
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
