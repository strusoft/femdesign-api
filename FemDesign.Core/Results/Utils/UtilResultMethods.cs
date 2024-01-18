using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using FemDesign.Calculate;
using System.ComponentModel;

namespace FemDesign.Results.Utils
{
    public static class UtilResultMethods
    {

        /// <summary>
        /// Filter the result list of type T by the name of the specified load combination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <param name="loadCombinationPropertyName">Type T property name related to load combination.</param>
        /// <param name="loadCombination">Load combination name to filter results.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> FilterResultsByLoadCombination<T>(this List<T> results, string loadCombinationPropertyName, string loadCombination) where T : IResult
        {
            PropertyInfo property = GetProperty<T>(loadCombinationPropertyName);

            if (!results.Select(r => (string)property.GetValue(r)).Contains(loadCombination, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Incorrect or unknown load combination name: {loadCombination}.");
            }
            var filteredResults = results.Where(r => String.Equals((string)property.GetValue(r), loadCombination, StringComparison.OrdinalIgnoreCase)).ToList();

            return filteredResults;
        }

        /// <summary>
        /// Filter the result list of type T by the names of the specified load combinations.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <param name="loadCombinationPropertyName">Type T property name related to load combinations.</param>
        /// <param name="loadCombination">List of load combination names to filter results.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> FilterResultsByLoadCombination<T>(this List<T> results, string loadCombinationPropertyName, List<string> loadCombination) where T : IResult
        {
            PropertyInfo property = GetProperty<T>(loadCombinationPropertyName);

            List<T> filteredResults = new List<T>();
            foreach (var comb in loadCombination)
            {
                if (!results.Select(r => (string)property.GetValue(r)).Contains(comb, StringComparer.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Incorrect or unknown load combination name: {comb}.");
                }
                var res = results.Where(r => String.Equals((string)property.GetValue(r), comb, StringComparison.OrdinalIgnoreCase)).ToList();
                
                filteredResults.AddRange(res);
            }

            return filteredResults;
        }

        /// <summary>
        /// Filter the result list of type T by the index of the specified shape identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <param name="shapeIdPropertyName">Type T property name related to shape identifier.</param>
        /// <param name="shapeId">Index of shape identifier to filter results.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> FilterResultsByShapeId<T>(this List<T> results, string shapeIdPropertyName, int shapeId) where T : IResult
        {
            PropertyInfo property = GetProperty<T>(shapeIdPropertyName);

            if ((shapeId < 1) || (shapeId > (int)results.Select(r => property.GetValue(r)).Max()))
            {
                throw new ArgumentException($"ShapeId {shapeId} is out of range.");
            }
            var filteredResults = results.Where(r => (int)property.GetValue(r) == shapeId).ToList();

            return filteredResults;
        }

        /// <summary>
        /// Filter the result list of type T by the indexes of the specified shape identifiers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <param name="shapeIdPropertyName">Type T property name related to shape identifiers.</param>
        /// <param name="shapeId">Indexes of shape identifiers to filter results.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> FilterResultsByShapeId<T>(this List<T> results, string shapeIdPropertyName, List<int> shapeId) where T : IResult
        {
            PropertyInfo property = GetProperty<T>(shapeIdPropertyName);

            List<T> filteredResults = new List<T>();
            foreach (var shape in shapeId)
            {
                if ((shape < 1) || (shape > (int)results.Select(r => property.GetValue(r)).Max()))
                {
                    throw new ArgumentException($"ShapeId {shape} is out of range.");
                }
                var res = results.Where(r => (int)property.GetValue(r) == shape).ToList();

                filteredResults.AddRange(res);
            }

            return filteredResults;
        }

        public static (List<string>, List<int>) GetCombosAndShapes<T>(this List<T> results, string loadCombinationPropertyName, string shapeIdPropertyName) where T : IResult
        {
            // get properties
            PropertyInfo comboProperty = GetProperty<T>(loadCombinationPropertyName);
            PropertyInfo shapeProperty = GetProperty<T>(shapeIdPropertyName);

            // get load combinations from results
            List<string> combsFromCalc = results.Select(r => (string)comboProperty.GetValue(r)).Distinct().ToList();

            // get number of shapes by load combinations
            List<int> maxIdsByComb = new List<int>();
            foreach (var combo in combsFromCalc)
            {
                maxIdsByComb.Add(
                    results.Where(r => (string)comboProperty.GetValue(r) == combo).Select(sr => (int)shapeProperty.GetValue(sr)).Max()
                    );
            }

            return (combsFromCalc, maxIdsByComb);
        }

        private static PropertyInfo GetProperty<T>(string propertyName) where T : IResult
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property {propertyName} doesn't exist in type {typeof(T).Name}.");
            }

            return property;
        }
    }
}
