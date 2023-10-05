using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.GenericClasses;
using FemDesign.Utils;

namespace FemDesign.Grasshopper
{
    /// <summary>
    /// A class for generating load combinations based on a national annex
    /// </summary>
    public class LoadGroupCombineGeneral : FEM_Design_API_Component
    {
        public LoadGroupCombineGeneral()
          : base("LoadGroup.CombineGeneral", "CombineGeneral", "This component/method is a utility contribution from a user (@GabrielEdefors) and further developed by another user (@Boohman). This component/method is not part of FEM-Design but might still be useful when automating stuff 😉\n\nCombines the load cases in each load group into load combinations", "FEM-Design Utils", "Loads")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "LoadGroups to include in LoadCombination. Single LoadGroups or list of LoadGroups.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("CombinationType", "CombinationType", "Type of combination", GH_ParamAccess.item, 0);
            Param_Integer typeComb = pManager[1] as Param_Integer;
            typeComb.AddNamedValue("6.10a", 0);
            typeComb.AddNamedValue("6.10b", 1);
            typeComb.AddNamedValue("characteristic", 2);
            typeComb.AddNamedValue("frequent", 3);
            typeComb.AddNamedValue("quasi permanent", 4);
            pManager.AddIntegerParameter("NationalAnnex", "NationalAnnex", "National annex for combining loads", GH_ParamAccess.item, 0);
            Param_Integer typeNationalAnex = pManager[2] as Param_Integer;
            typeNationalAnex.AddNamedValue("EKS", 0);
            typeNationalAnex.AddNamedValue("TSFS", 1);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "List of load combinations", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get data
            List<FemDesign.Loads.ModelGeneralLoadGroup> loadGroups = new List<FemDesign.Loads.ModelGeneralLoadGroup>();
            if (!DA.GetDataList(0, loadGroups)) { return; }
            if (loadGroups == null) { return; }

            // Combination type
            int combType = 0;
            if (!DA.GetData(1, ref combType)) { return; }

            // National annex
            int natAnnex = 0;
            if (!DA.GetData(2, ref natAnnex)) { return; }

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

            // Convert National Annex to Enum
            ENationalAnnex nationalAnnexEnum = ENationalAnnex.EKS;

            if (natAnnex == 0)
                nationalAnnexEnum = ENationalAnnex.EKS;
            else if (natAnnex == 1)
                nationalAnnexEnum = ENationalAnnex.TSFS;

            // Raise error if 6.10a and no permanent load group
            int permanentGroupsCount = 0;
            if(combTypeEnum == ELoadCombinationType.SixTenA)
            {
                foreach (FemDesign.Loads.ModelGeneralLoadGroup loadGroup in loadGroups)
                {
                    if (loadGroup.GetSpecificLoadGroup() is FemDesign.Loads.LoadGroupPermanent)
                        permanentGroupsCount += 1;
                }
                if(permanentGroupsCount == 0)
                    throw new System.ArgumentException("6.10a requires at least one permanent load group");
            }


            // Create load combinations
            List<FemDesign.Loads.LoadCombination> loadCombinations;
            List<FemDesign.Loads.LoadGroupBase> specificLoadGroups = loadGroups.Select(lg => lg.GetSpecificLoadGroup()).ToList();
            loadCombinations = CreateCombinations(specificLoadGroups, combTypeEnum, nationalAnnexEnum);

            DA.SetDataList(0, loadCombinations);
        }

        private List<FemDesign.Loads.LoadCombination> CreateCombinations(List<FemDesign.Loads.LoadGroupBase> loadGroups, ELoadCombinationType combinationType, ENationalAnnex nationalAnnex)
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
            loadCombinationTable.GenerateLoadCombinations(loadGroups, loadCombinationNameTag, combinationType, nationalAnnex);
            return loadCombinationTable.LoadCombinations;
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.CombineLoadGroups;
        public override Guid ComponentGuid => new Guid("15962A56-37AB-4ECD-B808-F555D7D2A588");
    }
}
