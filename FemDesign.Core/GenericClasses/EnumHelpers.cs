﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ParseableAttribute : Attribute
    {
        public readonly string[] Names;

        public ParseableAttribute(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            Names = new[] { name };
        }

        public ParseableAttribute(params string[] names)
        {
            if (names == null || names.Any(x => x == null))
            {
                throw new ArgumentNullException();
            }

            Names = names;
        }
    }

    public static class EnumParser
    {
        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            try
            {
                return ParseEnumImpl<TEnum>.Values[value];
            }
            catch (KeyNotFoundException)
            {
                var possible = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
                string possibleString = string.Join(", ", possible);
                throw new ArgumentException($"Could not parse value '{value}' to type {typeof(TEnum)}. Must be one of the following values '{possibleString}'.");
            }
        }

        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return ParseEnumImpl<TEnum>.Values.TryGetValue(value, out result);
        }

        private static class ParseEnumImpl<TEnum> where TEnum : struct
        {
            public static readonly Dictionary<string, TEnum> Values = new Dictionary<string, TEnum>();

            static ParseEnumImpl()
            {
                var nameAttributes = typeof(TEnum)
                    .GetFields()
                    .Select(x => new
                    {
                        Value = x,
                        Names = x.GetCustomAttributes(typeof(ParseableAttribute), false)
                            .Cast<ParseableAttribute>()
                    });

                var degrouped = nameAttributes.SelectMany(
                    x => x.Names.SelectMany(y => y.Names),
                    (x, y) => new { Value = x.Value, Name = y });

                Values = degrouped.ToDictionary(
                    x => x.Name,
                    x => (TEnum)x.Value.GetValue(null));
            }
        }
    }
}
