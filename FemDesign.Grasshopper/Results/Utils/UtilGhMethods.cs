using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;


namespace FemDesign.Grasshopper
{
    public static class UtilGhMethods
    {
        public static DataTree<T> CreateResultTree<T>(this List<T> results, string propertyName) where T : FemDesign.Results.IResult
        {
            DataTree<T> resultsTree = new DataTree<T>();

            // check property
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Porperty {property} doesn't exist in type {typeof(T).Name}.");
            }

            // set tree values by property type
            Type propType = property.PropertyType;
            if (propType == typeof(int))
            {
                // get property values
                var uniquePropertyValues = results.Select(r => (int)property.GetValue(r)).Distinct().ToList();

                // set tree values
                for (int i = 0; i < uniquePropertyValues.Count; i++)
                {
                    var allResultsByProperty = results.Where(r => (int)property.GetValue(r) == uniquePropertyValues[i]).ToList();
                    resultsTree.AddRange(allResultsByProperty, new GH_Path(i));
                }
            }
            else if (propType == typeof(string))
            {
                // get property values
                var uniquePropertyValues = results.Select(r => property.GetValue(r).ToString()).Distinct().ToList();

                // set tree values
                for (int i = 0; i < uniquePropertyValues.Count; i++)
                {
                    var allResultsByProperty = results.Where(r => property.GetValue(r).ToString() == uniquePropertyValues[i]).ToList();
                    resultsTree.AddRange(allResultsByProperty, new GH_Path(i));
                }
            }

            return resultsTree;
        }
    }
}
