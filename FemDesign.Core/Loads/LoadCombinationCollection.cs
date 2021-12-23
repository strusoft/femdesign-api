using System;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Loads
{
    /// <summary>
    /// Class for combining load cases
    /// </summary>
    public class LoadCombinationCollection
    {
        /// List of load combinations in collection
        public List<LoadCombination> LoadCombinations { get; set; }

        ///<summary>
        /// Constructor
        ///</summary>
        public LoadCombinationCollection()
        {
            LoadCombinations = new List<LoadCombination>();
        }

        ///<summary>
        /// Adds load combinations if not already present in LoadCombinations
        ///</summary>
        private void AddLoadCombination(LoadCombination loadCombination)
        {
            bool areLoadCasesSame = false;
            bool areGammasSame = false;

            foreach (LoadCombination currentLoadCombination in LoadCombinations)
            {
                areLoadCasesSame = currentLoadCombination.ModelLoadCase.SequenceEqual(loadCombination.ModelLoadCase, new GenericClasses.ModelLoadCaseComparer());
                areGammasSame = Enumerable.SequenceEqual(currentLoadCombination.GetGammas(), loadCombination.GetGammas());
            }

            if (!(areLoadCasesSame && areGammasSame))
                LoadCombinations.Add(loadCombination);
        }

        /// <summary>
        /// Creates an instance of <see cref="LoadCombination">LoadCombination</see> from the load cases in <paramref name="temporaryLoadCases"/>, 
        /// the first load case is the leading action.
        /// </summary>
        /// <param name="temporaryLoadCases">List of permanent load cases to be included in the combination</param>
        /// <param name="loadCombNumber">The index of the combination</param>
        /// <param name="loadCombinationNameTag">The name tag used for naming the combination</param>
        /// <param name="permanentLoadGroups">List of permanent load groups</param>
        /// <param name="associatedTemporaryLoadGroups">List of temporary load groups</param>
        /// <param name="combinationType">Type of loda combination <see cref="ELoadCombinationType">ELoadCombinationType</see></param>
        /// <returns></returns>
        private LoadCombination CreateLoadCombination<T>(List<LoadCase> temporaryLoadCases, int loadCombNumber, string loadCombinationNameTag,
                                                            List<T> permanentLoadGroups,
                                                            List<LoadGroupBase> associatedTemporaryLoadGroups, ELoadCombinationType combinationType) where T : LoadGroupBase
        {
            List<double> loadCombGammas = new List<double>();
            List<LoadCase> loadCases = new List<LoadCase>();

            // Add permanent load cases and coefficients
            AddPermanentLoadCases(permanentLoadGroups, loadCases, combinationType, loadCombGammas);

            // Get the indices of the leading action (or actions)
            List<int> indicesLeadingCases = GetLeadingLoadCaseIndices(associatedTemporaryLoadGroups);

            // Add variable load cases
            string leadingActionName;
            leadingActionName = AddTemporaryLoadCases(temporaryLoadCases, combinationType, loadCases,
                                                                                   indicesLeadingCases, associatedTemporaryLoadGroups, loadCombGammas);

            string loadCombinationType = "ultimate_ordinary";

            if (combinationType == ELoadCombinationType.SixTenA || combinationType == ELoadCombinationType.SixTenB)
                loadCombinationType = "ultimate_ordinary";
            else if (combinationType == ELoadCombinationType.Characteristic)
                loadCombinationType = "serviceability_characteristic";
            else if (combinationType == ELoadCombinationType.Frequent)
                loadCombinationType = "serviceability_frequent";
            else if (combinationType == ELoadCombinationType.QuasiPermanent)
                loadCombinationType = "serviceability_quasi_permanent";

            // Create load combination
            string loadCombName;
            if (combinationType == ELoadCombinationType.SixTenA)
                loadCombName = "LC " + loadCombNumber.ToString() + " " + loadCombinationNameTag;
            else
                loadCombName = "LC " + loadCombNumber.ToString() + " " + loadCombinationNameTag + " - " + leadingActionName + " as leading action";
            LoadCombination loadCombination = new LoadCombination(loadCombName, loadCombinationType, loadCases, loadCombGammas);

            return loadCombination;
        }

        /// <summary>
        /// Finds all combinations, where load cases from the first group are leading actions, from the provided <paramref name="loadGroups"/>, 
        /// by selecting one case from each group
        /// </summary>
        /// <param name="loadGroups"></param>
        /// <returns>Two nested lists with load cases and their associated load groups, where each element in the out list is a list with load cases or groups</returns>
        private (List<List<LoadCase>>, List<List<LoadGroupBase>>) PermuteLoadCases<T>(List<T> loadGroups) where T: LoadGroupBase
        {
            List<List<LoadCase>> loadPermutations = new List<List<LoadCase>>();
            List<List<LoadGroupBase>> associatedloadGroups = new List<List<LoadGroupBase>>();

            // To keep track of next load case in each of the load groups
            int[] indices = new int[loadGroups.Count];
            int combIter = 0;

            // Start with first load case in each load group
            for (int i = 0; i < loadGroups.Count; i++)
                indices[i] = 0;

            while (true)
            {
                loadPermutations.Add(new List<LoadCase>());
                associatedloadGroups.Add(new List<LoadGroupBase>());

                // Store current combination and associated load group
                for (int i = 0; i < loadGroups.Count; i++)
                {
                    if (loadGroups[i].Relationship == ELoadGroupRelationship.Entire)
                    {
                        loadPermutations[combIter].AddRange(loadGroups[i].LoadCase);
                        for (int j = 0; j < loadGroups[i].LoadCase.Count; j++)
                            associatedloadGroups[combIter].Add(loadGroups[i]);
                    }
                    else
                    {
                        loadPermutations[combIter].Add(loadGroups[i].LoadCase[indices[i]]);
                        associatedloadGroups[combIter].Add(loadGroups[i]);
                    }
                }

                // Find the last load group that has more load cases
                // left after the current load case in that group
                int next = loadGroups.Count - 1;
                while (next >= 0 &&
                      (indices[next] + 1 >=
                       loadGroups[next].LoadCase.Count))
                    next--;

                // No such load group is found so no more combinations left
                if (next < 0)
                    return (loadPermutations, associatedloadGroups);

                // If sucg load group found, move to next load case in that group
                indices[next]++;

                // For all load groups to the right of this load groups current index again points to first element
                for (int i = next + 1; i < loadGroups.Count; i++)
                    indices[i] = 0;

                combIter++;
            }
        }

        /// <summary>
        /// Combines the load cases in the list of load groups provided
        /// </summary>
        /// <param name="loadGroups"></param>
        /// <param name="loadCombinationNameTag"></param>
        /// <param name="combinationType"></param>
        /// <returns>Returns a list of instances of load combinations and the set of all load cases used</returns>
        public (List<LoadCombination>, List<LoadCase>) GenerateLoadCombinations(List<LoadGroupBase> loadGroups, string loadCombinationNameTag, ELoadCombinationType combinationType)
        {
            // Separate out the permanent load groups and the temporary
            List<LoadGroupPermanent> permanentLoadGroups = loadGroups.Where(lg => lg is LoadGroupPermanent).Cast<LoadGroupPermanent>().ToList();
            List<LoadGroupTemporary> temporaryLoadGroups = loadGroups.Where(lg => lg is LoadGroupTemporary).Cast<LoadGroupTemporary>().ToList();

            // Initiate lists for storing load cases and groups for each combination
            int loadCombCounter = 1;
            List<LoadCase> loadCasesInComb = new List<LoadCase>();
            List<double> loadCombGammas = new List<double>();
            List<List<LoadCase>> loadCasePermutations = new List<List<LoadCase>>();
            List<List<LoadGroupBase>> associatedPermanentLoadGroups = new List<List<LoadGroupBase>>();
            List<List<LoadCase>> loadCasePermutationsTemp = new List<List<LoadCase>>();
            List<List<LoadGroupBase>> associatedLoadGroupsTemp = new List<List<LoadGroupBase>>();


            // Find all combinations of temporary load groups, such that all groups are leading action once (order of accompyaning actions not included)
            for (int i = 0; i < temporaryLoadGroups.Count(); i++)
            {
                if (!temporaryLoadGroups[i].LeadingCases)
                    continue; // Dont generate combinations where current groups load cases are leading actions

                ExtensionMethods.Swap(temporaryLoadGroups, i, 0);
                (loadCasePermutationsTemp, associatedLoadGroupsTemp) = PermuteLoadCases(temporaryLoadGroups);
                loadCasePermutations.AddRange(loadCasePermutationsTemp);
                associatedPermanentLoadGroups.AddRange(associatedLoadGroupsTemp);
            }

            // Create a load combination for each permutation of temporary loads
            for (int i = 0; i < loadCasePermutations.Count; i++)
            {
                LoadCombination currentLoadCombination;
                currentLoadCombination = CreateLoadCombination(loadCasePermutations[i], loadCombCounter,
                                                                            loadCombinationNameTag, permanentLoadGroups,
                                                                            associatedPermanentLoadGroups[i], combinationType);
                AddLoadCombination(currentLoadCombination);
                
                // If 6.10a only one combination is needed
                if (combinationType == ELoadCombinationType.SixTenA)
                    break;

                loadCombCounter++;
            }

            // Find all unique load cases
            List<LoadCase> usedLoadCases = GetLoadCaseSet(loadGroups.Cast<LoadGroupBase>().ToList());
            return (LoadCombinations, usedLoadCases);
        }

        /// <summary>
        /// Adds the load cases in <paramref name="permanentLoadGroups"/> to <paramref name="loadCases"/> and computes associated load coefficients
        /// </summary>
        /// <param name="permanentLoadGroups"></param>
        /// <param name="loadCases"></param>
        /// <param name="combinationType"></param>
        /// <param name="loadCombGammas"></param>
        private void AddPermanentLoadCases<T>(List<T> permanentLoadGroups, List<LoadCase> loadCases, 
                                                                     ELoadCombinationType combinationType, List<double> loadCombGammas) where T : LoadGroupBase
        {
            foreach (LoadGroupBase loadGroup in permanentLoadGroups)
            {
                foreach (LoadCase loadCase in loadGroup.LoadCase)
                {
                    loadCases.Add(loadCase);
                    if (combinationType == ELoadCombinationType.SixTenA)
                        loadCombGammas.Add(((LoadGroupPermanent)loadGroup).StandardUnfavourable);
                    else if (combinationType == ELoadCombinationType.SixTenB)
                        loadCombGammas.Add(((LoadGroupPermanent)loadGroup).Xi * ((LoadGroupPermanent)loadGroup).StandardUnfavourable);
                    else if (combinationType == ELoadCombinationType.Characteristic)
                        loadCombGammas.Add(1);
                    else if (combinationType == ELoadCombinationType.Frequent)
                        loadCombGammas.Add(1);
                    else if (combinationType == ELoadCombinationType.QuasiPermanent)
                        loadCombGammas.Add(1);
                }
            }
        }

        /// <summary>
        /// Add temporary load cases and associated combination factors to the list of load cases and factors provided
        /// </summary>
        /// <param name="temporaryLoadCases">List of temporary load cases to add</param>
        /// <param name="combinationType">Combination type used for the factors</param>
        /// <param name="loadCases">A list of load cases to add to</param>
        /// <param name="indicesLeadingCases">The indices of the leading load cases</param>
        /// <param name="associatedTemporaryLoadGroups">The load groups that the load cases belong to</param>
        /// <param name="loadCombGammas">A list of combiation factors to add to</param>
        private string AddTemporaryLoadCases(List<LoadCase> temporaryLoadCases, ELoadCombinationType combinationType,
                                                                     List<LoadCase> loadCases, List<int> indicesLeadingCases, 
                                                                     List<LoadGroupBase> associatedTemporaryLoadGroups, List<double> loadCombGammas)
        {
            string leadingActionName = "";

            for (int i = 0; i < temporaryLoadCases.Count; i++)
            {
                LoadGroupTemporary loadGroupTemporary = (LoadGroupTemporary)associatedTemporaryLoadGroups[i];

                // If combination type is not 6.10a, include load case
                if (combinationType != ELoadCombinationType.SixTenA)
                    loadCases.Add(temporaryLoadCases[i]);

                // If leading action
                if (indicesLeadingCases.Contains(i))
                {
                    // Find a suitable name for the leading action
                    if (associatedTemporaryLoadGroups[i].Relationship == ELoadGroupRelationship.Alternative)
                        leadingActionName = temporaryLoadCases[i].Name;
                    else if (associatedTemporaryLoadGroups[i].Relationship == ELoadGroupRelationship.Entire)
                        leadingActionName = temporaryLoadCases[0].Name;

                    // Assign the combination factors
                    if (combinationType == ELoadCombinationType.SixTenB)
                        loadCombGammas.Add(loadGroupTemporary.SafetyFactor);
                    else if (combinationType == ELoadCombinationType.Characteristic)
                        loadCombGammas.Add(1);
                    else if (combinationType == ELoadCombinationType.Frequent)
                        loadCombGammas.Add(loadGroupTemporary.Psi1);
                    else if (combinationType == ELoadCombinationType.QuasiPermanent)
                        loadCombGammas.Add(loadGroupTemporary.Psi2);
                }
                else
                { //Else accompanying action
                    if (combinationType == ELoadCombinationType.SixTenB)
                        loadCombGammas.Add(loadGroupTemporary.SafetyFactor * loadGroupTemporary.Psi0);
                    else if (combinationType == ELoadCombinationType.Characteristic)
                        loadCombGammas.Add(loadGroupTemporary.Psi0);
                    else if (combinationType == ELoadCombinationType.Frequent)
                        loadCombGammas.Add(loadGroupTemporary.Psi2);
                    else if (combinationType == ELoadCombinationType.QuasiPermanent)
                        loadCombGammas.Add(loadGroupTemporary.Psi2);
                }

                // Remove the load case if gamma is zero
                if (loadCombGammas[loadCombGammas.Count - 1] == 0)
                {
                    loadCombGammas.RemoveAt(loadCombGammas.Count - 1);
                    loadCases.RemoveAt(loadCases.Count - 1);

                }
            }
            return (leadingActionName);
        }

        /// <summary>
        /// Gets the indices of the leading action (0) if load case relation is entire or the whole groups indices if load case relation is entire
        /// </summary>
        /// <param name="associatedTemporaryLoadGroups"></param>
        /// <returns>Returns a list of indices</returns>
        private List<int> GetLeadingLoadCaseIndices(List<LoadGroupBase> associatedTemporaryLoadGroups)
        {
            List<int> indicesLeadingCases = new List<int>();
            // If relationship is entire the whole group is leading action
            if (associatedTemporaryLoadGroups[0].Relationship == ELoadGroupRelationship.Entire)
                indicesLeadingCases.AddRange(Enumerable.Range(0, associatedTemporaryLoadGroups[0].LoadCase.Count));
            else if (associatedTemporaryLoadGroups[0].Relationship == ELoadGroupRelationship.Alternative)
                indicesLeadingCases.Add(0);

            return indicesLeadingCases;
        }

        /// <summary>
        /// Generates the set of load cases in the list of load groups (unique load cases)
        /// </summary>
        /// <param name="loadGroups">List of load groups</param>
        /// <returns>List of unique load cases</returns>
        private List<LoadCase> GetLoadCaseSet(List<LoadGroupBase> loadGroups)
        {
            List<LoadCase> usedLoadCases = new List<LoadCase>();
            foreach (LoadGroupBase loadGroup in loadGroups)
                usedLoadCases.AddRange(loadGroup.LoadCase);
            usedLoadCases = usedLoadCases.Distinct().ToList();
            return usedLoadCases;
        }
    }
}
