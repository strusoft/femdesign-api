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
