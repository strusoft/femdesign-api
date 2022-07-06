using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationConcrete : IQuantityEstimationResult
    {
        /// <summary>
        /// Read Bar Stress from a previously run model.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "Storey", "Structure", "Id", "Quality", "Section", "Subtotal", "Volume", "TotalWeigth", "Formwork", "Reinforcement" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.QuantityEstimationConcrete> Result)
        {
            var storey = Result.Select(x => x.Storey);
            var structure = Result.Select(x => x.Structure);
            var id = Result.Select(x => x.Id);
            var quality = Result.Select(x => x.Quality);
            var section = Result.Select(x => x.Section);
            var subTotal = Result.Select(x => x.SubTotal);
            var volume = Result.Select(x => x.Volume);
            var totalWeight = Result.Select(x => x.TotalWeight);
            var formwork = Result.Select(x => x.Formwork);
            var reinforcement = Result.Select(x => x.Reinforcement);

            return new Dictionary<string, object>
            {
                {"Storey", storey},
                {"Structure", structure},
                {"Id", id},
                {"Quality", quality},
                {"Section", section},
                {"Subtotal", subTotal},
                {"Volume", volume},
                {"TotalWeigth", totalWeight},
                {"Formwork", formwork},
                {"Reinforcement", reinforcement},
            };
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationReinforcement : IQuantityEstimationResult
    {
        /// <summary>
        /// Read Bar Stress from a previously run model.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "Storey", "Structure", "Id", "Quality", "Diameter", "TotalWeight"})]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.QuantityEstimationReinforcement> Result)
        {
            var storey = Result.Select(x => x.Storey);
            var structure = Result.Select(x => x.Structure);
            var id = Result.Select(x => x.Id);
            var quality = Result.Select(x => x.Quality);
            var diameter = Result.Select(x => x.Diameter);
            var totalWeight = Result.Select(x => x.TotalWeight);

            return new Dictionary<string, object>
            {
                {"Storey", storey},
                {"Structure", structure},
                {"Id", id},
                {"Quality", quality},
                {"Diameter", diameter},
                {"TotalWeigth", totalWeight},
            };
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationSteel : IQuantityEstimationResult
    {
        /// <summary>
        /// Read Bar Stress from a previously run model.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "Storey", "Structure", "Id", "Quality", "Section", "UnitWeight", "Subtotal", "TotalWeigth", "PaintedArea" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.QuantityEstimationSteel> Result)
        {
            var storey = Result.Select(x => x.Storey);
            var structure = Result.Select(x => x.Structure);
            var id = Result.Select(x => x.Id);
            var quality = Result.Select(x => x.Quality);
            var section = Result.Select(x => x.Section);
            var unitWeight = Result.Select(x => x.UnitWeight);
            var subTotal = Result.Select(x => x.SubTotal);
            var totalWeight = Result.Select(x => x.TotalWeight);
            var paintedArea = Result.Select(x => x.PaintedArea);


            return new Dictionary<string, object>
            {
                {"Storey", storey},
                {"Structure", structure},
                {"Id", id},
                {"Quality", quality},
                {"Section", section},
                {"UnitWeight", unitWeight},
                {"Subtotal", subTotal},
                {"TotalWeigth", totalWeight},
                {"PaintedArea", paintedArea},
            };
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationTimberPanel : IQuantityEstimationResult
    {
        /// <summary>
        /// Read Bar Stress from a previously run model.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "Storey", "Structure", "Id", "Quality", "PanelType", "Thickness", "Length", "Width", "Area", "Pieces", "TotalWeigth" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.QuantityEstimationTimberPanel> Result)
        {

            var structure = Result.Select(x => x.Structure);
            var storey = Result.Select(x => x.Storey);
            var id = Result.Select(x => x.Id);
            var quality = Result.Select(x => x.Quality);
            var panelType = Result.Select(x => x.PanelType);
            var thickness = Result.Select(x => x.Thickness);
            var totalWeight = Result.Select(x => x.TotalWeight);
            var length = Result.Select(x => x.Length);
            var width = Result.Select(x => x.Width);
            var area = Result.Select(x => x.Area);
            var pieces = Result.Select(x => x.Count);


            return new Dictionary<string, object>
            {
                {"Storey", storey},
                {"Structure", structure},
                {"Id", id},
                {"Quality", quality},
                {"PanelType", panelType},
                {"Thickness", thickness},
                {"Length", length},
                {"Width", width},
                {"Area", area},
                {"Pieces", pieces},
                {"TotalWeigth", totalWeight},
            };
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationTimber : IQuantityEstimationResult
    {
        /// <summary>
        /// Read Bar Stress from a previously run model.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "Storey", "Structure", "Id", "Quality", "Section", "UnitWeight", "SubTotal", "PaintedArea", "TotalWeigth" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.QuantityEstimationTimber> Result)
        {
            var storey = Result.Select(x => x.Storey);
            var structure = Result.Select(x => x.Structure);
            var id = Result.Select(x => x.Id);
            var quality = Result.Select(x => x.Quality);
            var section = Result.Select(x => x.Section);
            var unitWeight = Result.Select(x => x.UnitWeight);
            var subTotal = Result.Select(x => x.SubTotal);
            var paintedArea = Result.Select(x => x.PaintedArea);
            var totalWeight = Result.Select(x => x.TotalWeight);

            return new Dictionary<string, object>
            {
                {"Storey", storey},
                {"Structure", structure},
                {"Id", id},
                {"Quality", quality},
                {"Section", section},
                {"UnitWeight", unitWeight},
                {"Subtotal", subTotal},
                {"PaintedArea", paintedArea},
                {"TotalWeigth", totalWeight},
            };
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationProfiledPlate : IQuantityEstimationResult
    {
    }


    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationMasonry : IQuantityEstimationResult
    {
    }


    [IsVisibleInDynamoLibrary(false)]
    public partial class QuantityEstimationGeneral : IQuantityEstimationResult
    {
        /// <summary>
        /// Read Bar Stress from a previously run model.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "Storey", "Structure", "Id", "Quality", "Section", "UnitWeight", "SubTotal", "PaintedArea", "TotalWeigth" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.QuantityEstimationGeneral> Result)
        {
            var storey = Result.Select(x => x.Storey);
            var structure = Result.Select(x => x.Structure);
            var id = Result.Select(x => x.Id);
            var quality = Result.Select(x => x.Quality);
            var section = Result.Select(x => x.Section);
            var unitWeight = Result.Select(x => x.UnitWeight);
            var subTotal = Result.Select(x => x.SubTotal);
            var paintedArea = Result.Select(x => x.PaintedArea);
            var totalWeight = Result.Select(x => x.TotalWeight);

            return new Dictionary<string, object>
            {
                {"Storey", storey},
                {"Structure", structure},
                {"Id", id},
                {"Quality", quality},
                {"Section", section},
                {"UnitWeight", unitWeight},
                {"Subtotal", subTotal},
                {"PaintedArea", paintedArea},
                {"TotalWeigth", totalWeight},
            };
        }
    }
}
