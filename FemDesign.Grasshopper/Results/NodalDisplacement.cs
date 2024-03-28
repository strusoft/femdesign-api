using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;
using FemDesign.Loads;
using Rhino.Commands;
using Grasshopper.Kernel.Data;

namespace FemDesign.Grasshopper
{
    public class NodalDisplacement : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the NodalDisplacement class.
        /// </summary>
        public NodalDisplacement()
          : base("NodalDisplacement",
                "NodalDisplacement",
                "Read the nodal displacement for the entire model",
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
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.", GH_ParamAccess.list);
            pManager.AddTextParameter("ElementId", "ElementId", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("NodeId", "NodeId", "Node Index", GH_ParamAccess.list);
            pManager.AddVectorParameter("Translation", "Translation", "Nodal translations in global x, y, z for all nodes.", GH_ParamAccess.list);
            pManager.AddVectorParameter("Rotation", "Rotation", "Nodal rotations in global x, y, z for all nodes.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.NodalDisplacement> results = new List<FemDesign.Results.NodalDisplacement>();
            DA.GetDataList("Result", results);

            string loadCase = null;
            DA.GetData(1, ref loadCase);


            if (loadCase != null)
            {
                results = results.Where(x => x.CaseIdentifier == loadCase).ToList();
            }

            // collect output in datatree
            DataTree<string> loadCases = new DataTree<string>();
            DataTree<string> elementId = new DataTree<string>();
            DataTree<int> nodeId = new DataTree<int>();
            DataTree<Vector3d> translation = new DataTree<Vector3d>();
            DataTree<Vector3d> rotation = new DataTree<Vector3d>();

            var grouping = results.GroupBy(x => x.CaseIdentifier);

            int iteration = DA.Iteration;
            int grpCounter = 0;
            foreach (var group in grouping)
            {
                var caseIdentifier = group.Key;
                var elementIds = group.Select(x => x.Id);
                var nodeIds = group.Select(x => x.NodeId);
                var translations = group.Select(x => new Vector3d(x.Ex, x.Ey, x.Ez));
                var rotations = group.Select(x => new Vector3d(x.Fix, x.Fiy, x.Fiz));


                loadCases.Add(caseIdentifier, new GH_Path(iteration, grpCounter));
                elementId.AddRange(elementIds, new GH_Path(iteration, grpCounter));
                nodeId.AddRange(nodeIds, new GH_Path(iteration, grpCounter));
                translation.AddRange(translations, new GH_Path(iteration, grpCounter));
                rotation.AddRange(rotations, new GH_Path(iteration, grpCounter));
                grpCounter++;
            }


            // Set output
            DA.SetDataTree(0, loadCases);
            DA.SetDataTree(1, elementId);
            DA.SetDataTree(2, nodeId);
            DA.SetDataTree(3, translation);
            DA.SetDataTree(4, rotation);
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

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
            get { return new Guid("{AF74B638-2EE0-43F1-82BC-89CFB17728FB}"); }
        }
    }
}