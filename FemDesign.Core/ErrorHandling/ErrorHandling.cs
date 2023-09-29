using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
            "Unknown error."
        };
        
        public readonly static List<string> WarningMessase = new List<string>{
            "Large nodal displacement or rotation was found.",
            "One or more identical copies of \\d+ structural elements and loads are found.",
        };


        public static string HasError(List<string> messages, out string error)
        {
            error = null;
            foreach (string message in messages)
            {
                foreach (var errorMsg in ErrorMessage)
                {
                    if (Regex.IsMatch(message, errorMsg))
                    {
                        error = errorMsg;
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
                foreach(var warningMsg in WarningMessase)
                {
                    if(Regex.IsMatch(message, warningMsg))
                    {
                        warning = warningMsg;
                        return warning;
                    }
                }
            }

            return warning;
        }
    }
}