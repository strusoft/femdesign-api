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
    public class ShellStress : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ShellStress()
          : base("ShellStress",
                "ShellStress",
                "Read the shell stresses for the entire model",
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
            pManager.AddTextParameter("Side", "Side", "Accepted values are top, bottom or membrane", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
            pManager.Register_GenericParam("ElementId", "ElementId", "Element Index");
            pManager.Register_GenericParam("NodeId", "NodeId", "Node Index");
            pManager.Register_GenericParam("SigmaX", "SigmaX", "Normal Stress in the local X direction");
            pManager.Register_GenericParam("SigmaY", "SigmaY", "Normal Stress in the local Y direction");
            pManager.Register_GenericParam("TauXY", "TauXY", "Tangential Stress in XY plane");
            pManager.Register_GenericParam("TauXZ", "TauXZ", "Tangential Stress in XZ plane");
            pManager.Register_GenericParam("TauYZ", "TauYZ", "Tangential Stress in YZ plane");
            pManager.Register_GenericParam("Sigma1", "Sigma1", "Principal Stress Value - First direction");
            pManager.Register_GenericParam("Sigma2", "Sigma2", "Principal Stress Value - Second direction");
            pManager.Register_GenericParam("SigmaVM", "SigmaVM", "VonMises Stress");
            pManager.Register_GenericParam("Alpha", "Alpha", "Angle between the local X axis and the direction");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.ShellStress> iResult = new List<FemDesign.Results.ShellStress>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            string side = null;
            DA.GetData(2, ref side);
            var sideOption= new List<string> { "top", "membrane", "bottom"};

            if (!sideOption.Contains(side))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Side Input must be 'top', 'membrane', 'bottom'");
                return;
            }

            // Select the results only at the specific Side
            iResult = iResult.Where(x => x.Side == side).ToList();

            // Read Result from Abstract Method
            Dictionary<string, object> result;

            try
            {
                result = FemDesign.Results.ShellStress.DeconstructShellStress(iResult, iLoadCase);
            }
            catch (ArgumentException ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                return;
            }

            // Extract Results from the Dictionary
            var loadCases = (List<string>)result["CaseIdentifier"];

            var identifier = (List<string>)result["CaseIdentifier"];
            var elementId = (List<int>)result["ElementId"];
            var nodeId = (List<int?>)result["NodeId"];

            var sigmaX = (List<double>)result["SigmaX"];
            var sigmaY = (List<double>)result["SigmaY"];
            var tauXY = (List<double>)result["TauXY"];
            var tauXZ = (List<double>)result["TauXZ"];
            var tauYZ = (List<double>)result["TauYZ"];
            var sigmaVM = (List<double>)result["SigmaVM"];
            var sigma1 = (List<double>)result["Sigma1"];
            var sigma2 = (List<double>)result["Sigma2"];
            var alpha = (List<double>)result["Alpha"];


            var uniqueLoadCase = loadCases.Distinct().ToList();
            var uniqueId = elementId.Distinct().ToList();


            // Convert Data in DataTree structure
            DataTree<object> elementIdTree = new DataTree<object>();
            DataTree<object> nodeIdTree = new DataTree<object>();
            DataTree<object> sigmaXTree = new DataTree<object>();
            DataTree<object> sigmaYTree = new DataTree<object>();
            DataTree<object> tauXYTree = new DataTree<object>();
            DataTree<object> tauXZTree = new DataTree<object>();
            DataTree<object> tauYZTree = new DataTree<object>();
            DataTree<object> sigmaVMTree = new DataTree<object>();
            DataTree<object> sigma1Tree = new DataTree<object>();
            DataTree<object> sigma2Tree = new DataTree<object>();
            DataTree<object> alphaTree = new DataTree<object>();


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
                    sigmaXTree.Add(sigmaX.ElementAt(index), new GH_Path(ghPath, i));
                    sigmaYTree.Add(sigmaY.ElementAt(index), new GH_Path(ghPath, i));
                    tauXYTree.Add(tauXY.ElementAt(index), new GH_Path(ghPath, i));
                    tauXZTree.Add(tauXZ.ElementAt(index), new GH_Path(ghPath, i));
                    tauYZTree.Add(tauYZ.ElementAt(index), new GH_Path(ghPath, i));
                    sigmaVMTree.Add(sigmaVM.ElementAt(index), new GH_Path(ghPath, i));
                    sigma1Tree.Add(sigma1.ElementAt(index), new GH_Path(ghPath, i));
                    sigma2Tree.Add(sigma2.ElementAt(index), new GH_Path(ghPath, i));
                    alphaTree.Add(alpha.ElementAt(index), new GH_Path(ghPath, i));
                }
                i++;
            }

            DA.SetDataList("CaseIdentifier", uniqueLoadCase);
            DA.SetDataTree(1, elementIdTree);
            DA.SetDataTree(2, nodeIdTree);
            DA.SetDataTree(3, sigmaXTree);
            DA.SetDataTree(4, sigmaYTree);
            DA.SetDataTree(5, tauXYTree);
            DA.SetDataTree(6, tauXZTree);
            DA.SetDataTree(7, tauYZTree);
            DA.SetDataTree(8, sigmaVMTree);
            DA.SetDataTree(9, sigma1Tree);
            DA.SetDataTree(10, sigma2Tree);
            DA.SetDataTree(11, alphaTree);
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
            get { return new Guid("813EC5C8-0C41-41E1-8778-E8C49E015DAC"); }
        }
    }
}