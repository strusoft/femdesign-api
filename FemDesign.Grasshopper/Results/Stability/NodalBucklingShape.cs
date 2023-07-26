using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class NodalBucklingShape : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NodalBucklingShape class.
        /// </summary>
        public NodalBucklingShape() : base("NodalBucklingShape", "NodalBucklingShape", "Read the Nodal buckling shape values for the entire model", CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddTextParameter("Combination Name", "CombName", "Name of Load Combination for which to return the results.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Buckling Shape", "Shape", "Buckling shape indexes start from '1' as per FEM-Design", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("Identifier", "Id", "Element identifier.");
            pManager.Register_StringParam("CaseIdentifier", "CaseId", "Case identifier.");
            pManager.Register_IntegerParam("ShapeId", "ShapeId", "Shape index");
            pManager.Register_IntegerParam("NodeId", "NodeId", "Node index");
            pManager.Register_VectorParam("Translation", "Translation", "Nodal translations in global x, y, z for all nodes");
            pManager.Register_VectorParam("Rotation", "Rotation", "Nodal rotations in global x, y, z for all nodes.");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.NodalBucklingShape> iResult = new List<FemDesign.Results.NodalBucklingShape>();
            DA.GetDataList(0, iResult);

            string combName = null;
            DA.GetData(1, ref combName);

            int? bucklingShape = null;
            DA.GetData(2, ref bucklingShape);
           

            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                // The method "DeconstructNodalBuckling" required a string option as it has been
                // replicated from other Deconstructor methods
                result = DeconstructNodalBucklingShape(iResult, combName, bucklingShape);
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }

            var identifier = (List<string>)result["Id"];
            var caseId = (List<string>)result["CaseId"];
            var shapeIds = (List<int>)result["ShapeId"];
            var nodeId = (List<int>)result["NodeId"];
            var iTranslation = (List<FemDesign.Geometry.Vector3d>)result["Translation"];
            var iRotation = (List<FemDesign.Geometry.Vector3d>)result["Rotation"];

            // Convert the FdVector to Rhino
            var oTranslation = iTranslation.Select(x => x.ToRhino());
            var oRotation = iRotation.Select(x => x.ToRhino());


            // Set output
            DA.SetDataList(0, identifier);
            DA.SetDataList(1, caseId);
            DA.SetDataList(2, shapeIds);
            DA.SetDataList(3, nodeId);
            DA.SetDataList(4, oTranslation);
            DA.SetDataList(5, oRotation);
        }

        public static Dictionary<string, object> DeconstructNodalBucklingShape(List<FemDesign.Results.NodalBucklingShape> bucklingResult, string caseIdentifier, int? modeShapeId)
        {
            var nodalBucklingShape = bucklingResult.Cast<FemDesign.Results.NodalBucklingShape>();

            // Return the unique load combinations
            var uniqueCaseId = nodalBucklingShape.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Return the unique shape identifiers
            var uniqueShapeId = nodalBucklingShape.Select(n => n.Shape).Distinct().ToList();

            // Select the Nodal Buckling shapes for the selected Load Combination
            if (caseIdentifier != null)
            {
                if (uniqueCaseId.Contains(caseIdentifier, StringComparer.OrdinalIgnoreCase))
                {
                    nodalBucklingShape = nodalBucklingShape.Where(n => String.Equals(n.CaseIdentifier, caseIdentifier, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    var warning = $"NodalBucklingShape result for load combination '{caseIdentifier}' does not exist";
                    throw new ArgumentException(warning);
                }
            }

            // Select the Nodal Buckling shapes for the selected Shape identifier
            if (modeShapeId != null)
            {
                if (uniqueShapeId.Any(u => u == modeShapeId))
                {
                    nodalBucklingShape = nodalBucklingShape.Where(n => n.Shape == modeShapeId);
                }
                else
                {
                    throw new System.ArgumentException($"Shape index {modeShapeId} is unknown or out of range.");
                }
            }

            // Parse Results from the object
            var identifier = nodalBucklingShape.Select(n => n.Id).ToList();
            var caseId = nodalBucklingShape.Select(n => n.CaseIdentifier).ToList();
            var nodeId = nodalBucklingShape.Select(n => n.NodeId).ToList();
            var shapeIds = nodalBucklingShape.Select(n => n.Shape).ToList();

            // Create a Vector for Displacement and Rotation
            var translation = new List<FemDesign.Geometry.Vector3d>();
            var rotation = new List<FemDesign.Geometry.Vector3d>();

            foreach (var nodeBuckling in nodalBucklingShape)
            {
                var transVector = new FemDesign.Geometry.Vector3d(nodeBuckling.Ex, nodeBuckling.Ey, nodeBuckling.Ez);
                translation.Add(transVector);

                var rotVector = new FemDesign.Geometry.Vector3d(nodeBuckling.Fix, nodeBuckling.Fiy, nodeBuckling.Fiz);
                rotation.Add(rotVector);
            }


            return new Dictionary<string, dynamic>
            {
                {"Id", identifier},
                {"CaseId", caseId},
                {"ShapeId", shapeIds},
                {"NodeId", nodeId},
                {"Translation", translation},
                {"Rotation", rotation},
            };
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.Results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("121F493D-432F-4E2A-B0CF-B19543FBAA10"); }
        }
    }
}