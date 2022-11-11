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
    public class SurfaceSupportReaction : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PointSupportReaction class.
        /// </summary>
        public SurfaceSupportReaction()
          : base("SurfaceSupportReaction",
                "SurfaceSupportReaction",
                "Read the Surface Support Reaction",
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
            pManager.Register_GenericParam("Name", "Name", "Element Name");
            pManager.Register_GenericParam("ElementId", "ElementId", "Element Index");
            pManager.Register_GenericParam("NodeId", "NodeId", "Node Index");
            pManager.AddVectorParameter("ReactionForce", "ReactionForce", "Reaction Forces in global x, y, z for all nodes.", GH_ParamAccess.list);
            pManager.AddNumberParameter("ForceResultant", "ForceResultant", "Force Resultant", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.SurfaceSupportReaction> iResult = new List<FemDesign.Results.SurfaceSupportReaction>();
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


            // Extract Results from the Dictionary
            var loadCases = new List<string>();
            var name = new List<string>();
            var elementId = new List<int>();
            var nodeId = new List<int?>();
            var iForce = new List<Vector3d>();
            var resultant = new List<double>();

            foreach (var result in iResult)
            {
                loadCases.Add(result.CaseIdentifier);
                name.Add(result.Name);
                elementId.Add(result.ElementId);
                nodeId.Add(result.NodeId);
                iForce.Add(new Vector3d(result.Fx, result.Fy, result.Fz));
                resultant.Add(result.Fr);
            }


            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            DataTree<object> nameTree = new DataTree<object>();
            DataTree<object> elementIdTree = new DataTree<object>();
            DataTree<object> nodeIdTree = new DataTree<object>();
            DataTree<object> oForceTree = new DataTree<object>();
            DataTree<object> oResultant = new DataTree<object>();


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
                    nameTree.Add(name.ElementAt(index), new GH_Path(ghPath, i));
                    nodeIdTree.Add(nodeId.ElementAt(index), new GH_Path(ghPath, i));
                    oForceTree.Add(iForce.ElementAt(index), new GH_Path(ghPath, i));
                    oResultant.Add(resultant.ElementAt(index), new GH_Path(ghPath, i));
                }
                i++;
            }

            DA.SetDataList("CaseIdentifier", uniqueLoadCase);
            DA.SetDataTree(1, nameTree);
            DA.SetDataTree(2, elementIdTree);
            DA.SetDataTree(3, nodeIdTree);
            DA.SetDataTree(4, oForceTree);
            DA.SetDataTree(5, oResultant);
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
            get { return new Guid("{21DAE46D-101C-4D04-8CFC-6154105B30DB}"); }
        }
    }
}