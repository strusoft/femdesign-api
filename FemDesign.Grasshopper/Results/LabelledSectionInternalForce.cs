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
    public class LabelledSectionInternalForce : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public LabelledSectionInternalForce()
          : base("LabelledSectionInternalForce",
                "LabelledSectionInternalForce",
                "Read the Labelled Section Internal Force for the elements",
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
            pManager.Register_DoubleParam("Nx", "Nx", "");
            pManager.Register_DoubleParam("Ny", "Ny", "");
            pManager.Register_DoubleParam("Nxy", "Nxy", "");
            pManager.Register_DoubleParam("Mx", "Mx", "");
            pManager.Register_DoubleParam("My", "My", "");
            pManager.Register_DoubleParam("Mxy", "Mxy", "");
            pManager.Register_DoubleParam("Txz", "Txz", "");
            pManager.Register_DoubleParam("Tyz", "Tyz", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.LabelledSectionInternalForce> iResult = new List<FemDesign.Results.LabelledSectionInternalForce>();
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
            DataTree<object> positionResultTree = new DataTree<object>();
            DataTree<object> nxTree = new DataTree<object>();
            DataTree<object> nyTree = new DataTree<object>();
            DataTree<object> nxyTree = new DataTree<object>();
            DataTree<object> mxTree = new DataTree<object>();
            DataTree<object> myTree = new DataTree<object>();
            DataTree<object> mxyTree = new DataTree<object>();
            DataTree<object> txzTree = new DataTree<object>();
            DataTree<object> tyzTree = new DataTree<object>();

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

                    positionResultTree.Add(iResult.ElementAt(index).Pos, new GH_Path(ghPath, i));

                    nxTree.Add(iResult.ElementAt(index).Nx, new GH_Path(ghPath, i));
                    nyTree.Add(iResult.ElementAt(index).Ny, new GH_Path(ghPath, i));
                    nxyTree.Add(iResult.ElementAt(index).Nxy, new GH_Path(ghPath, i));
                    mxTree.Add(iResult.ElementAt(index).Mx, new GH_Path(ghPath, i));
                    myTree.Add(iResult.ElementAt(index).My, new GH_Path(ghPath, i));
                    mxyTree.Add(iResult.ElementAt(index).Mxy, new GH_Path(ghPath, i));
                    txzTree.Add(iResult.ElementAt(index).Txz, new GH_Path(ghPath, i));
                    tyzTree.Add(iResult.ElementAt(index).Tyz, new GH_Path(ghPath, i));
                }
                i++;
            }

            // Set output
            DA.SetData(0, iLoadCase);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, positionResultTree);
            DA.SetDataTree(3, nxTree);
            DA.SetDataTree(4, nyTree);
            DA.SetDataTree(5, nxyTree);
            DA.SetDataTree(6, mxTree);
            DA.SetDataTree(7, myTree);
            DA.SetDataTree(8, mxyTree);
            DA.SetDataTree(9, txzTree);
            DA.SetDataTree(10, tyzTree);
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
            get { return new Guid("{16E48449-08E4-422E-86E7-AF927967D7DC}"); }
        }
    }
}