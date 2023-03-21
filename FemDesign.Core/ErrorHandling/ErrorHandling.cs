using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Utils
{
    public static class ErrorHandling
    {
        public static List<string> ErrorMessage = new List<string>{
            "No declared load or dead load.",
            "No declared mass.",
            "No declared load combination.",
            "No declared support."
        };
        
        public static List<string> WarningMessase = new List<string>{
            "Dlg message ## ~Large nodal displacement or rotation was found."
        };


        public static string HasError(List<string> message, out string error)
        {
            foreach(string s in message)
            {
                if (ErrorMessage.Contains(s))
                {
                    error = s;
                    return error;
                }
            }
            error = null;
            return error;
        }

        public static string HasWarning(List<string> message, out string warning)
        {
            foreach (string s in message)
            {
                if (WarningMessase.Contains(s))
                {
                    warning = s;
                    return warning;
                }
            }
            warning = null;
            return warning;
        }


    }
}
