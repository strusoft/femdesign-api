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
    public class LineConnectionForce : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the LineConnectionForce class.
        /// </summary>
        public LineConnectionForce()
          : base("LineConnectionForce",
                "LineConnectionForce",
                "Read the Line Connection Forces for the elements",
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
            pManager.Register_IntegerParam("ElementIndex", "ElementIndex", "");
            pManager.Register_IntegerParam("NodeId", "NodeId", "");
            pManager.Register_DoubleParam("Fx", "Fx", "");
            pManager.Register_DoubleParam("Fy", "Fy", "");
            pManager.Register_DoubleParam("Fz", "Fz", "");
            pManager.Register_DoubleParam("Mx", "Mx", "");
            pManager.Register_DoubleParam("My", "My", "");
            pManager.Register_DoubleParam("Mz", "Mz", "");
            pManager.Register_DoubleParam("Fr", "Fr", "");
            pManager.Register_DoubleParam("Mr", "Mr", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.LineConnectionForce> iResult = new List<FemDesign.Results.LineConnectionForce>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);


            // Return the unique load case - load combination
            var uniqueLoadCases = iResult.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(iLoadCase, StringComparer.OrdinalIgnoreCase))
            {
                iResult = iResult.Where(n => String.Equals(n.CaseIdentifier, iLoadCase, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                var warning = $"Load Case '{iLoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            var uniqueId = iResult.Select(x => x.Id).Distinct().ToList();
            var elementId = iResult.Select(x => x.Id).ToList();

            // Convert Data in DataTree structure
            DataTree<object> elementIdTree = new DataTree<object>();

            DataTree<object> elementNumberTree = new DataTree<object>();
            DataTree<object> nodeTree = new DataTree<object>();

            DataTree<object> fxTree = new DataTree<object>();
            DataTree<object> fyTree = new DataTree<object>();
            DataTree<object> fzTree = new DataTree<object>();

            DataTree<object> mxTree = new DataTree<object>();
            DataTree<object> myTree = new DataTree<object>();
            DataTree<object> mzTree = new DataTree<object>();

            DataTree<object> frTree = new DataTree<object>();
            DataTree<object> mrTree = new DataTree<object>();

            // Current Iteration Count
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
                    elementNumberTree.Add(iResult.ElementAt(index).ElementId, new GH_Path(ghPath, i));
                    nodeTree.Add(iResult.ElementAt(index).NodeId, new GH_Path(ghPath, i));


                    fxTree.Add(iResult.ElementAt(index).Fx, new GH_Path(ghPath, i));
                    fyTree.Add(iResult.ElementAt(index).Fy, new GH_Path(ghPath, i));
                    fzTree.Add(iResult.ElementAt(index).Fz, new GH_Path(ghPath, i));

                    mxTree.Add(iResult.ElementAt(index).Mx, new GH_Path(ghPath, i));
                    myTree.Add(iResult.ElementAt(index).My, new GH_Path(ghPath, i));
                    mzTree.Add(iResult.ElementAt(index).Mz, new GH_Path(ghPath, i));

                    frTree.Add(iResult.ElementAt(index).Mz, new GH_Path(ghPath, i));
                    mrTree.Add(iResult.ElementAt(index).Mz, new GH_Path(ghPath, i));
                }
                i++;
            }

            // Set output
            DA.SetData(0, iLoadCase);
            DA.SetDataTree(1, elementIdTree);

            DA.SetDataTree(2, elementNumberTree);
            DA.SetDataTree(3, nodeTree);

            DA.SetDataTree(4, fxTree);
            DA.SetDataTree(5, fyTree);
            DA.SetDataTree(6, fzTree);

            DA.SetDataTree(7, mxTree);
            DA.SetDataTree(8, myTree);
            DA.SetDataTree(9, mzTree);

            DA.SetDataTree(10, frTree);
            DA.SetDataTree(11, mrTree);
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
                return FemDesign.Properties.Resources.Results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{90167A24-E068-4E92-BA0D-A2B0C9DE3441}"); }
        }
    }
}