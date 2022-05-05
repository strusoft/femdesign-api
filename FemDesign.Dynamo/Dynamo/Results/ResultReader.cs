using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;

namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class ResultsReader : CsvParser
    {
    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class CsvParser : IDisposable
    {
    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class ParseException : ApplicationException
    {
    }

}