using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Calculate
{

    [IsVisibleInDynamoLibrary(false)]
    public partial class Comb
    {
        /// <summary>Define calculation parameters for the Load combinations calculation type. To setup which analysis types to consider for a specific load combination - use LoadCombination.SetupCalculation (found under the Calculate category).</summary>
        /// <remarks>Create</remarks>
        /// <param name="NLEmaxiter">Non-linear elastic analysis: Maximum iteration number.</param>
        /// <param name="PLdefloadstep">Plastic analysis: Default load step in % of the total load.</param>
        /// <param name="PLminloadstep">Plastic analysis: Minimal load step [%]</param>
        /// <param name="PLmaxeqiter">Plastic analysis: Maximum equilibrium iteration number.</param>
        /// <param name="NLSMohr">Non-linear soil: Consider Mohr-Coulomb criteria.</param>
        /// <param name="NLSinitloadstep">Non-linear soil: Initial load step [%]</param>
        /// <param name="NLSminloadstep">Non-linear soil: Minimal load step [%]</param>
        /// <param name="NLSactiveelemratio">Non-linear soil: Volume ratio of nonlinearly active elements in one step [%]</param>
        /// <param name="NLSplasticelemratio">Non-linear soil: Volume ratio of plastic elements in one step [%]</param>
        /// <param name="CRloadstep">Cracked section analysis: One load step in % of the total load.</param>
        /// <param name="CRmaxiter">Cracked section analysis: Maximum iteration number.</param>
        /// <param name="CRstifferror">Cracked section analysis: Allowed stiffness change error [%]</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Comb Define(int NLEmaxiter = 30, int PLdefloadstep = 20, int PLminloadstep = 2, int PLmaxeqiter = 30, bool NLSMohr = true, int NLSinitloadstep = 10, int NLSminloadstep = 10, int NLSactiveelemratio = 5, int NLSplasticelemratio = 5, int CRloadstep = 20, int CRmaxiter = 30, int CRstifferror = 2)
        {
            return new Comb(NLEmaxiter, PLdefloadstep, PLminloadstep, PLmaxeqiter, NLSMohr, NLSinitloadstep, NLSminloadstep, NLSactiveelemratio, NLSplasticelemratio, CRloadstep, CRmaxiter, CRstifferror);
        }
    }
}