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
    public class BarStress : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BarStress()
          : base("BarStress",
                "BarStress",
                "Read the Bar Stress for the elements",
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
            pManager.Register_DoubleParam("SigmaXiMax", "SigmaXiMax", "SigmaXiMax");
            pManager.Register_DoubleParam("SigmaXiMin", "SigmaXiMin", "SigmaXiMin");
            pManager.Register_DoubleParam("SigmaVM", "SigmaVM", "SigmaVM");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.BarStress> iResult = new List<FemDesign.Results.BarStress>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.BarStress.DeconstructBarStress(iResult, iLoadCase);
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
            var fx = (List<double>)result["SigmaXiMax"];
            var fy = (List<double>)result["SigmaXiMin"];
            var fz = (List<double>)result["SigmaVM"];

            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            DataTree<object> elementIdTree = new DataTree<object>();
            DataTree<object> positionResultTree = new DataTree<object>();
            DataTree<object> sigmaXiMaxTree = new DataTree<object>();
            DataTree<object> sigmaXiMinTree = new DataTree<object>();
            DataTree<object> sigmaVMTree = new DataTree<object>();

            // Current Iteration Count
            var ghPath = DA.Iteration;
            var i = 0;

            foreach (var id in uniqueId)
            {
                elementIdTree.Add(id, new GH_Path(ghPath, i));
                // indexes where the uniqueId matches in the list
                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                foreach (int index in indexes)
                {
                    //loadCasesTree.Add(loadCases.ElementAt(index), new GH_Path(i));
                    
                    positionResultTree.Add(positionResult.ElementAt(index), new GH_Path(ghPath, i));

                    sigmaXiMaxTree.Add(fx.ElementAt(index), new GH_Path(ghPath, i));
                    sigmaXiMinTree.Add(fy.ElementAt(index), new GH_Path(ghPath, i));
                    sigmaVMTree.Add(fz.ElementAt(index), new GH_Path(ghPath, i));
                }
                i++;
            }

            // Set output
            DA.SetDataList(0, uniqueLoadCase);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, positionResultTree);
            DA.SetDataTree(3, sigmaXiMaxTree);
            DA.SetDataTree(4, sigmaXiMinTree);
            DA.SetDataTree(5, sigmaVMTree);
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
            get { return new Guid("19E95AE9-F1F4-47D1-8ADF-7A71B6B4AB2F"); }
        }
    }
}