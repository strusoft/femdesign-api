using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    [System.Serializable]
    public enum ListProc
    {
        /* QUANTITY ESTIMATION */
        /// <summary>
        /// Quantity estimation, Concrete
        /// </summary>
        [XmlEnum("ccQuantityConc_ListProc")]
        QuantityEstimationConcrete,
        /// <summary>
        /// Quantity estimation, Reinforcement
        /// </summary>
        [XmlEnum("ccQuantityReinf_ListProc")]
        QuantityEstimationReinforcement,
        /// <summary>
        /// Quantity estimation, Steel
        /// </summary>
        [XmlEnum("ccQuantitySteel_ListProc")]
        QuantityEstimationSteel,
        /// <summary>
        /// Quantity estimation, Timber
        /// </summary>
        [XmlEnum("ccQuantityTimber_ListProc")]
        QuantityEstimationTimber,
        /// <summary>
        /// Quantity estimation, Masonry
        /// </summary>
        [XmlEnum("ccQuantityMasonry_ListProc")]
        QuantityEstimationMasonry,
        /// <summary>
        /// Quantity estimation, General
        /// </summary>
        [XmlEnum("ccQuantityGeneral_ListProc")]
        QuantityEstimationGeneral,
        /// <summary>
        /// Quantity estimation, Profiled panel
        /// </summary>
        [XmlEnum("ccQuantityProfiledPanel_ListProc")]
        QuantityEstimationProfiledPanel,

        /* LOADCASES*/
        /// <summary>
        /// Load case: Nodal displacements
        /// </summary>
        [XmlEnum("frCaseDispNodal_ListProc")]
        NodalDisplacementsLoadCase,
        /// <summary>
        /// Load case: Bars, internal forces
        /// </summary>
        [XmlEnum("frCaseIntfBar_ListProc")]
        BarsInternalForcesLoadCase,
        /// <summary>
        /// Load case: Point support group, Reactions
        /// </summary>
        [XmlEnum("frCaseReacPtGroup_ListProc")]
        PointSupportReactionsLoadCase,
        /// <summary>
        /// Load case: Line support group, Reactions
        /// </summary>
        [XmlEnum("frCaseReacLnGroup_ListProc")]
        LineSupportReactionsLoadCase,

        /* LOADCOMBINATIONS*/

        /// <summary>
        /// Load combination: Nodal displacements
        /// </summary>
        [XmlEnum("frCombDispNodal_ListProc")]
        NodalDisplacementsLoadCombination,
        /// <summary>
        /// Load combination: Bars, End forces
        /// </summary>
        [XmlEnum("frCombIntfBarEnd_ListProc")]
        BarsEndForcesLoadCombination,
        /// <summary>
        /// Load combination: Labelled sections, internal forces
        /// </summary>
        [XmlEnum("frCombIntfResSection_ListProc")]
        LabelledSectionsInternalForcesLoadCombination,
        /// <summary>
        /// Load combination: Labelled sections, Resultants
        /// </summary>
        [XmlEnum("frCombResResSection")]
        LabelledSectionsResultantsLoadCombination,
        /// <summary>
        /// Load combination: Point support group, Reactions
        /// </summary>
        [XmlEnum("frCombReacPtGroup_ListProc")]
        PointSupportReactionsLoadCombination,
    }
}
