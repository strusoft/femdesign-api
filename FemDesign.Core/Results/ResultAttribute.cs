using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.Calculate;


namespace FemDesign.Results
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ResultAttribute : Attribute
    {
        public readonly Type ResultType;
        public readonly ListProc CaseListProc;
        public readonly ListProc CombinationListProc;
        public ResultAttribute(Type resultType, ListProc caseListProc, ListProc combinationListProc)
        {
            if (resultType == null)
                throw new ArgumentNullException();

            if (!typeof(IResult).IsAssignableFrom(resultType))
                throw new ArgumentException();

            ResultType = resultType;
            CaseListProc = caseListProc;
            CombinationListProc = combinationListProc;
        }
    }

    public static class ResultAttributeExtentions
    {
        public static Dictionary<ResultType, ResultAttribute> ResultAttributes = new Dictionary<ResultType, ResultAttribute>();
        public static Dictionary<ResultType, Type> ResultTypes = new Dictionary<ResultType, Type>();
        public static Dictionary<ResultType, ListProc> CaseListProcs = new Dictionary<ResultType, ListProc>();
        public static Dictionary<ResultType, ListProc> CombinationListProcs = new Dictionary<ResultType, ListProc>();

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
            CaseListProcs = ResultAttributes.ToDictionary(kv => kv.Key, kv => kv.Value.CaseListProc);
            CombinationListProcs = ResultAttributes.ToDictionary(kv => kv.Key, kv => kv.Value.CombinationListProc);
        }
    }
}
