using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;
using FemDesign.Calculate;

using Newtonsoft.Json;

namespace FemDesign.Results
{
    public static class Convert
    {
        /// <summary>
        /// The string representation of null.
        /// </summary>
        private static readonly string Null = "null";
        public static string ToJSON(this object value)
        {
            if (value == null) return Null;

            try
            {
                string json = JsonConvert.SerializeObject(value);
                return json;
            }
            catch (Exception exception)
            {
                //log exception but dont throw one
                throw new Exception(exception.Message);
            }
        }
    }
}