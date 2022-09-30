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
    public class ShellInternalForce : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ShellInternalForce()
          : base("ShellForce",
                "Shell Forces",
                "Read the shell forces for the entire model",
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
            pManager.Register_GenericParam("Mx", "Mx", "Bending Moment in the local X direction");
            pManager.Register_GenericParam("My", "My", "Bending Moment in the local Y direction");
            pManager.Register_GenericParam("Mxy", "Mxy", "Bending Moment in the local xy plane");
            pManager.Register_GenericParam("Nx", "Nx", "Normal Force in the local X direction");
            pManager.Register_GenericParam("Ny", "Ny", "Normal Force in the local Y direction");
            pManager.Register_GenericParam("Nxy", "Nxy", "Shear Force in the local xy plane");
            pManager.Register_GenericParam("Txz", "Txz", "Shear Force in the local xz plane");
            pManager.Register_GenericParam("Tyz", "Tyz", "Shear Force in the local xz plane");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.ShellInternalForce> iResult = new List<FemDesign.Results.ShellInternalForce>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.ShellInternalForce.DeconstructShellInternalForce(iResult, iLoadCase);
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];

            var identifier = (List<string>)result["Identifier"];
            var elementId = (List<int>)result["ElementId"];
            var nodeId = (List<int?>)result["NodeId"];

            var mx = (List<double>)result["Mx"];
            var my = (List<double>)result["My"];
            var mxy = (List<double>)result["Mxy"];
            var nx = (List<double>)result["Nx"];
            var ny = (List<double>)result["Ny"];
            var nxy = (List<double>)result["Nxy"];
            var txz = (List<double>)result["Txz"];
            var tyz = (List<double>)result["Tyz"];


            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            DataTree<object> elementIdTree = new DataTree<object>();
            DataTree<object> nodeIdTree = new DataTree<object>();
            DataTree<object> mxTree = new DataTree<object>();
            DataTree<object> myTree = new DataTree<object>();
            DataTree<object> mxyTree = new DataTree<object>();
            DataTree<object> nxTree = new DataTree<object>();
            DataTree<object> nyTree = new DataTree<object>();
            DataTree<object> nxyTree = new DataTree<object>();
            DataTree<object> txzTree = new DataTree<object>();
            DataTree<object> tyzTree = new DataTree<object>();


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
                    nodeIdTree.Add(nodeId.ElementAt(index), new GH_Path(ghPath, i));
                    mxTree.Add(mx.ElementAt(index), new GH_Path(ghPath, i));
                    myTree.Add(my.ElementAt(index), new GH_Path(ghPath, i));
                    mxyTree.Add(mxy.ElementAt(index), new GH_Path(ghPath, i));
                    nxTree.Add(nx.ElementAt(index), new GH_Path(ghPath, i));
                    nyTree.Add(ny.ElementAt(index), new GH_Path(ghPath, i));
                    nxyTree.Add(nxy.ElementAt(index), new GH_Path(ghPath, i));
                    txzTree.Add(txz.ElementAt(index), new GH_Path(ghPath, i));
                    tyzTree.Add(tyz.ElementAt(index), new GH_Path(ghPath, i));
                }
                i++;
            }

            DA.SetDataList("CaseIdentifier", uniqueLoadCase);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, nodeIdTree);
            DA.SetDataTree(3, mxTree);
            DA.SetDataTree(4, myTree);
            DA.SetDataTree(5, mxyTree);
            DA.SetDataTree(6, nxTree);
            DA.SetDataTree(7, nyTree);
            DA.SetDataTree(8, nxyTree);
            DA.SetDataTree(9, txzTree);
            DA.SetDataTree(10, tyzTree);
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
            get { return new Guid("BED6CA8B-7B6C-4CAE-B5D9-CD001BB8C01B"); }
        }
    }
}