
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Calculate
{
    /// Dummy class to put node at correct heirarchy in Dynamo.
    [IsVisibleInDynamoLibrary(false)]
    public partial class LoadCombination
    {
        #region dynamo
        /// <summary>
        /// Setup which analyses to consider during calculation of a specific load combination.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="ImpfRqd">Required imperfection shapes</param>
        /// <param name="StabRqd">Required buckling shapes for stability analysis</param>
        /// <param name="NLE">Consider elastic non-linear behaviour of structural elements</param>
        /// <param name="PL">Consider plastic behaviour of structural elements</param>
        /// <param name="NLS">Consider non-linear behaviour of soil</param>
        /// <param name="Cr">Cracked section analysis. Note that Cr only executes properly in RCDesign with DesignCheck set to true.</param>
        /// <param name="f2nd">2nd order analysis</param>
        /// <param name="Im">Imperfection shape for 2nd order analysis</param>
        /// <param name="Waterlevel">Ground water level</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Loads.LoadCombination SetupCalculation(Loads.LoadCombination loadCombination, int ImpfRqd = 0, int StabRqd = 0, bool NLE = false, bool PL = false, bool NLS = false, bool Cr = false, bool f2nd = false, int Im = 0, int Waterlevel = 0)
        {
            loadCombination.CombItem = new Calculate.CombItem(ImpfRqd, StabRqd, NLE, PL, NLS, Cr, f2nd, Im, Waterlevel);
            return loadCombination;
        }
        #endregion
    }
}