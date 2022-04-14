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
        #region QUANTITY ESTIMATION
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
        /// Quantity estimation, Timber panel
        /// </summary>
        [XmlEnum("ccQuantityTmPanel_ListProc")]
        QuantityEstimationTimberPanel,
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
        #endregion

        #region LOADCASES
        /* LOAD CASES */
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
        /// Load case: Bars, stresses
        /// </summary>
        [XmlEnum("frCaseStrsBar_ListProc")]
        BarsStressesLoadCase,
        /// <summary>
        /// Load case: Bars, End Displacement
        /// </summary>
        [XmlEnum("frCaseDispBar_ListProc")]
        BarsDisplacementsLoadCase,
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
        /// <summary>
        /// Load case: Line support group, Resultants
        /// </summary>
        [XmlEnum("frCaseReacLnGroupRes_ListProc")]
        LineSupportResultantsLoadCase,
        /// <summary>
        /// Load case: Shells, Displacements (Extract)
        /// </summary>
        [XmlEnum("frCaseDispShellExtract_ListProc")]
        ShellDisplacementExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Internal Forces
        /// </summary>
        [XmlEnum("frCaseIntfShell_ListProc")]
        ShellInternalForceLoadCase,
        /// <summary>
        /// Load case: Shells, Internal Forces (Extract)
        /// </summary>
        [XmlEnum("frCaseIntfShellExtract_ListProc")]
        ShellInternalForceExtractLoadCase,
        #endregion

        #region LOAD COMBINATIONS
        /* LOAD COMBINATIONS */
        /// <summary>
        /// Load combination: Nodal displacements
        /// </summary>
        [XmlEnum("frCombDispNodal_ListProc")]
        NodalDisplacementsLoadCombination,
        /// <summary>
        /// Load combination: Bars, internal forces
        /// </summary>
        [XmlEnum("frCombIntfBar_ListProc")]
        BarsInternalForcesLoadCombination,
        /// <summary>
        /// Load combination: Bars, Stresses
        /// </summary>
        [XmlEnum("frCombStrsBar_ListProc")]
        BarsStressesLoadCombination,
        /// <summary>
        /// Load combination: Bars, End Displacement
        /// </summary>
        [XmlEnum("frCombDispBar_ListProc")]
        BarsDisplacementsLoadCombination,
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
        /// <summary>
        /// Load combination: Line support group, Reactions
        /// </summary>
        [XmlEnum("frCombReacLnGroup_ListProc")]
        LineSupportReactionsLoadCombination,
        /// <summary>
        /// Load combination: Line support group, Resultants
        /// </summary>
        [XmlEnum("frCombReacLnGroupRes_ListProc")]
        LineSupportResultantsLoadCombination,
        /// <summary>
        /// Load combination: Shells, Displacements (Extract)
        /// </summary>
        [XmlEnum("frCombDispShellExtract_ListProc")]
        ShellDisplacementExtractLoadCombination,
        /// <summary>
        /// Load combination: Shells, Internal Forces
        /// </summary>
        [XmlEnum("frCombIntfShell_ListProc")]
        ShellInternalForceLoadCombination,
        /// <summary>
        /// Load combination: Shells, Internal Forces (Extract)
        /// </summary>
        [XmlEnum("frCombIntfShellExtract_ListProc")]
        ShellInternalForceExtractLoadCombination,
        #endregion

        #region EIGEN FREQUENCIES
        /* EIGEN FREQUENCIES */
        /// <summary>
        /// Eigen Frequencies: Structural Model
        /// </summary>
        [XmlEnum("frFreqEigen_ListProc")]
        EigenFrequencies,

        #endregion

        #region RC design
        /* RC design */
        /// <summary>
        /// RC design: Shell, Crack width
        /// </summary>
        [XmlEnum("RCShellCrackWidthCmax_ListProc")]
        RCDesignShellCrackWidthMaxComb,
        /// <summary>
        /// RC design: Shell, Utilization
        /// </summary>
        [XmlEnum("RCShellUtilizationCmax_ListProc")]
        RCDesignShellUtilizationMaxComb,

        /// <summary>
        /// RC design: Shell, Crack width
        /// </summary>
        [XmlEnum("RCShellCrackWidthComb_ListProc")]
        RCDesignShellCrackWidthLoadCombination,
        /// <summary>
        /// RC design: Shell, Utilization.
        /// </summary>
        [XmlEnum("RCShellUtilizationComb_ListProc")]
        RCDesignShellUtilizationLoadCombination,
        #endregion
    }
}
