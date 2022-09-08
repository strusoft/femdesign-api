using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Calculate;


namespace FemDesign.Results
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public partial class ResultAttribute : Attribute
    {
        public readonly Type ResultType;
        public readonly ListProc[] ListProcs;
        public ResultAttribute(Type resultType, params ListProc[] listProc)
        {
            if (!typeof(IResult).IsAssignableFrom(resultType))
                throw new ArgumentException();

            ListProcs = listProc;
        }
    }
}