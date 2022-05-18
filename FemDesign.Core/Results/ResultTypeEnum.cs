using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.GenericClasses;
using System.Xml.Serialization;
using FemDesign.Calculate;


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
        [Parseable("FeaBar")]
        [Result(typeof(FeaBar), ListProc.FeaBar)]
        FeaBar,
        /// <summary>
        /// Fea Nodes
        /// </summary>
        [Parseable("FeaShell")]
        [Result(typeof(FeaShell), ListProc.FeaShell)]
        FeaShell,
        #endregion

        #region QUANTITY ESTIMATION
        /* QUANTITY ESTIMATION */
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
        [Parseable("QuantityEstimationProfiledPanel")]
        [Result(typeof(QuantityEstimationProfiledPlate), ListProc.QuantityEstimationProfiledPanel)]
        QuantityEstimationProfiledPanel,

        /// <summary>
        /// Quantity estimation, Masonry
        /// </summary>
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
        /// Point support group, Reactions
        /// </summary>
        [Parseable("PointSupportReaction")]
        [Result(typeof(PointSupportReaction), ListProc.PointSupportReactionsLoadCase, ListProc.PointSupportReactionsLoadCombination)]
        PointSupportReaction,

        /// <summary>
        /// Line connection forces
        /// </summary>
        [Parseable("LineConnectionForce")]
        [Result(typeof(LineConnectionForce), ListProc.LineConnectionForceLoadCase, ListProc.LineConnectionForceLoadCombination)]
        LineConnectionForce,
        
        /// <summary>
        /// Line support group, Reactions
        /// </summary>
        [Parseable("LineSupportReaction")]
        [Result(typeof(LineSupportReaction), ListProc.LineSupportReactionsLoadCase, ListProc.LineSupportReactionsLoadCombination)]
        LineSupportReaction,

        /// <summary>
        /// Line support group, Resultants
        /// </summary>
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
        [Parseable("ShellInternalForceExtract")]
        [Result(typeof(ShellInternalForce), ListProc.ShellInternalForceExtractLoadCase, ListProc.ShellInternalForceExtractLoadCombination)]
        ShellInternalForceExtract,

        /// <summary>
        /// Shells, Derived Forces
        /// </summary>
        [Parseable("ShellDerivedForce")]
        [Result(typeof(ShellDerivedForce), ListProc.ShellDerivedForceLoadCase, ListProc.ShellDerivedForceLoadCombination)]
        ShellDerivedForce,

        /// <summary>
        /// Shells, Derived Forces
        /// </summary>
        [Parseable("ShellDerivedForceExtract")]
        [Result(typeof(ShellDerivedForce), ListProc.ShellDerivedForceExtractLoadCase, ListProc.ShellDerivedForceExtractLoadCombination)]
        ShellDerivedForceExtract,

        /// <summary>
        /// Shells, Stress
        /// </summary>
        [Parseable("ShellStressTop")]
        [Result(typeof(ShellStress), ListProc.ShellStressesTopLoadCase, ListProc.ShellStressesTopLoadCombination)]
        ShellStressTop,

        /// <summary>
        /// Shells, Stress
        /// </summary>
        [Parseable("ShellStressMembrane")]
        [Result(typeof(ShellStress), ListProc.ShellStressesMembraneLoadCase, ListProc.ShellStressesMembraneLoadCombination)]
        ShellStressMembrane,

        /// <summary>
        /// Shells, Stress
        /// </summary>
        [Parseable("ShellStressBottom")]
        [Result(typeof(ShellStress), ListProc.ShellStressesBottomLoadCase, ListProc.ShellStressesBottomLoadCombination)]
        ShellStressBottom,

        /// <summary>
        /// Shells, Stress (Extract)
        /// </summary>
        [Parseable("ShellStressTopExtract")]
        [Result(typeof(ShellStress), ListProc.ShellStressesTopExtractLoadCase, ListProc.ShellStressesTopExtractLoadCombination)]
        ShellStressTopExtract,

        /// <summary>
        /// Shells, Stress (Extract)
        /// </summary>
        [Parseable("ShellStressMembraneExtract")]
        [Result(typeof(ShellStress), ListProc.ShellStressesMembraneExtractLoadCase, ListProc.ShellStressesMembraneExtractLoadCombination)]
        ShellStressMembraneExtract,

        /// <summary>
        /// Shells, Stress (Extract)
        /// </summary>
        [Parseable("ShellStressBottomExtract")]
        [Result(typeof(ShellStress), ListProc.ShellStressesBottomExtractLoadCase, ListProc.ShellStressesBottomExtractLoadCombination)]
        ShellStressBottomExtract,

        /// <summary>
        /// Shells, Displacements
        /// </summary>
        [Parseable("ShellDisplacement")]
        [Result(typeof(ShellDisplacement), ListProc.ShellDisplacementLoadCase, ListProc.ShellDisplacementLoadCombination)]
        ShellDisplacement,

        /// <summary>
        /// Shells, Displacements (Extract)
        /// </summary>
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
        #endregion

        #region FOOTFALL
        /// <summary>
        /// Nodal Response Factor: FootFall
        /// </summary>
        [Parseable("NodalResponseFactor")]
        [Result(typeof(NodalResponseFactor), ListProc.NodalResponseFactor)]
        NodalResponseFactor,

        /// <summary>
        /// Nodal Acceleration: FootFall
        /// </summary>
        [Parseable("NodalAcceleration")]
        [Result(typeof(NodalAcceleration), ListProc.NodalAcceleration)]
        NodalAcceleration,

        #endregion

        #region RC design
        /* RC design */
        /// <summary>
        /// Shell, Utilization
        /// </summary>
        [Parseable("RCDesignShellUtilization")]
        [Result(typeof(ShellDisplacement), ListProc.RCDesignShellUtilizationLoadCombination)]
        RCDesignShellUtilization,

        /// <summary>
        /// Shell, Crack width
        /// </summary>
        [Parseable("RCDesignShellCrackWidth")]
        [Result(typeof(ShellDisplacement), ListProc.RCDesignShellCrackWidthLoadCombination)]
        RCDesignShellCrackWidth,
        #endregion
    }
}
