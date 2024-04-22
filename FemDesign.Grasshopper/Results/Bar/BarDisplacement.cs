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
    public class BarDisplacement : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the BarDisplacement class.
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
            pManager[pManager.ParamCount - 1].Optional = true;
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
            List<FemDesign.Results.BarDisplacement> results = new List<FemDesign.Results.BarDisplacement>();
            DA.GetDataList("Result", results);

            string loadCase = null;
            DA.GetData(1, ref loadCase);

            if (loadCase != null)
            {
                results = results.Where(x => x.CaseIdentifier == loadCase).ToList();
            }


            DataTree<string> loadCases = new DataTree<string>();
            DataTree<string> elementId = new DataTree<string>();
            DataTree<double> positionResult = new DataTree<double>();
            DataTree<Vector3d> translation = new DataTree<Vector3d>();
            DataTree<Vector3d> rotation = new DataTree<Vector3d>();

            var grouping = results.GroupBy(x => x.CaseIdentifier);
            
            int i = 0;
            int iteration = DA.Iteration;

            foreach (var group in grouping)
            {
                var caseIdentifier = group.Key;
                var elementIds = group.Select(x => x.Id);
                var positionResults = group.Select(x => x.Pos);
                var translations = group.Select(x => new Vector3d(x.Ex, x.Ey, x.Ez));
                var rotations = group.Select(x => new Vector3d(x.Fix, x.Fiy, x.Fiz));


                loadCases.Add(caseIdentifier, new GH_Path(iteration, i));
                elementId.AddRange(elementIds, new GH_Path(iteration, i));
                positionResult.AddRange(positionResults, new GH_Path(iteration, i));
                translation.AddRange(translations, new GH_Path(iteration, i));
                rotation.AddRange(rotations, new GH_Path(iteration, i));
                i++;
            }

            // Set output
            DA.SetDataTree(0, loadCases);
            DA.SetDataTree(1, elementId);
            DA.SetDataTree(2, positionResult);
            DA.SetDataTree(3, translation);
            DA.SetDataTree(4, rotation);
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
            get { return new Guid("{0F122166-B666-4B59-9FBC-EA1B7D9A466D}"); }
        }
    }
}