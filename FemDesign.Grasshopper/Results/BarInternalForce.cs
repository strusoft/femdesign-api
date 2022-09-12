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
    public class BarInternalForce : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BarInternalForce()
          : base("BarInternalForce",
                "BarInternalForce",
                "Read the Bar InternalForces for the elements",
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
            pManager.Register_DoubleParam("Fx", "Fx", "Fx");
            pManager.Register_DoubleParam("Fy", "Fy", "Fy");
            pManager.Register_DoubleParam("Fz", "Fz", "Fz");
            pManager.Register_DoubleParam("Mx", "Mx", "mx");
            pManager.Register_DoubleParam("My", "My", "My");
            pManager.Register_DoubleParam("Mz", "Mz", "Mz");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.BarInternalForce> iResult = new List<FemDesign.Results.BarInternalForce>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.BarInternalForce.DeconstructBarInternalForce(iResult, iLoadCase);
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
            var fx = (List<double>)result["Fx"];
            var fy = (List<double>)result["Fy"];
            var fz = (List<double>)result["Fz"];
            var mx = (List<double>)result["Mx"];
            var my = (List<double>)result["My"];
            var mz = (List<double>)result["Mz"];

            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            DataTree<object> elementIdTree = new DataTree<object>();
            DataTree<object> positionResultTree = new DataTree<object>();
            DataTree<object> fxTree = new DataTree<object>();
            DataTree<object> fyTree = new DataTree<object>();
            DataTree<object> fzTree = new DataTree<object>();
            DataTree<object> mxTree = new DataTree<object>();
            DataTree<object> myTree = new DataTree<object>();
            DataTree<object> mzTree = new DataTree<object>();

            // Current Iteration Count
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
                    //loadCasesTree.Add(loadCases.ElementAt(index), new GH_Path(i));
                    positionResultTree.Add(positionResult.ElementAt(index), new GH_Path(ghPath, i));

                    fxTree.Add(fx.ElementAt(index), new GH_Path(ghPath, i));
                    fyTree.Add(fy.ElementAt(index), new GH_Path( ghPath, i));
                    fzTree.Add(fz.ElementAt(index), new GH_Path(ghPath, i));
                    mxTree.Add(mx.ElementAt(index), new GH_Path(ghPath, i));
                    myTree.Add(my.ElementAt(index), new GH_Path(ghPath, i));
                    mzTree.Add(mz.ElementAt(index), new GH_Path(ghPath, i));

                }
                i++;
            }

            // Set output
            DA.SetDataList(0, uniqueLoadCase);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, positionResultTree);
            DA.SetDataTree(3, fxTree);
            DA.SetDataTree(4, fyTree);
            DA.SetDataTree(5, fzTree);
            DA.SetDataTree(6, mxTree);
            DA.SetDataTree(7, myTree);
            DA.SetDataTree(8, mzTree);
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
            get { return new Guid("9F85287C-3C29-4D58-8E1F-2CEBC5D2379E"); }
        }
    }
}