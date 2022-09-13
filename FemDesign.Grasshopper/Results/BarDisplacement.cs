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
    public class BarDisplacement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BarDisplacement()
          : base("BarDisplacement",
                "BarDisplacement",
                "Read the bar displacement for the elements",
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
            pManager.Register_StringParam("ElementId", "ElementId", "Element Id");
            pManager.Register_DoubleParam("PositionResult", "PositionResult", "Position Result");
            pManager.Register_VectorParam("Translation", "Translation", "Element translations in local x, y, z for all nodes.");
            pManager.Register_VectorParam("Rotation", "Rotation", "Element rotations in local x, y, z for all nodes.");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.BarDisplacement> iResult = new List<FemDesign.Results.BarDisplacement>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);


            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.BarDisplacement.DeconstructBarDisplacements(iResult, iLoadCase);
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];
            var elementId = (List<string>)result["ElementId"];
            var positionResult = (List<double>)result["PositionResult"];
            var iTranslation = (List<FemDesign.Geometry.Vector3d>)result["Translation"];
            var iRotation = (List<FemDesign.Geometry.Vector3d>)result["Rotation"];


            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            DataTree<object> elementIdTree = new DataTree<object>();
            DataTree<object> positionResultTree = new DataTree<object>();
            DataTree<object> oTranslationTree = new DataTree<object>();
            DataTree<object> oRotationTree = new DataTree<object>();


            var ghPath = DA.Iteration;
            var i = 0;

            foreach (var id in uniqueId)
            {
                // indexes where the uniqueId matches in the list
                elementIdTree.Add(id, new GH_Path(ghPath, i));

                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                foreach (int index in indexes)
                {
                    positionResultTree.Add(positionResult.ElementAt(index), new GH_Path(ghPath, i));
                    oTranslationTree.Add(iTranslation.ElementAt(index).ToRhino(), new GH_Path(ghPath, i));
                    oRotationTree.Add(iRotation.ElementAt(index).ToRhino(), new GH_Path(ghPath, i));
                }
                i++;
            }

            // Set output
            DA.SetDataList(0, loadCases);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, positionResultTree);
            DA.SetDataTree(3, oTranslationTree);
            DA.SetDataTree(4, oRotationTree);
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

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
            get { return new Guid("A30A0D60-ECBE-4B34-8B38-0F95E9Fbbbbb"); }
        }
    }
}