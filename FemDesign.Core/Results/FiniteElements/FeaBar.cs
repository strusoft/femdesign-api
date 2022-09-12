using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;

using FemDesign.Calculate;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Node" result
    /// </summary>
    [Result(typeof(FeaBar), ListProc.FeaBar)]
    public partial class FeaBar : IResult
    {
        /// <summary>
        /// Element Name Identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Element Index
        /// </summary>
        public int ElementId { get; }

        /// <summary>
        /// X-coordinate
        /// </summary>
        public int Nodei { get; }

        /// <summary>
        /// Y-coordinate
        /// </summary>
        public int Nodej { get; }


        internal FeaBar(string id, int elementId, int nodei, int nodej)
        {
            this.Id = id;
            this.ElementId = elementId;
            this.Nodei = nodei;
            this.Nodej = nodej;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {ElementId}, {Nodei}, {Nodej}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Bar elements)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Bar elements)|^Bar\t");
            }
        }

        internal static FeaBar Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string id = row[0];
            int elementId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            int nodei = Int32.Parse(row[2], CultureInfo.InvariantCulture);
            int nodej = Int32.Parse(row[3], CultureInfo.InvariantCulture);
            string test = HeaderData["type"];
            return new FeaBar(id, elementId, nodei, nodej);
        }

        /// <summary>
        /// The method has been created for returning the value for Grasshopper and Dynamo.
        /// The method can still be use for C# users.
        /// </summary>
        public static Dictionary<string, object> DeconstructFeaBar(List<FemDesign.Results.FeaBar> Result)
        {
            var feaBars = Result.Cast<FemDesign.Results.FeaBar>();

            // Parse Results from the object
            var id = feaBars.Select(n => n.Id).ToList();
            var elementId = feaBars.Select(n => n.ElementId).ToList();
            var nodei = feaBars.Select(n => n.Nodei).ToList();
            var nodej = feaBars.Select(n => n.Nodej).ToList();

            return new Dictionary<string, dynamic>
            {
                {"Identifier", id},
                {"ElementId", elementId},
                {"Nodei", nodei},
                {"Nodej", nodej}
            };
        }
    }
}