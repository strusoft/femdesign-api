using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class ShellDisplacement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ShellDisplacement()
          : base("ShellDisplacement",
                "ShellDisplacement",
                "Read the shell displacement for the entire model",
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
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
            pManager.Register_GenericParam("ElementId", "ElementId", "Element Index");
            pManager.Register_GenericParam("NodeId", "NodeId", "Node Index");
            pManager.Register_VectorParam("Translation", "Translation", "Nodal translations in global x, y, z for all nodes.");
            pManager.Register_VectorParam("Rotation", "Rotation", "Nodal rotations in global x, y, z for all nodes.");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.ShellDisplacement> iResult = new List<FemDesign.Results.ShellDisplacement>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.ShellDisplacement.DeconstructShellDisplacement(iResult, iLoadCase);
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];
            var elementId = (List<int>)result["ElementId"];
            var nodeId = (List<int?>)result["NodeId"];
            var iTranslation = (List<FemDesign.Geometry.Vector3d>)result["Translation"];
            var iRotation = (List<FemDesign.Geometry.Vector3d>)result["Rotation"];


            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            DataTree<object> elementIdTree = new DataTree<object>();
            DataTree<object> nodeIdTree = new DataTree<object>();
            DataTree<object> oTranslationTree = new DataTree<object>();
            DataTree<object> oRotationTree = new DataTree<object>();


            var ghPath = DA.Iteration;
            var i = 0;

            foreach (var id in uniqueId)
            {
                // indexes where the uniqueId matches in the list
                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                elementIdTree.Add(id, new GH_Path(ghPath, i));

                foreach (int index in indexes)
                {
                    //loadCasesTree.Add(loadCases.ElementAt(index), new GH_Path(i));
                    //elementIdTree.Add(elementId.ElementAt(index), new GH_Path(ghPath, i));
                    nodeIdTree.Add(nodeId.ElementAt(index), new GH_Path(ghPath, i));
                    oTranslationTree.Add(iTranslation.ElementAt(index).ToRhino(), new GH_Path(ghPath, i));
                    oRotationTree.Add(iRotation.ElementAt(index).ToRhino(), new GH_Path(ghPath, i));
                }
                i++;
            }

            DA.SetDataList("CaseIdentifier", uniqueLoadCase);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, nodeIdTree);
            DA.SetDataTree(3, oTranslationTree);
            DA.SetDataTree(4, oRotationTree);
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

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
            get { return new Guid("DD977169-C380-4DBE-8F39-87448F42BD8D"); }
        }
    }
}