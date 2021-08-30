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
    public interface IQuantityEstimationResult : IResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Storey identifier
        /// </summary>
        string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        string Structure { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        string Quality { get; }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Concrete" result
    /// </summary>
    public class QuantityEstimationConcrete : IQuantityEstimationResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Volume quantity [mm3]
        /// </summary>
        public double Volume { get; }
        /// <summary>
        /// Formwork quantity [mm2]
        /// </summary>
        public double Formwork { get; }

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
    public class QuantityEstimationReinforcement : IQuantityEstimationResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Diameter [mm]
        /// </summary>
        public double Diameter { get; }
        /// <summary>
        /// Quantity [t]
        /// </summary>
        public double Quantity { get; }

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

    /// <summary>
    /// FemDesign "Quantity estimation, Steel" result
    /// </summary>
    public class QuantityEstimationSteel : IQuantityEstimationResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section/Thickness identifier
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Weight per length [t/m, t/m2]
        /// </summary>
        public double UnitWeight { get; }
        /// <summary>
        /// Subtotal [m, m2]
        /// </summary>
        public double Subtotal { get; }
        /// <summary>
        /// Total weight [t]
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area [m2]
        /// </summary>
        public double PaintedArea { get; }
        internal QuantityEstimationSteel(string id, string storey, string structure, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            Subtotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Steel)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, Steel|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationSteel Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double unitWeight = double.Parse(row[5], CultureInfo.InvariantCulture);
            double subtotal = double.Parse(row[6], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[7], CultureInfo.InvariantCulture);
            double paintedArea = double.Parse(row[8], CultureInfo.InvariantCulture);
            return new QuantityEstimationSteel(id, storey, structure, quality, section, unitWeight, subtotal, totalWeight, paintedArea);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Timber" result
    /// </summary>
    public class QuantityEstimationTimber : IQuantityEstimationResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section/Thickness identifier
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Weight per length [t/m, t/m2]
        /// </summary>
        public double UnitWeight { get; }
        /// <summary>
        /// Subtotal [m, m2]
        /// </summary>
        public double Subtotal { get; }
        /// <summary>
        /// Total weight [t]
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area [m2]
        /// </summary>
        public double PaintedArea { get; }
        internal QuantityEstimationTimber(string id, string storey, string structure, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            Subtotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Timber)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, Timber|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationTimber Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double unitWeight = double.Parse(row[5], CultureInfo.InvariantCulture);
            double subtotal = double.Parse(row[6], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[7], CultureInfo.InvariantCulture);
            double paintedArea = double.Parse(row[8], CultureInfo.InvariantCulture);
            return new QuantityEstimationTimber(id, storey, structure, quality, section, unitWeight, subtotal, totalWeight, paintedArea);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Timber" result
    /// </summary>
    public class QuantityEstimationProfiledPlate : IQuantityEstimationResult
    {
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section/Thickness identifier
        /// </summary>
        public string Section { get; }

        public double Type { get; }
        /// <summary>
        /// Length of plate [m]
        /// </summary>
        public double Length { get; }
        /// <summary>
        /// Width of plate [m]
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Height of plate [m]
        /// </summary>
        public double Height { get; }
        /// <summary>
        /// Area of the plate [m2]
        /// </summary>
        public double Area { get; }
        /// <summary>
        /// Total weight [t]
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Count/Sum of sections
        /// </summary>
        public int Sum { get; }
        internal QuantityEstimationProfiledPlate(string id, string storey, string structure, string quality, string section, double type, double length, double width, double height, double area, double totalWeight, int count)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            Type = type;
            Length = length;
            Width = width;
            Height = height;
            Area = area;
            TotalWeight = totalWeight;
            Sum = count;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}";
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Profiled panel)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, Profiled panel|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationProfiledPlate Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            double type = double.Parse(row[5], CultureInfo.InvariantCulture);
            double length = double.Parse(row[6], CultureInfo.InvariantCulture);
            double width = double.Parse(row[7], CultureInfo.InvariantCulture);
            double height = double.Parse(row[8], CultureInfo.InvariantCulture);
            double area = double.Parse(row[9], CultureInfo.InvariantCulture);
            double weight = double.Parse(row[10], CultureInfo.InvariantCulture);
            int count = int.Parse(row[11], CultureInfo.InvariantCulture);
            return new QuantityEstimationProfiledPlate(id, storey, structure, quality, section, type, length, width, height, area, weight, count);
        }
    }
}
