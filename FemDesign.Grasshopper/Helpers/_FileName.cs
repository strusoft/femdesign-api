using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Components;

namespace FemDesign.Grasshopper
{
    internal static class _FileName
    {
        public static string _NotASCIIPattern = "[\x80-\xFF]";

        public static bool IsASCII(string filePath)
        {
            Match m = Regex.Match(filePath, _NotASCIIPattern, RegexOptions.IgnoreCase);
            return m.Success;
        }
    }
}
