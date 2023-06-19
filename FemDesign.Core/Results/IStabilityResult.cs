using System;
using System.Collections.Generic;
using System.Linq;


namespace FemDesign.Results
{
    public interface IStabilityResult : IResult
    {
        string CaseIdentifier { get; }
        int Shape { get; }
    }

}
