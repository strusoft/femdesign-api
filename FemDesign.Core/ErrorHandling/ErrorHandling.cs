using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using FemDesign.Results;

namespace FemDesign.Utils
{
    public static class ErrorHandling
    {
        public readonly static List<string> ErrorMessage = new List<string>{
            "No declared load or dead load.",
            "No declared mass.",
            "No declared mass or converted load case.",
            "No declared load combination.",
            "No declared support.",
            "No declared structure to calculate.",
            "None of the Construction stages contains any load case.",
            "Time-dependent analysis is requested, but none of the object has Time-dependent property.",
            "Unknown error.",
            "Model loading problems",
            "The loading process aborted!",
            @"^Error",
            @"^ERROR",
        };
        
        public readonly static List<string> WarningMessage = new List<string>{
            "Large nodal displacement or rotation was found.",
            @"One or more identical copies of \\d+ structural elements and loads are found.",
            "Not enough data for time history calculation.",
            "Applied reinforcement is missing for cracked section analysis.",
            @"^Warning",
            @"^WARNING",
        };


        public static string HasError(List<string> messages, out string error)
        {
            error = null;
            foreach (string message in messages)
            {
                foreach (var errorMsg in ErrorMessage)
                {
                    if (Regex.IsMatch(message, errorMsg, RegexOptions.Multiline))
                    {
                        error = message;
                        return error;
                    }
                }
            }

            return error;
        }


        public static string HasWarning(List<string> messages, out string warning)
        {
            warning = null;
            foreach (string message in messages)
            {
                foreach(var warningMsg in WarningMessage)
                {                    
                    if(Regex.IsMatch(message, warningMsg, RegexOptions.Multiline))
                    {
                        warning = message;
                        return warning;
                    }
                }
            }

            return warning;
        }

    }
}