using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.GenericClasses;

namespace FemDesign.Loads
{
    public class LoadGroupCombine : GH_Component
    {
        public LoadGroupCombine()
          : base("LoadGroup.Combine", "Combine", "This component/method is a utility contribution from a user (@GabrielEdefors). This component/method is not part of FEM-Design but might still be useful when automating stuff 😉\n\nCombines the load cases in each load group into load combinations", "FEM-Design Utils", "Loads")
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
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            List<FemDesign.Loads.ModelGeneralLoadGroup> loadGroups = new List<FemDesign.Loads.ModelGeneralLoadGroup>();
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

            // Raise error if 6.10a and no permanent load group
            int permanentGroupsCount = 0;
            if(combTypeEnum == ELoadCombinationType.SixTenA)
            {
                foreach (ModelGeneralLoadGroup loadGroup in loadGroups)
                {
                    if (loadGroup.GetSpecificLoadGroup() is LoadGroupPermanent)
                        permanentGroupsCount += 1;
                }
                if(permanentGroupsCount == 0)
                    throw new System.ArgumentException("6.10a requires at least one permanent load group");
            }


            // Create load combinations
            List<FemDesign.Loads.LoadCombination> loadCombinations;
            List<LoadGroupBase> specificLoadGroups = loadGroups.Select(lg => lg.GetSpecificLoadGroup()).ToList();
            loadCombinations = CreateCombinations(specificLoadGroups, combTypeEnum);

            DA.SetDataList(0, loadCombinations);
        }

        private List<LoadCombination> CreateCombinations(List<LoadGroupBase> loadGroups, ELoadCombinationType combinationType)
        {
            // Fix how the combination type is printed
            string loadCombinationNameTag;
            if (combinationType == ELoadCombinationType.SixTenA)
                loadCombinationNameTag = "6.10a";
            else if (combinationType == ELoadCombinationType.SixTenB)
                loadCombinationNameTag = "6.10b";
            else
                loadCombinationNameTag = combinationType.ToString();

            LoadCombinationTable loadCombinationTable = new LoadCombinationTable();
            loadCombinationTable.GenerateLoadCombinations(loadGroups, loadCombinationNameTag, combinationType);
            return loadCombinationTable.LoadCombinations;
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