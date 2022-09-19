using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class NodalVibration : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public NodalVibration()
          : base("NodalVibration",
                "NodalVibration",
                "Read the Nodal Vibration Vector for the entire model",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Vibration Mode", "VibrationMode", "Vibration Mode indexs start from '1' as per FEM-Design", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("ShapeId", "ShapeId", "ShapeId");
            pManager.Register_StringParam("Identifier", "Identifier", "Identifier");
            pManager.Register_IntegerParam("NodeId", "NodeId", "Node Index");
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

            List<FemDesign.Results.NodalVibration> iResult = new List<FemDesign.Results.NodalVibration>();
            DA.GetDataList("Result", iResult);

            int vibrationMode = 0;
            DA.GetData(1, ref vibrationMode);


            // Read Result from Abstract Method
            Dictionary<string, object> result;


            try
            {
                // The method "DeconstructNodalVibration" required a string option as it has been
                // replicated from other Deconstructor methods
                result = FemDesign.Results.NodalVibration.DeconstructNodalVibration(iResult, vibrationMode.ToString());
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }

            var shapeIds = (List<int>)result["ShapeId"];
            var identifier = (List<string>)result["Id"];
            var nodeId = (List<int>)result["NodeId"];
            var iTranslation = (List<FemDesign.Geometry.Vector3d>)result["Translation"];
            var iRotation = (List<FemDesign.Geometry.Vector3d>)result["Rotation"];

            // Convert the FdVector to Dynamo
            var oTranslation = iTranslation.Select(x => x.ToRhino());
            var oRotation = iRotation.Select(x => x.ToRhino());



            // Set output
            DA.SetDataList("ShapeId", shapeIds);
            DA.SetDataList("Identifier", identifier);
            DA.SetDataList("NodeId", nodeId);
            DA.SetDataList("Translation", oTranslation);
            DA.SetDataList("Rotation", oRotation);
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
                return FemDesign.Properties.Resources.results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E9B4B4AF-5895-47E2-8DCF-6EDF939B1F22"); }
        }
    }
}