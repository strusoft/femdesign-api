using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;
using FemDesign.Calculate;
using Newtonsoft.Json;
#if ISDYNAMO
using Autodesk.DesignScript.Runtime;
#endif

namespace FemDesign.Results
{
#if ISDYNAMO
[IsVisibleInDynamoLibrary(false)]
#endif
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

        /// <summary>
        /// Total weight
        /// </summary>
        double TotalWeight { get; }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Concrete" result
    /// </summary>
    [Result(typeof(QuantityEstimationConcrete), ListProc.QuantityEstimationConcrete)]
    public partial class QuantityEstimationConcrete : IQuantityEstimationResult
    {
        /// <summary>
        /// Storey identifier
        /// </summary>
        public string Storey { get; }
        /// <summary>
        /// Structural element type
        /// </summary>
        public string Structure { get; }
        /// <summary>
        /// Element name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Material quality identifier
        /// </summary>
        public string Quality { get; }
        /// <summary>
        /// Section
        /// </summary>
        public string Section { get; }
        /// <summary>
        /// Length/Area Quantity
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Volume quantity
        /// </summary>
        public double Volume { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Formwork quantity
        /// </summary>
        public double Formwork { get; }
        /// <summary>
        /// Reinforcement weight per length/area
        /// </summary>
        public double Reinforcement { get; }

        [JsonConstructor]
        internal QuantityEstimationConcrete(string id, string storey, string structure, string section, string quality, double subTotal, double volume, double totalWeight, double formwork, double reinforcement)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Section = section;
            Quality = quality;
            SubTotal = subTotal;
            Volume = volume;
            TotalWeight = totalWeight;
            Formwork = formwork;
            Reinforcement = reinforcement;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
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
            string section = row[4];
            double subTotal = double.Parse(row[7], CultureInfo.InvariantCulture);
            double volume = double.Parse(row[8], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[9], CultureInfo.InvariantCulture);
            double formwork = double.Parse(row[10], CultureInfo.InvariantCulture);
            double reinforcement = double.Parse(row[11], CultureInfo.InvariantCulture);
            //string test = HeaderData["result"];
            return new QuantityEstimationConcrete(id, storey, structure, section, quality, subTotal, volume, totalWeight, formwork, reinforcement);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Reinforcement" result
    /// </summary>
    [Result(typeof(QuantityEstimationReinforcement), ListProc.QuantityEstimationReinforcement)]
    public partial class QuantityEstimationReinforcement : IQuantityEstimationResult
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
        /// Diameter
        /// </summary>
        public double Diameter { get; }
        /// <summary>
        /// Total weigt
        /// </summary>
        public double TotalWeight { get; }

        [JsonConstructor]
        internal QuantityEstimationReinforcement(string id, string storey, string structure, string quality, double diameter, double quantity)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Diameter = diameter;
            TotalWeight = quantity;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Reinforcement)$");
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
    [Result(typeof(QuantityEstimationSteel), ListProc.QuantityEstimationSteel)]
    public partial class QuantityEstimationSteel : IQuantityEstimationResult
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
        /// Subtotal
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area
        /// </summary>
        public double PaintedArea { get; }
        [JsonConstructor]
        internal QuantityEstimationSteel(string id, string storey, string structure, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            SubTotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
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
    [Result(typeof(QuantityEstimationTimber), ListProc.QuantityEstimationTimber)]
    public partial class QuantityEstimationTimber : IQuantityEstimationResult
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
        /// Subtotal
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area
        /// </summary>
        public double PaintedArea { get; }
        [JsonConstructor]
        internal QuantityEstimationTimber(string id, string storey, string structure, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            SubTotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Timber)$");
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
    /// FemDesign "Quantity estimation, Timber panel" result
    /// </summary>
    [Result(typeof(QuantityEstimationTimberPanel), ListProc.QuantityEstimationTimberPanel)]
    public partial class QuantityEstimationTimberPanel : IQuantityEstimationResult
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
        /// Thickness [mm]
        /// </summary>
        public double Thickness { get; }
        /// <summary>
        /// Panel type
        /// </summary>
        public string PanelType { get; }
        /// <summary>
        /// Length of panel
        /// </summary>
        public double Length { get; }
        /// <summary>
        /// Width of panel
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Area of panel
        /// </summary>
        public double Area { get; }
        /// <summary>
        /// Weight per unit
        /// </summary>
        public double Weight { get; }
        /// <summary>
        /// Number of panels of type
        /// </summary>
        public int Count { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        [JsonConstructor]
        internal QuantityEstimationTimberPanel(string id, string storey, string structure, string quality, double thickness, string panelType, double length, double width, double area, double weight, int count, double totalWeight)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Thickness = thickness;
            PanelType = panelType;
            Length = length;
            Width = width;
            Area = area;
            Weight = weight;
            Count = count;
            TotalWeight = totalWeight;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Timber panel)$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, Timber panel|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationTimberPanel Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            double thickness = double.Parse(row[4], CultureInfo.InvariantCulture);
            string panelType = row[5];
            double length = double.Parse(row[6], CultureInfo.InvariantCulture);
            double width = double.Parse(row[7], CultureInfo.InvariantCulture);
            double area = double.Parse(row[8], CultureInfo.InvariantCulture);
            double weight = double.Parse(row[9], CultureInfo.InvariantCulture);
            int pcs = int.Parse(row[10], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[11], CultureInfo.InvariantCulture);
            return new QuantityEstimationTimberPanel(id, storey, structure, quality, thickness, panelType, length, width, area, weight, pcs, totalWeight);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Timber" result
    /// </summary>
    [Result(typeof(QuantityEstimationProfiledPlate), ListProc.QuantityEstimationProfiledPanel)]
    public partial class QuantityEstimationProfiledPlate : IQuantityEstimationResult
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
        /// Panel type
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Length of plate
        /// </summary>
        public double Length { get; }
        /// <summary>
        /// Width of plate
        /// </summary>
        public double Width { get; }
        /// <summary>
        /// Weight of single plate
        /// </summary>
        public double Weight { get; }
        /// <summary>
        /// Area of the plate
        /// </summary>
        public double Area { get; }
        /// <summary>
        /// Total weight of all plates
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Count/Sum of sections
        /// </summary>
        public int Count { get; }
        [JsonConstructor]
        internal QuantityEstimationProfiledPlate(string id, string storey, string structure, string quality, string section, string type, double length, double width, double weight, double area, double totalWeight, int count)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            Type = type;
            Length = length;
            Width = width;
            Weight = weight;
            Area = area;
            TotalWeight = totalWeight;
            Count = count;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Quantity estimation), (?'result'Profiled panel)$");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^Quantity estimation, Profiled panel$|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationProfiledPlate Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            string section = row[4];
            //double thickness = double.Parse(row[5], CultureInfo.InvariantCulture);
            string type = row[6];
            double length = double.Parse(row[7], CultureInfo.InvariantCulture);
            double width = double.Parse(row[8], CultureInfo.InvariantCulture);
            double area = double.Parse(row[9], CultureInfo.InvariantCulture);
            double weight = double.Parse(row[10], CultureInfo.InvariantCulture);
            int count = int.Parse(row[11], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[12], CultureInfo.InvariantCulture);
            return new QuantityEstimationProfiledPlate(id, storey, structure, quality, section, type, length, width, weight, area, totalWeight, count);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, Concrete" result
    /// </summary>
    [Result(typeof(QuantityEstimationMasonry), ListProc.QuantityEstimationMasonry)]
    public partial class QuantityEstimationMasonry : IQuantityEstimationResult
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
        /// Thickness
        /// </summary>
        public double Thickness { get; }
        /// <summary>
        /// Weight per length or area unit
        /// </summary>
        public double UnitWeight { get; }
        /// <summary>
        /// Length/Area Quantity
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }

        [JsonConstructor]
        internal QuantityEstimationMasonry(string id, string storey, string structure, string quality, double thickness, double unitWeight, double subTotal, double totalWeight)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Thickness = thickness;
            UnitWeight = unitWeight;
            SubTotal = subTotal;
            TotalWeight = totalWeight;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'Masonry)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, Masonry|^Storey\t|^\ttype\t+\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationMasonry Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string storey = row[0] == "-" ? null : row[0];
            string structure = row[1];
            string id = row[2];
            string quality = row[3];
            double thickness = double.Parse(row[4], CultureInfo.InvariantCulture);
            double unitWeight = double.Parse(row[5], CultureInfo.InvariantCulture);
            double subTotal = double.Parse(row[6], CultureInfo.InvariantCulture);
            double totalWeight = double.Parse(row[7], CultureInfo.InvariantCulture);
            return new QuantityEstimationMasonry(id, storey, structure, quality, thickness, unitWeight, subTotal, totalWeight);
        }
    }

    /// <summary>
    /// FemDesign "Quantity estimation, General" result
    /// </summary>
    [Result(typeof(QuantityEstimationGeneral), ListProc.QuantityEstimationGeneral)]
    public partial class QuantityEstimationGeneral : IQuantityEstimationResult
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
        /// Subtotal
        /// </summary>
        public double SubTotal { get; }
        /// <summary>
        /// Total weight
        /// </summary>
        public double TotalWeight { get; }
        /// <summary>
        /// Painted area
        /// </summary>
        public double PaintedArea { get; }
        [JsonConstructor]
        internal QuantityEstimationGeneral(string id, string storey, string structure, string quality, string section, double unitWeight, double subtotal, double totalWeight, double paintedArea)
        {
            Storey = storey;
            Structure = structure;
            Id = id;
            Quality = quality;
            Section = section;
            UnitWeight = unitWeight;
            SubTotal = subtotal;
            TotalWeight = totalWeight;
            PaintedArea = paintedArea;
        }

        public override string ToString()
        {
            return ResultsReader.ObjectRepresentation(this);
        }

        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"(?'type'Quantity estimation), (?'result'General)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"Quantity estimation, General|Storey\t|\t*\[.+\]|TOTAL\t");
            }
        }

        internal static QuantityEstimationGeneral Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
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
            return new QuantityEstimationGeneral(id, storey, structure, quality, section, unitWeight, subtotal, totalWeight, paintedArea);
        }
    }
}
