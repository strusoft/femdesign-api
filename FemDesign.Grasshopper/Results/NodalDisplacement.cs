using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class NodalDisplacement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public NodalDisplacement()
          : base("NodalDisplacement",
                "NodalDisplacement",
                "Read the nodal displacement for the entire model",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddTextParameter("Case/Combination Name", "Case/Comb Name", "Name of Load Case/Load Combination for which to return the results.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("NodeId", "NodeId", "Node Index", GH_ParamAccess.list);
            pManager.AddVectorParameter("Translation", "Translation", "Nodal translations in global x, y, z for all nodes.", GH_ParamAccess.list);
            pManager.AddVectorParameter("Rotation", "Rotation", "Nodal rotations in global x, y, z for all nodes.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.NodalDisplacement> iResult = new List<FemDesign.Results.NodalDisplacement>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);


            // Read Result from Abstract Method
            Dictionary<string, object> result;


            try
            {
                result = FemDesign.Results.NodalDisplacement.DeconstructNodalDisplacements(iResult, iLoadCase);
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage( GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }


            var loadCases = (List<string>) result ["CaseIdentifier"];
            var nodeId = (List<int>) result["NodeId"];
            var iTranslation = (List<FemDesign.Geometry.Vector3d>) result["Translation"];
            var iRotation = (List<FemDesign.Geometry.Vector3d>) result["Rotation"];

            // Convert the FdVector to Dynamo
            var oTranslation = iTranslation.Select(x => x.ToRhino());
            var oRotation = iRotation.Select(x => x.ToRhino());



            // Set output
            DA.SetDataList("CaseIdentifier", loadCases);
            DA.SetDataList("NodeId", nodeId);
            DA.SetDataList("Translation", oTranslation);
            DA.SetDataList("Rotation", oRotation);
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

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
            get { return new Guid("4A4FD737-4510-4C00-893E-3DB6814A8F68"); }
        }
    }
}