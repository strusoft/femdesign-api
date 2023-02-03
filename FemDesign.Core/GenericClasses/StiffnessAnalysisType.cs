using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    public enum StiffnessAnalysisType
    {
        [Parseable("SameForAllCalculation")]
        SameForAllCalculation,
        [Parseable("FirstOrderU")]
        FirstOrderU,
        [Parseable("FirstOrderSq")]
        FirstOrderSq,
        [Parseable("FirstOrderSf")]
        FirstOrderSf,
        [Parseable("FirstOrderSc")]
        FirstOrderSc,
        [Parseable("SecondOrderU")]
        SecondOrderU,
        [Parseable("SecondOrderSq")]
        SecondOrderSq,
        [Parseable("SecondOrderSf")]
        SecondOrderSf,
        [Parseable("SecondOrderSc")]
        SecondOrderSc,
        [Parseable("Stability")]
        Stability,
        [Parseable("Dynamic")]
        Dynamic
    }
}
