//using System;
//using System.Collections.Generic;
//using Grasshopper;
//using Grasshopper.Kernel;
//using System.Linq;
//using Rhino.Geometry;
//using FemDesign.Results;

//namespace FemDesign.Grasshopper
//{
//    public class NodalBucklingShape : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the NodalBucklingShape class.
//        /// </summary>
//        public NodalBucklingShape() : base("NodalBucklingShape","NodalBucklingShape","Read the Nodal buckling shape vector for the entire model",CategoryName.Name(), SubCategoryName.Cat7b())
//        {

//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
//            pManager.AddTextParameter("Combination Name", "CombName", "Name of Load Combination for which to return the results.", GH_ParamAccess.item);
//            pManager.AddIntegerParameter("Buckling Mode", "BucklingMode", "Buckling Mode indexs start from '1' as per FEM-Design", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.Register_StringParam("CaseIdentifier", "CaseID", "Case identifier");
//            pManager.Register_StringParam("ShapeId", "ShapeId", "Shape index");
//            pManager.Register_IntegerParam("NodeId", "NodeId", "Node index");
//            pManager.Register_VectorParam("Translation", "Translation", "Nodal translations in global x, y, z for all nodes");
//            pManager.Register_VectorParam("Rotation", "Rotation", "Nodal rotations in global x, y, z for all nodes.");
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            // get indata

//            List<FemDesign.Results.NodalBucklingShape> iResult = new List<FemDesign.Results.NodalBucklingShape>();
//            DA.GetDataList(0, iResult);

//            string caseName = null;
//            DA.GetData(1, ref caseName);

//            int bucklingMode = 0;
//            DA.GetData(2, ref bucklingMode);


//            // Read Result from Abstract Method
//            Dictionary<string, object> result;


//            try
//            {
//                // The method "DeconstructNodalBuckling" required a string option as it has been
//                // replicated from other Deconstructor methods
//                result = FemDesign.Results.NodalBucklingShape.DeconstructNodalBucklingShape(iResult, caseName, bucklingMode.ToString());
//            }
//            catch (ArgumentException ex)
//            {
//                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
//                return;
//            }

//            var identifier = (List<string>)result["Id"];
//            var shapeIds = (List<int>)result["ShapeId"];
//            var nodeId = (List<int>)result["NodeId"];
//            var iTranslation = (List<FemDesign.Geometry.Vector3d>)result["Translation"];
//            var iRotation = (List<FemDesign.Geometry.Vector3d>)result["Rotation"];

//            // Convert the FdVector to Rhino
//            var oTranslation = iTranslation.Select(x => x.ToRhino());
//            var oRotation = iRotation.Select(x => x.ToRhino());



//            // Set output
//            DA.SetDataList(0, identifier);
//            DA.SetDataList(1, shapeIds);
//            DA.SetDataList(2, nodeId);
//            DA.SetDataList(3, oTranslation);
//            DA.SetDataList(4, oRotation);
//        }

//        public static Dictionary<string, object> DeconstructNodalBucklingShape(List<FemDesign.Results.NodalBucklingShape> Result, string CaseIdentifier, string ModeShapeId)
//        {
//            var nodalBucklingShape = Result.Cast<FemDesign.Results.NodalBucklingShape>();

//            // Return the unique load cases - load combinations
//            var uniqueShapeId = nodalBucklingShape.Select(n => n.ShapeID.ToString()).Distinct().ToList();

//            // Select the Nodal Buckling shapes for the selected Load Case - Load Combination
//            if (uniqueShapeId.Contains(ModeShapeId, StringComparer.OrdinalIgnoreCase))
//            {
//                nodalBucklingShape = nodalBucklingShape.Where(n => String.Equals(n.ShapeID.ToString(), ModeShapeId, StringComparison.OrdinalIgnoreCase));
//            }
//            else
//            {
//                var warning = $"Shape Mode '{ModeShapeId}' does not exist";
//                throw new ArgumentException(warning);
//            }

//            // Parse Results from the object
//            var identifier = nodalDisplacements.Select(n => n.Id).ToList();
//            var nodeId = nodalDisplacements.Select(n => n.NodeId).ToList();
//            var shapeIds = nodalDisplacements.Select(n => n.ShapeId).Distinct().ToList();

//            // Create a Rhino Vector for Displacement and Rotation
//            var translation = new List<FemDesign.Geometry.Vector3d>();
//            var rotation = new List<FemDesign.Geometry.Vector3d>();

//            foreach (var nodeDisp in nodalDisplacements)
//            {
//                var transVector = new FemDesign.Geometry.Vector3d(nodeDisp.Ex, nodeDisp.Ey, nodeDisp.Ez);
//                translation.Add(transVector);

//                var rotVector = new FemDesign.Geometry.Vector3d(nodeDisp.Fix, nodeDisp.Fiy, nodeDisp.Fiz);
//                rotation.Add(rotVector);
//            }


//            return new Dictionary<string, dynamic>
//            {
//                {"ShapeId", shapeIds},
//                {"Id", identifier},
//                {"NodeId", nodeId},
//                {"Translation", translation},
//                {"Rotation", rotation},
//            };
//        }

//        public override GH_Exposure Exposure => GH_Exposure.senary;

//        /// <summary>
//        /// Provides an Icon for the component.
//        /// </summary>
//        protected override System.Drawing.Bitmap Icon
//        {
//            get
//            {
//                //You can add image files to your project resources and access them like this:
//                // return Resources.IconForThisComponent;
//                return FemDesign.Properties.Resources.Results;
//            }
//        }

//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid
//        {
//            get { return new Guid("19237BFD-C55E-4327-A390-EF97C9FE6319"); }
//        }
//    }
//}