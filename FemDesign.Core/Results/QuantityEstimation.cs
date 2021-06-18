using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;


namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Quantity estimation, Concrete" result
    /// </summary>
    public class QuantityEstimationConcrete : IResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        public readonly string Id;
        /// <summary>
        /// Storey identifier
        /// </summary>
        public readonly string Storey;
        /// <summary>
        /// Structural element type
        /// </summary>
        public readonly string Structure;
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public readonly string Quality;
        /// <summary>
        /// Volume quantity [m3]
        /// </summary>
        public readonly double Volume;
        /// <summary>
        /// Formwork quantity [m2]
        /// </summary>
        public readonly double Formwork;

        internal QuantityEstimationConcrete(string id, string storey, string structure, string quality, double volume, double formwork)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Volume = volume;
            Formwork = formwork;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Concrete)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, Concrete|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationConcrete Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            double volume = double.Parse(row[4], CultureInfo.InvariantCulture);
            double formwork = double.Parse(row[5], CultureInfo.InvariantCulture);
            return new QuantityEstimationConcrete(id, storey, structure, quality, volume, formwork);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Reinforcement" result
    /// </summary>
    public class QuantityEstimationReinforcement : IResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        public readonly string Id;
        /// <summary>
        /// Storey identifier
        /// </summary>
        public readonly string Storey;
        /// <summary>
        /// Structural element type
        /// </summary>
        public readonly string Structure;
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public readonly string Quality;
        /// <summary>
        /// Diameter [mm]
        /// </summary>
        public readonly double Diameter;
        /// <summary>
        /// Quantity [t]
        /// </summary>
        public readonly double Quantity;

        internal QuantityEstimationReinforcement(string id, string storey, string structure, string quality, double diameter, double quantity)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Diameter = diameter;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Reinforcement)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, Reinforcement|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationReinforcement Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            double diameter = double.Parse(row[4], CultureInfo.InvariantCulture);
            double quantity = double.Parse(row[5], CultureInfo.InvariantCulture);
            return new QuantityEstimationReinforcement(id, storey, structure, quality, diameter, quantity);
        }
    }
}
