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

            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<LoadCombination>();

            // Separate out the permanent load groups and the temporary
            IEnumerable<LoadGroup> permanentLoadGroups = loadGroups.Where(lg => lg.Type == ELoadGroupType.Permanent);
            List<LoadGroup> temporaryLoadGroups = loadGroups.Where(lg => lg.Type == ELoadGroupType.Variable).ToList();

            // Initiate lists for storing load cases and groups for each combination
            int loadCombCounter = 1;
            List<LoadCase> loadCasesInComb = new List<LoadCase>();
            List<double> loadCombGammas = new List<double>();
            List<List<LoadCase>> loadCasePermutations = new List<List<LoadCase>>();
            List<List<LoadGroup>> associatedLoadGroups = new List<List<LoadGroup>>();
            List<List<LoadCase>> loadCasePermutationsTemp = new List<List<LoadCase>>();
            List<List<LoadGroup>> associatedLoadGroupsTemp = new List<List<LoadGroup>>();

            // Find all combinations of temporary load groups, such that all groups are leading action once (order of accompyaning actions not included)
            for (int i = 0; i < temporaryLoadGroups.Count(); i++)
            {
                ExtensionMethods.Swap(temporaryLoadGroups, i, 0);
                (loadCasePermutationsTemp, associatedLoadGroupsTemp) = LoadCaseCombiner.PermuteLoadCases(temporaryLoadGroups);
                loadCasePermutations.AddRange(loadCasePermutationsTemp);
                associatedLoadGroups.AddRange(associatedLoadGroupsTemp);
            }

            // Create a load combination for each permutation of temporary loads
            for (int i = 0; i < loadCasePermutations.Count; i++)
            {
                loadCombinations.Add(LoadCaseCombiner.CreateLoadCombination(loadCasePermutations[i], loadCombCounter, 
                                                                            loadCombinationNameTag, permanentLoadGroups.ToList(), 
                                                                            associatedLoadGroups[i], combinationType));
                loadCombCounter++;
            }

            // Find all unique load cases
            List<LoadCase> usedLoadCases = new List<LoadCase>();

            foreach (LoadGroup loadGroup in loadGroups)
                usedLoadCases.AddRange(loadGroup.LoadCases);
            usedLoadCases = usedLoadCases.Distinct().ToList();
            
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