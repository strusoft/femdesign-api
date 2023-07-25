using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


namespace FemDesign.Results.Utils
{
    public class UtilResultMethods
    {

        /// <summary>
        /// Filter the result list of type T by the name of the specified load combination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <param name="propertyName">Type T property name related to load combination.</param>
        /// <param name="loadCombination">Load combination name to filter results.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> FilterResultsByLoadCombination<T>(List<T> results, string propertyName, string loadCombination)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Porperty {property} doesn't exist in type {typeof(T).Name}.");
            }

            if (!results.Select(r => property.GetValue(r).ToString()).Contains(loadCombination, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Incorrect or unknown load combination name.");
            }
            var filteredResults = results.Where(r => String.Equals(property.GetValue(r).ToString(), loadCombination, StringComparison.OrdinalIgnoreCase)).ToList();

            return filteredResults;
        }

        /// <summary>
        /// Filter the result list of type T by the index of the specified shape identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <param name="propertyName">Type T property name related to shape identifier.</param>
        /// <param name="shapeId">Index of shape identifier to filter results.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> FilterResultsByShapeId<T>(List<T> results, string propertyName, int shapeId)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Porperty {property} doesn't exist in type {typeof(T).Name}.");
            }

            if ((shapeId < 1) || (shapeId > (int)results.Select(r => property.GetValue(r)).Max()))
            {
                throw new ArgumentException("ShapeId is out of range.");
            }
            var filteredResults = results.Where(r => (int)property.GetValue(r) == shapeId).ToList();

            return filteredResults;
        }
    }
}
