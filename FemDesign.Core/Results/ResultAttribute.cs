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

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public partial class ResultAttribute : Attribute
    {
        public readonly Type ResultType;
        public readonly ListProc[] ListProcs;
        public ResultAttribute(Type resultType, params ListProc[] listProc)
        {
            if (resultType == null)
                throw new ArgumentNullException();

            if (!typeof(IResult).IsAssignableFrom(resultType))
                throw new ArgumentException();

            ResultType = resultType;
            ListProcs = listProc;
        }
    }

    public static class ResultAttributeExtentions
    {
        public static Dictionary<ResultType, ResultAttribute> ResultAttributes = new Dictionary<ResultType, ResultAttribute>();
        public static Dictionary<ResultType, Type> ResultTypes = new Dictionary<ResultType, Type>();
        public static Dictionary<ResultType, ListProc[]> ListProcs = new Dictionary<ResultType, ListProc[]>();

        static ResultAttributeExtentions()
        {
            var resultAttributes = typeof(ResultType)
                    .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .Select(x => new
                    {
                        Field = x,
                        ResultAttributes = x.GetCustomAttributes(typeof(ResultAttribute), false)
                            .Cast<ResultAttribute>()
                    }).ToList();

            ResultAttributes = resultAttributes.ToDictionary(v => (ResultType)v.Field.GetValue(null), v => v.ResultAttributes.First());
            ResultTypes = ResultAttributes.ToDictionary(kv => kv.Key, kv => kv.Value.ResultType);
            ListProcs = ResultAttributes.ToDictionary(kv => kv.Key, kv => kv.Value.ListProcs);
        }
    }
}