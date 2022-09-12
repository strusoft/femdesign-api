using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FemDesign.Results
{
    /// <summary>
    /// FEM-Design list tables result types
    /// </summary>
    public interface IResult
    {
        /*
         * Interface cannot have a static method in language version below 8.0
         */
        //internal static IResult Parse(string[] row, Results.CsvReader reader, Dictionary<string, string> HeaderData)
        //internal static Regex IdentificationExpression { get; }
        //internal static Regex HeaderExpression { get; }
    }
}
