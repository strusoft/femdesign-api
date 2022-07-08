using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.GenericClasses;
using System.Xml.Serialization;
using FemDesign.Calculate;


#if ISDYNAMO
using Autodesk.DesignScript.Runtime;
#endif

namespace FemDesign.Results
{

    public enum ResultType
    {
        #region FINITE ELEMENTS
        /* FINITE ELEMENTS */
        /// <summary>
        /// Fea Nodes
        /// </summary>

        [Parseable("FeaNode")]
        [Result(typeof(FeaNode), ListProc.FeaNode)]
        FeaNode,
        /// <summary>
        /// Fea Nodes
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("FeaBar")]
        [Result(typeof(FeaBar), ListProc.FeaBar)]
        FeaBar,
        /// <summary>
        /// Fea Nodes
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("FeaShell")]
        [Result(typeof(FeaShell), ListProc.FeaShell)]
        FeaShell,
        #endregion

        #region QUANTITY ESTIMATION
        /* QUANTITY ESTIMATION */

        /// <summary>
        /// Quantity estimation, Concrete
        /// </summary>
        [Parseable("QuantityEstimationGeneral")]
        [Result(typeof(QuantityEstimationGeneral), ListProc.QuantityEstimationGeneral)]
        QuantityEstimationGeneral,

        /// <summary>
        /// Quantity estimation, Concrete
        /// </summary>
        [Parseable("QuantityEstimationConcrete")]
        [Result(typeof(QuantityEstimationConcrete), ListProc.QuantityEstimationConcrete)]
        QuantityEstimationConcrete,

        /// <summary>
        /// Quantity estimation, Steel
        /// </summary>
        [Parseable("QuantityEstimationSteel")]
        [Result(typeof(QuantityEstimationSteel), ListProc.QuantityEstimationSteel)]
        QuantityEstimationSteel,

        /// <summary>
        /// Quantity estimation, Timber
        /// </summary>
        [Parseable("QuantityEstimationTimber")]
        [Result(typeof(QuantityEstimationTimber), ListProc.QuantityEstimationTimber)]
        QuantityEstimationTimber,

        /// <summary>
        /// Quantity estimation, Timber panel
        /// </summary>
        [Parseable("QuantityEstimationTimberPanel")]
        [Result(typeof(QuantityEstimationTimberPanel), ListProc.QuantityEstimationTimberPanel)]
        QuantityEstimationTimberPanel,

        /// <summary>
        /// Quantity estimation, Reinforcement
        /// </summary>
        [Parseable("QuantityEstimationReinforcement")]
        [Result(typeof(QuantityEstimationReinforcement), ListProc.QuantityEstimationReinforcement)]
        QuantityEstimationReinforcement,

        /// <summary>
        /// Quantity estimation, Profiled panel
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("QuantityEstimationProfiledPanel")]
        [Result(typeof(QuantityEstimationProfiledPlate), ListProc.QuantityEstimationProfiledPanel)]
        QuantityEstimationProfiledPanel,

        /// <summary>
        /// Quantity estimation, Masonry
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("QuantityEstimationMasonry")]
        [Result(typeof(QuantityEstimationMasonry), ListProc.QuantityEstimationMasonry)]
        QuantityEstimationMasonry,
        #endregion

        #region LOAD CASES AND COMBINATIONS
        /* LOAD CASES AND COMBINATIONS */
        /// <summary>
        /// Node, Displacements
        /// </summary>
        [Parseable("NodalDisplacement")]
        [Result(typeof(NodalDisplacement), ListProc.NodalDisplacementsLoadCase, ListProc.NodalDisplacementsLoadCombination)]
        NodalDisplacement,

        /// <summary>
        /// Bars, Internal Forces
        /// </summary>
        [Parseable("BarInternalForce")]
        [Result(typeof(BarInternalForce), ListProc.BarsInternalForcesLoadCase, ListProc.BarsInternalForcesLoadCombination)]
        BarInternalForce,

        /// <summary>
        /// Bars, Internal Forces
        /// </summary>
        [Parseable("BarStress")]
        [Result(typeof(BarStress), ListProc.BarsStressesLoadCase, ListProc.BarsStressesLoadCombination)]
        BarStress,

        /// <summary>
        /// Bars, Displacements
        /// </summary>
        [Parseable("BarDisplacement")]
        [Result(typeof(BarDisplacement), ListProc.BarsDisplacementsLoadCase, ListProc.BarsDisplacementsLoadCombination)]
        BarDisplacement,

        /// <summary>
        /// Labelled Sections, Internal Forces
        /// </summary>
        [Parseable("LabelledSectionInternalForce")]
        [Result(typeof(LabelledSectionInternalForce), ListProc.LabelledSectionsInternalForcesLoadCase, ListProc.LabelledSectionsInternalForcesLoadCombination)]
        LabelledSectionInternalForce,

        /// <summary>
        /// Labelled Sections, Resultants
        /// </summary>
        [Parseable("LabelledSectionResultant")]
        [Result(typeof(LabelledSectionResultant), ListProc.LabelledSectionsResultantsLoadCase, ListProc.LabelledSectionsResultantsLoadCombination)]
        LabelledSectionResultant,

        /// <summary>
        /// Point support group, Reactions
        /// </summary>
        [Parseable("PointSupportReaction")]
        [Result(typeof(PointSupportReaction), ListProc.PointSupportReactionsLoadCase, ListProc.PointSupportReactionsLoadCombination)]
        PointSupportReaction,

        /// <summary>
        /// Line connection forces
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("LineConnectionForce")]
        [Result(typeof(LineConnectionForce), ListProc.LineConnectionForceLoadCase, ListProc.LineConnectionForceLoadCombination)]
        LineConnectionForce,

        /// <summary>
        /// Line connection forces
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("LineConnectionResultant")]
        [Result(typeof(LineConnectionResultant), ListProc.LineConnectionResultantLoadCase, ListProc.LineConnectionResultantLoadCombination)]
        LineConnectionResultant,

        /// <summary>
        /// Line support group, Reactions
        /// </summary>
        [Parseable("LineSupportReaction")]
        [Result(typeof(LineSupportReaction), ListProc.LineSupportReactionsLoadCase, ListProc.LineSupportReactionsLoadCombination)]
        LineSupportReaction,

        /// <summary>
        /// Line support group, Resultants
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("LineSupportResultant")]
        [Result(typeof(LineSupportResultant), ListProc.LineSupportResultantsLoadCase, ListProc.LineSupportResultantsLoadCombination)]
        LineSupportResultant,

        /// <summary>
        /// Shells, Internal Force
        /// </summary>
        [Parseable("ShellInternalForce")]
        [Result(typeof(ShellInternalForce), ListProc.ShellInternalForceLoadCase, ListProc.ShellInternalForceLoadCombination)]
        ShellInternalForce,

        /// <summary>
        /// Shells, Internal Force (Extract)
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("ShellInternalForceExtract")]
        [Result(typeof(ShellInternalForce), ListProc.ShellInternalForceExtractLoadCase, ListProc.ShellInternalForceExtractLoadCombination)]
        ShellInternalForceExtract,

        /// <summary>
        /// Shells, Derived Forces
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("ShellDerivedForce")]
        [Result(typeof(ShellDerivedForce), ListProc.ShellDerivedForceLoadCase, ListProc.ShellDerivedForceLoadCombination)]
        ShellDerivedForce,

        /// <summary>
        /// Shells, Derived Forces
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("ShellDerivedForceExtract")]
        [Result(typeof(ShellDerivedForce), ListProc.ShellDerivedForceExtractLoadCase, ListProc.ShellDerivedForceExtractLoadCombination)]
        ShellDerivedForceExtract,

        /// <summary>
        /// Shells, Stress
        /// </summary>
        [Parseable("ShellStress")]
        [Result(typeof(ShellStress), ListProc.ShellStressesTopLoadCase, ListProc.ShellStressesTopLoadCombination, ListProc.ShellStressesMembraneLoadCase, ListProc.ShellStressesMembraneLoadCombination, ListProc.ShellStressesBottomLoadCase, ListProc.ShellStressesBottomLoadCombination)]
        ShellStress,

        /// <summary>
        /// Shells, Stress (Extract)
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("ShellStressExtract")]
        [Result(typeof(ShellStress), ListProc.ShellStressesTopExtractLoadCase, ListProc.ShellStressesTopExtractLoadCombination, ListProc.ShellStressesMembraneExtractLoadCase, ListProc.ShellStressesMembraneExtractLoadCombination, ListProc.ShellStressesBottomExtractLoadCase, ListProc.ShellStressesBottomExtractLoadCombination)]
        ShellStressExtract,

        /// <summary>
        /// Shells, Displacements
        /// </summary>
        [Parseable("ShellDisplacement")]
        [Result(typeof(ShellDisplacement), ListProc.ShellDisplacementLoadCase, ListProc.ShellDisplacementLoadCombination)]
        ShellDisplacement,

        /// <summary>
        /// Shells, Displacements (Extract)
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("ShellDisplacementExtract")]
        [Result(typeof(ShellDisplacement), ListProc.ShellDisplacementExtractLoadCase, ListProc.ShellDisplacementExtractLoadCombination)]
        ShellDisplacementExtract,
        #endregion

        #region EIGEN FREQUENCIES
        /// <summary>
        /// Eigen Frequencies: Structural Model
        /// </summary>
        [Parseable("EigenFrequencies")]
        [Result(typeof(EigenFrequencies), ListProc.EigenFrequencies)]
        EigenFrequencies,

        /// <summary>
        /// Eigen Frequencies: Structural Model
        /// </summary>
        [Parseable("NodalVibrationShape")]
        [Result(typeof(NodalVibration), ListProc.NodalVibrationShape)]
        NodalVibrationShape,
        #endregion

        #region FOOTFALL
        /// <summary>
        /// Nodal Response Factor: FootFall
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("NodalResponseFactor")]
        [Result(typeof(NodalResponseFactor), ListProc.NodalResponseFactor)]
        NodalResponseFactor,

        /// <summary>
        /// Nodal Acceleration: FootFall
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("NodalAcceleration")]
        [Result(typeof(NodalAcceleration), ListProc.NodalAcceleration)]
        NodalAcceleration,

        #endregion

        #region RC design
        /* RC design */
        /// <summary>
        /// Shell, Utilization
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("RCDesignShellUtilization")]
        [Result(typeof(ShellDisplacement), ListProc.RCDesignShellUtilizationLoadCombination)]
        RCDesignShellUtilization,

        /// <summary>
        /// Shell, Crack width
        /// </summary>
#if ISDYNAMO
        [IsVisibleInDynamoLibrary(false)]
#endif
        [Parseable("RCDesignShellCrackWidth")]
        [Result(typeof(ShellDisplacement), ListProc.RCDesignShellCrackWidthLoadCombination)]
        RCDesignShellCrackWidth,
        #endregion
    }
}
