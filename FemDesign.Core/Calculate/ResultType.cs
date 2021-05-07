using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FemDesign.Calculate
{
    [System.Serializable]
    public enum ResultType 
    {
        /* LOADCASES*/

        /// <summary>
        /// Bars, internal forces
        /// </summary>
        frCaseIntfBar_ListProc,

        /* LOADCOMBINATIONS*/

        /// <summary>
        /// Bars, End forces
        /// </summary>
        frCombIntfBarEnd_ListProc,
        /// <summary>
        /// Labelled sections, internal forces
        /// </summary>
        frCombIntfResSection_ListProc,
        /// <summary>
        /// Labelled sections, Resultants
        /// </summary>
        frCombResResSection,
    }
}
