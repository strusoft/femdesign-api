using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Runtime;


namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class EigenFrequencies : IResult
    {
        [IsVisibleInDynamoLibrary(true)]
        public static string ResultType()
        {
            return "EigenFrequencies";
        }

        /// <summary>
        /// Read the EigenFrequencies for the entire model
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "ShapeId", "Frequency", "Period", "ModalMass", "MassParticipantXi", "MassParticipantYi", "MassParticipantZi" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.EigenFrequencies> Result)
        {

            var shapeId = Result.Select(x => x.ShapeId).ToList();
            var frequency = Result.Select(x => x.Frequency).ToList();
            var period = Result.Select(x => x.Period).ToList();
            var modalMass = Result.Select(x => x.ModalMass).ToList();
            var massParticipantXi = Result.Select(x => x.MassParticipantXi).ToList();
            var massParticipantYi = Result.Select(x => x.MassParticipantYi).ToList();
            var massParticipantZi = Result.Select(x => x.MassParticipantZi).ToList();


            return new Dictionary<string, object>
            {
                {"ShapeId", shapeId },
                {"Frequency", frequency },
                {"Period", period },
                {"ModalMass", modalMass },
                {"MassParticipantXi", massParticipantXi },
                {"MassParticipantYi", massParticipantYi },
                {"MassParticipantZi", massParticipantZi },
            };
        }
    }
}
