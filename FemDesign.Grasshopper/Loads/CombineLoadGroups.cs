using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    public class CombineLoadGroups : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CombineLoadGroups class.
        /// </summary>
        public CombineLoadGroups()
          : base("CombineLoadGroups", "combineLoadGroups", "Combines the load cases in each load group into load combinations", "FemDesign", "Loads")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "LoadGroups to include in LoadCombination. Single LoadGrousp or list of LoadGroups.", GH_ParamAccess.list);
            pManager.AddTextParameter("CombinationType", "CombinationType", "'Type of combination. 6.10a/6.10b", GH_ParamAccess.item, "6.10b");
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "List of load combinations", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            List<FemDesign.Loads.LoadGroup> loadCases = new List<FemDesign.Loads.LoadGroup>();
            if (!DA.GetDataList(0, loadCases)) { return; }
            if (loadCases == null) { return; }

            string combType = "6.10b";
            if (!DA.GetData(1, ref combType))
            {
                // pass
            }

            if (combType == null) { return; }

            // Create load combinations
            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<LoadCombination>();

            DA.SetDataList(0, loadCombinations);

        }

        private List<LoadCombination> CreateCombinations610b(List<LoadGroup> loadGroups)
        {
            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<LoadCombination>();
            FemDesign.Loads.LoadCombination loadCombination;

            foreach (LoadGroup loadGroup in loadGroups)
            {
                 //loadCombination = new FemDesign.Loads.LoadCombination(name, type, loadCases, gammas);

                if(loadGroup.Type == ELoadGroupType.Permanent)

                if (loadGroup.LoadCaseRelation == ELoadGroupRelation.Correlated)
                {
                    
                }
            }

            return loadCombinations;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("fefa0b81-39da-47b3-b126-358673888f62"); }
        }
    }
}