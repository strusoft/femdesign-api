using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            "No declared structure to calculate."
        };
        
        public readonly static List<string> WarningMessase = new List<string>{
            "Large nodal displacement or rotation was found.",
            "One or more identical copies of 1 structural elements and loads are found."
        };


        public static string HasError(List<string> messages, out string error)
        {
            foreach (string message in messages)
            {
                if (ErrorMessage.Any(msg => message.Contains(msg)))
                {
                    error = ErrorMessage.First(msg => message.Contains(msg));
                    return error;
                }
            }

            error = null;
            return error;
        }

        public static string HasWarning(List<string> messages, out string warning)
        {
            foreach (string message in messages)
            {
                if (WarningMessase.Any(msg => message.Contains(msg)))
                {
                    warning = WarningMessase.First(msg => message.Contains(msg));
                    return warning;
                }
            }

            warning = null;
            return warning;
        }
    }
}