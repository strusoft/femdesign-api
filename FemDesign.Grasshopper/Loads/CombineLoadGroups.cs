using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.GenericClasses;

namespace FemDesign.Loads
{
    public class CombineLoadGroups : GH_Component
    {
        public CombineLoadGroups()
          : base("CombineLoadGroups", "CombineLoadGroups", "Combines the load cases in each load group into load combinations", "FemDesign", "Loads")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "LoadGroups to include in LoadCombination. Single LoadGrousp or list of LoadGroups.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("CombinationType", "CombinationType", "Type of combination", GH_ParamAccess.item, 0);
            Param_Integer type = pManager[1] as Param_Integer;
            type.AddNamedValue("6.10a", 0);
            type.AddNamedValue("6.10b", 1);
            type.AddNamedValue("characteristic", 2);
            type.AddNamedValue("frequent", 3);
            type.AddNamedValue("quasi permanent", 4);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "List of load combinations", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCases", "LoadCases", "List of load cases used in the combinatoins", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            List<FemDesign.Loads.LoadGroup> loadGroups = new List<FemDesign.Loads.LoadGroup>();
            if (!DA.GetDataList(0, loadGroups)) { return; }
            if (loadGroups == null) { return; }

            int combType = 0;
            if (!DA.GetData(1, ref combType)) { return; }

            // Convert combination type to enum
            ELoadCombinationType combTypeEnum = ELoadCombinationType.SixTenB;
            if (combType == 0)
                combTypeEnum = ELoadCombinationType.SixTenA;
            else if (combType == 1)
                combTypeEnum = ELoadCombinationType.SixTenB;
            else if (combType == 2)
                combTypeEnum = ELoadCombinationType.Characteristic;
            else if (combType == 3)
                combTypeEnum = ELoadCombinationType.Frequent;
            else if (combType == 4)
                combTypeEnum = ELoadCombinationType.QuasiPermanent;

            // Create load combinations
            List<FemDesign.Loads.LoadCombination> loadCombinations;
            List<LoadCase> loadCases;
            (loadCombinations, loadCases) = CreateCombinations(loadGroups, combTypeEnum);

            DA.SetDataList(0, loadCombinations);
            DA.SetDataList(1, loadCases);

        }

        private (List<LoadCombination>, List<LoadCase>) CreateCombinations(List<LoadGroup> loadGroups, ELoadCombinationType combinationType)
        {
            string loadCombinationNameTag = combinationType.ToString();

            List<LoadCombination> loadCombinations;
            List<LoadCase> usedLoadCases;

            (loadCombinations, usedLoadCases) = LoadCaseCombiner.GenerateLoadCombinations(loadGroups, loadCombinationNameTag, combinationType);

            return (loadCombinations, usedLoadCases);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.CombineLoadGroups;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("fefa0b81-39da-47b3-b126-358673888f62"); }
        }
    }
}