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
    public class labelledSectionResultant : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public labelledSectionResultant()
          : base("LabelledSectionResultant",
                "LabelledSectionResultant",
                "Read the Labelled Section Resultant Force for the elements",
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
            pManager.Register_DoubleParam("BasePointFromSP.", "BasePointFromSP.", "");
            pManager.Register_DoubleParam("Fx", "Fx", "");
            pManager.Register_DoubleParam("Fy", "Fy", "");
            pManager.Register_DoubleParam("Fz", "Fz", "");
            pManager.Register_DoubleParam("Mx", "Mx", "");
            pManager.Register_DoubleParam("My", "My", "");
            pManager.Register_DoubleParam("Mz", "Mz", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.LabelledSectionResultant> iResult = new List<FemDesign.Results.LabelledSectionResultant>();
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
            DataTree<object> nzTree = new DataTree<object>();
            DataTree<object> mxTree = new DataTree<object>();
            DataTree<object> myTree = new DataTree<object>();
            DataTree<object> mzTree = new DataTree<object>();

            // Current Iteration Count
            var ghPath = DA.Iteration;
            var i = 0;

            foreach (var id in uniqueId)
            {
                // indexes where the uniqueId matches in the list
                var indexes = elementId.Select((value, index) => new { value, index })
                  .Where(a => string.Equals(a.value, id))
                  .Select(a => a.index);

                foreach (int index in indexes)
                {
                    //loadCasesTree.Add(loadCases.ElementAt(index), new GH_Path(i));
                    elementIdTree.Add(iResult.ElementAt(index).Id, new GH_Path(ghPath, i));
                    positionResultTree.Add(iResult.ElementAt(index).BasePointFromSP, new GH_Path(ghPath, i));

                    nxTree.Add(iResult.ElementAt(index).Fx, new GH_Path(ghPath, i));
                    nyTree.Add(iResult.ElementAt(index).Fy, new GH_Path(ghPath, i));
                    nzTree.Add(iResult.ElementAt(index).Fz, new GH_Path(ghPath, i));
                    mxTree.Add(iResult.ElementAt(index).Mx, new GH_Path(ghPath, i));
                    myTree.Add(iResult.ElementAt(index).My, new GH_Path(ghPath, i));
                    mzTree.Add(iResult.ElementAt(index).Mz, new GH_Path(ghPath, i));
                }
                i++;
            }

            // Set output
            DA.SetData(0, iLoadCase);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, positionResultTree);
            DA.SetDataTree(3, nxTree);
            DA.SetDataTree(4, nyTree);
            DA.SetDataTree(5, nzTree);
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
            get { return new Guid("{7F6B3886-0731-47D8-A1E6-957F0E15040D}"); }
        }
    }
}