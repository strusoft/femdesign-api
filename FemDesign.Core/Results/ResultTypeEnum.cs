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

        /* LOAD CASES AND COMBINATIONS */
        [Parseable("NodalDisplacement")]
        [Result(typeof(NodalDisplacement), ListProc.NodalDisplacementsLoadCase, ListProc.NodalDisplacementsLoadCombination)]
        NodalDisplacement,

        //[Parseable("BarEndForce")]
        //[Result(typeof(BarEndForce))]
        //BarEndForce,

        /// <summary>
        /// Point support group, Reactions
        /// </summary>
        [Parseable("PointSupportReaction")]
        [Result(typeof(PointSupportReaction), ListProc.PointSupportReactionsLoadCase, ListProc.PointSupportReactionsLoadCombination)]
        PointSupportReaction,
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
        /// Shells, Displacements (Extract)
        /// </summary>
        [Parseable("ShellDisplacementExtract")]
        [Result(typeof(ShellsDisplacement), ListProc.ShellDisplacementExtractLoadCase, ListProc.ShellDisplacementExtractLoadCombination)]
        ShellDisplacementExtract,

        /* RC design */
        /// <summary>
        /// Shell, Utilization
        /// </summary>
        [Parseable("RCDesignShellUtilization")]
        [Result(typeof(ShellsDisplacement), ListProc.RCDesignShellUtilizationLoadCombination)]
        RCDesignShellUtilization,

        /// <summary>
        /// Shell, Crack width
        /// </summary>
        [Parseable("RCDesignShellCrackWidth")]
        [Result(typeof(ShellsDisplacement), ListProc.RCDesignShellCrackWidthLoadCombination)]
        RCDesignShellCracking,
    }
}
