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
        [Result(typeof(QuantityEstimationConcrete), ListProc.QuantityEstimationConcrete, ListProc.QuantityEstimationConcrete)]
        QuantityEstimationConcrete,
        /// <summary>
        /// Quantity estimation, Steel
        /// </summary>
        [Parseable("QuantityEstimationSteel")]
        [Result(typeof(QuantityEstimationSteel), ListProc.QuantityEstimationSteel, ListProc.QuantityEstimationSteel)]
        QuantityEstimationSteel,
        /// <summary>
        /// Quantity estimation, Timber
        /// </summary>
        [Parseable("QuantityEstimationTimber")]
        [Result(typeof(QuantityEstimationTimber), ListProc.QuantityEstimationTimber, ListProc.QuantityEstimationTimber)]
        QuantityEstimationTimber,
        /// <summary>
        /// Quantity estimation, Profiled panel
        /// </summary>
        [Parseable("QuantityEstimationProfiledPanel")]
        [Result(typeof(QuantityEstimationProfiledPlate), ListProc.QuantityEstimationProfiledPanel, ListProc.QuantityEstimationProfiledPanel)]
        QuantityEstimationProfiledPanel,

        /* LOAD CASES AND COMBINATIONS */
        [Parseable("NodalDisplacement")]
        [Result(typeof(NodalDisplacement), ListProc.NodalDisplacementsLoadCase, ListProc.NodalDisplacementsLoadCombination)]
        NodalDisplacement,

        //[Parseable("BarEndForce")]
        //[Result(typeof(BarEndForce))]
        //BarEndForce,

        [Parseable("PointSupportReaction")]
        [Result(typeof(PointSupportReaction), ListProc.PointSupportReactionsLoadCase, ListProc.PointSupportReactionsLoadCombination)]
        PointSupportReaction,

        //[Parseable("LineSupportReaction")]
        //[Result(typeof(LineSupportReaction))]
        //LineSupportReaction,
    }
}
