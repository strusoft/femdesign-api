using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace FemDesign.GenericClasses
{
    public partial class IndexedGuid
    {
        private static Regex Pattern = new Regex(@"(?'guid'[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12})(#(?'index'[1-9][0-9]{0,4}))?");
        public Guid Guid { get; set; }
        public int? Index { get; set; }
        public bool HasIndex
        {
            get
            {
                return this.Index.HasValue;
            }
        }
        public override string ToString()
        {
            return this.HasIndex ? $"{Guid}#{Index.Value}" : $"{Guid}";
        }
        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private IndexedGuid() { }
        public IndexedGuid(string g)
        {
            var match = Pattern.Match(g);
            if (match.Groups["guid"].Success)
                Guid = new Guid(match.Groups["guid"].Value);
            else
                throw new ArgumentException("Guid part of string is not valid");
            if (g.Contains("#"))
            {
                if (match.Groups["index"].Success)
                    Index = int.Parse(match.Groups["index"].Value);
                else
                    throw new ArgumentException("Index part of string is not valid");
            }
        }
        public IndexedGuid(Guid guid)
        {
            Guid = guid;
        }
        public IndexedGuid(Guid guid, int index): this($"{guid}#{index}") { }

        public override bool Equals(object o)
        {
            return (o as IndexedGuid)?.Guid == this.Guid && (o as IndexedGuid)?.Index == this.Index;
        }
        public static bool operator ==(IndexedGuid lhs, IndexedGuid rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(IndexedGuid lhs, IndexedGuid rhs) => !(lhs == rhs);
        public static implicit operator IndexedGuid(Guid guid) => new IndexedGuid(guid);
        public static explicit operator Guid(IndexedGuid guid) => guid.Guid;
    }
}
