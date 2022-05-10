using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#if ISDYNAMO
using Autodesk.DesignScript.Runtime;
#endif

namespace FemDesign.Calculate
{
    [System.Serializable]
    #if ISDYNAMO
	[IsVisibleInDynamoLibrary(false)]
    #endif
    public enum ListProc
    {
        #region FINITE ELEMENTS
        /* FINITE ELEMENTS */
        /// <summary>
        /// Fea Nodes
        /// </summary>
        [XmlEnum("femNode_ListProc")]
        FeaNode,
        #endregion

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
        /// Load case: Shells, Displacements
        /// </summary>
        [XmlEnum("frCaseDispShell_ListProc")]
        ShellDisplacementLoadCase,
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
        /// <summary>
        /// Load case: Shells, Derived Forces
        /// </summary>
        [XmlEnum("frCaseIntfDerShell_ListProc")]
        ShellDerivedForceLoadCase,
        /// <summary>
        /// Load case: Shells, Derived Forces (Extract)
        /// </summary>
        [XmlEnum("frCaseIntfDerShellExtract_ListProc")]
        ShellDerivedForceExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Top Side
        /// </summary>
        [XmlEnum("frCaseStrsTopShell_ListProc")]
        ShellStressesTopLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Top Side (Extract)
        /// </summary>
        [XmlEnum("frCaseStrsTopShellExtract_ListProc")]
        ShellStressesTopExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Mid Axis
        /// </summary>
        [XmlEnum("frCaseStrsMembShell_ListProc")]
        ShellStressesMembraneLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Mid Axis (Extract)
        /// </summary>
        [XmlEnum("frCaseStrsMembShellExtract_ListProc")]
        ShellStressesMembraneExtractLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Bottom Side
        /// </summary>
        [XmlEnum("frCaseStrsBotShell_ListProc")]
        ShellStressesBottomLoadCase,
        /// <summary>
        /// Load case: Shells, Stresses Bottom Side (Extract)
        /// </summary>
        [XmlEnum("frCaseStrsBotShellExtract_ListProc")]
        ShellStressesBottomExtractLoadCase,
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
        /// Load case: Shells, Derived Forces
        /// </summary>
        [XmlEnum("frCombIntfDerShell_ListProc")]
        ShellDerivedForceLoadCombination,
        /// <summary>
        /// Load case: Shells, Derived Forces (Extract)
        /// </summary>
        [XmlEnum("frCombIntfDerShellExtract_ListProc")]
        ShellDerivedForceExtractLoadCombination,
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
        /// Load combination: Shells, Displacements
        /// </summary>
        [XmlEnum("frCombDispShell_ListProc")]
        ShellDisplacementLoadCombination,
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

        /// <summary>
        /// Load case: Shells, Stresses Top Side
        /// </summary>
        [XmlEnum("frCombStrsTopShell_ListProc")]
        ShellStressesTopLoadCombination,
        /// <summary>
        /// Load case: Shells, Stresses Top Side (Extract)
        /// </summary>
        [XmlEnum("frCombStrsTopShellExtract_ListProc")]
        ShellStressesTopExtractLoadCombination,
        /// <summary>
        /// Load case: Shells, Stresses Mid Axis
        /// </summary>
        [XmlEnum("frCombStrsMembShell_ListProc")]
        ShellStressesMembraneLoadCombination,
        /// <summary>
        /// Load case: Shells, Stresses Mid Axis (Extract)
        /// </summary>
        [XmlEnum("frCombStrsMembShellExtract_ListProc")]
        ShellStressesMembraneExtractLoadCombination,
        /// <summary>
        /// Load case: Shells, Stresses Bottom Side
        /// </summary>
        [XmlEnum("frCombStrsBotShell_ListProc")]
        ShellStressesBottomLoadCombination,
        /// <summary>
        /// Load case: Shells, Stresses Bottom Side (Extract)
        /// </summary>
        [XmlEnum("frCombStrsBotShellExtract_ListProc")]
        ShellStressesBottomExtractLoadCombination,
        #endregion

        #region EIGEN FREQUENCIES
        /* EIGEN FREQUENCIES */
        /// <summary>
        /// Eigen Frequencies: Structural Model
        /// </summary>
        [XmlEnum("frFreqEigen_ListProc")]
        EigenFrequencies,

        #endregion

        #region FOOTFALL
        /* FOOTFALL */
        /// <summary>
        /// Eigen Frequencies: FootFall
        /// </summary>
        [XmlEnum("frFootEigen_ListProc")]
        EigenFrequenciesFootFall,

        /// <summary>
        /// Nodal Response Factor: FootFall
        /// </summary>
        [XmlEnum("frFootRespNodal_ListProc")]
        NodalResponseFactor,

        /// <summary>
        /// Nodal Acceleration: FootFall
        /// </summary>
        [XmlEnum("frFootAccNodal_ListProc")]
        NodalAcceleration,

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
