using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Calculate;


namespace FemDesign.Results
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public partial class Result2Attribute : Attribute
    {
        public readonly Type ResultType;
        public readonly ListProc[] ListProcs;
        public Result2Attribute(Type resultType, params ListProc[] listProc)
        {
            if (!typeof(IResult).IsAssignableFrom(resultType))
                throw new ArgumentException();

            ListProcs = listProc;
        }
    }
}