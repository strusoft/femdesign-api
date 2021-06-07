using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    [System.Serializable]
    public enum ResultType 
    {
        /* LOADCASES*/
        /// <summary>
        /// Nodal displacements
        /// </summary>
        frCaseDispNodal_ListProc,
        /// <summary>
        /// Bars, internal forces
        /// </summary>
        frCaseIntfBar_ListProc,
        /// <summary>
        /// Point support group, Reactions
        /// </summary>
        frCaseReacPtGroup_ListProc,
        /// <summary>
        /// Line support group, Reactions
        /// </summary>
        frCaseReacLnGroup_ListProc,

        /* LOADCOMBINATIONS*/

        /// <summary>
        /// Nodal displacements
        /// </summary>
        frCombDispNodal_ListProc,
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
        /// <summary>
        /// Point support group, Reactions
        /// </summary>
        frCombReacPtGroup_ListProc,
    }
}
