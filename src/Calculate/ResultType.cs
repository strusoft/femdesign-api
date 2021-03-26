using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Calculate
{
    public enum ResultType 
    {
        /* LOADCASES*/

        /// <summary>
        /// Bars, internal forces
        /// </summary>
        frCaseIntfBar_ListProc,

        /* LOADCOMBINATIONS*/

        /// <summary>
        /// Labelled sections, internal forces
        /// </summary>
        frCombIntfResSection_ListProc
    }
}
