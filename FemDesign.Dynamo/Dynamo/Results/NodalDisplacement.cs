using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Results
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class NodalDisplacement : IResult
    {
        /// <summary>
        /// Create new model. Add entities to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <param name="Result">Result to be Parse</param>
        /// <param name="LoadCase">Name of Load Case for which to return the results. Default value returns the displacement for the first load case</param>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "CaseIdentifier", "NodeId", "Translation", "Rotation" })]
        public static Dictionary<string, object> Deconstruct(List<FemDesign.Results.NodalDisplacement> Result, [DefaultArgument("null")] string LoadCase)
        {
            var nodalDisplacement = Result.Cast<FemDesign.Results.NodalDisplacement>();


            // Return the unique load case - load combination
            var uniqueLoadCases = nodalDisplacement.Select(n => n.CaseIdentifier).Distinct().ToList();


            // Select a Default load case if the user does not provide an input
            LoadCase = LoadCase == null ? uniqueLoadCases.First() : LoadCase;


            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(LoadCase, StringComparer.OrdinalIgnoreCase))
            {
                nodalDisplacement = nodalDisplacement.Where(n => String.Equals(n.CaseIdentifier, LoadCase, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                string msg = string.Format("Load Case '{0}' does not exist", LoadCase);
                throw new Exception(msg);
            }


            // Parse Results from the object
            var nodeId = nodalDisplacement.Select(n => n.NodeId);
            var loadCases = nodalDisplacement.Select(n => n.CaseIdentifier).Distinct().ToList();
            var transX = nodalDisplacement.Select(n => n.Ex);
            var transY = nodalDisplacement.Select(n => n.Ey);
            var transZ = nodalDisplacement.Select(n => n.Ez);
            var rotationX = nodalDisplacement.Select(n => n.Fix);
            var rotationY = nodalDisplacement.Select(n => n.Fiy);
            var rotationZ = nodalDisplacement.Select(n => n.Fiz);


            // Create a Rhino Vector for Displacement and Rotation
            var translation = new List<Autodesk.DesignScript.Geometry.Vector>();
            var rotation = new List<Autodesk.DesignScript.Geometry.Vector>();

            for (int i = 0; i < nodalDisplacement.Count(); i++)
            {
                var transVector = new FemDesign.Geometry.FdVector3d(transX.ElementAt(i), transY.ElementAt(i), transZ.ElementAt(i));
                translation.Add(transVector.ToDynamo());

                var rotVector = new FemDesign.Geometry.FdVector3d(rotationX.ElementAt(i), rotationY.ElementAt(i), rotationZ.ElementAt(i));
                rotation.Add(rotVector.ToDynamo());
            }


            var CaseIdentifier = loadCases;
            var NodeId = nodeId;
            var Translation = translation;
            var Rotation = rotation;

            return new Dictionary<string, object>
            {
                {"CaseIdentifier", CaseIdentifier},
                {"NodeId", nodeId},
                {"Translation", Translation},
                {"Rotation", Rotation}
            };
        }
    }
}
