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
        /// Creates an instance of <see cref="LoadCombination">LoadCombination</see>, includes all permanent loads in all combinations.
        /// </summary>
        /// <param name="temporaryLoadCases">List of permanent load cases to be included in the combination</param>
        /// <param name="loadCombNumber">The index of the combination</param>
        /// <param name="loadCombinationNameTag">The name tag used for naming the combination</param>
        /// <param name="permanentLoadGroups">List of permanent load groups</param>
        /// <param name="associatedTemporaryLoadGroups">List of temporary load groups</param>
        /// <param name="combinationType">Type of loda combination <see cref="ELoadCombinationType">ELoadCombinationType</see></param>
        /// <returns></returns>
        private LoadCombination CreateLoadCombination(List<LoadCase> temporaryLoadCases, int loadCombNumber, string loadCombinationNameTag,
                                                            List<LoadGroup> permanentLoadGroups,
                                                            List<LoadGroup> associatedTemporaryLoadGroups, ELoadCombinationType combinationType)
        {

            List<double> loadCombGammas = new List<double>();
            List<LoadCase> loadCases = new List<LoadCase>();

            // Add permanent load cases
            foreach (LoadGroup loadGroup in permanentLoadGroups)
            {
                foreach (LoadCase loadCase in loadGroup.LoadCases)
                {
                    loadCases.Add(loadCase);
                    if (combinationType == ELoadCombinationType.SixTenA)
                        loadCombGammas.Add(loadGroup.Gamma_d * loadGroup.SafetyFactor);
                    else if (combinationType == ELoadCombinationType.SixTenB)
                        loadCombGammas.Add(loadGroup.Gamma_d * loadGroup.Xi * loadGroup.SafetyFactor);
                    else if (combinationType == ELoadCombinationType.Characteristic)
                        loadCombGammas.Add(1);
                    else if (combinationType == ELoadCombinationType.Frequent)
                        loadCombGammas.Add(1);
                    else if (combinationType == ELoadCombinationType.QuasiPermanent)
                        loadCombGammas.Add(1);
                }
            }

            string leadingActionName = "";

            // Find out which load cases are leading actions
            List<int> indicesLeadingCases = new List<int>();
            // If relationship is entire the whole group is leading action
            if (associatedTemporaryLoadGroups[0].LoadCaseRelation == ELoadGroupRelationship.Entire)
                indicesLeadingCases.AddRange(Enumerable.Range(0, associatedTemporaryLoadGroups[0].LoadCases.Count));
            else if (associatedTemporaryLoadGroups[0].LoadCaseRelation == ELoadGroupRelationship.Alternative)
                indicesLeadingCases.Add(0);

            // Add variable load cases
            for (int i = 0; i < temporaryLoadCases.Count; i++)
            {
                // If combination type is not 6.10a, include load case
                if (combinationType != ELoadCombinationType.SixTenA)
                    loadCases.Add(temporaryLoadCases[i]);

                // If leading action
                if (indicesLeadingCases.Contains(i))
                {
                    // Find a suitable name for the leading action
                    if (associatedTemporaryLoadGroups[i].LoadCaseRelation == ELoadGroupRelationship.Alternative)
                        leadingActionName = temporaryLoadCases[i].Name;
                    else if (associatedTemporaryLoadGroups[i].LoadCaseRelation == ELoadGroupRelationship.Entire)
                        leadingActionName = associatedTemporaryLoadGroups[i].Name;

                    // Assign the combination factors
                    if (combinationType == ELoadCombinationType.SixTenB)
                        loadCombGammas.Add(associatedTemporaryLoadGroups[i].Gamma_d * associatedTemporaryLoadGroups[i].SafetyFactor);
                    else if (combinationType == ELoadCombinationType.Characteristic)
                        loadCombGammas.Add(1);
                    else if (combinationType == ELoadCombinationType.Frequent)
                        loadCombGammas.Add(associatedTemporaryLoadGroups[i].LoadCategory.Psi1);
                    else if (combinationType == ELoadCombinationType.QuasiPermanent)
                        loadCombGammas.Add(associatedTemporaryLoadGroups[i].LoadCategory.Psi2);
                }
                else
                { //Else accompanying action
                    if (combinationType == ELoadCombinationType.SixTenB)
                        loadCombGammas.Add(associatedTemporaryLoadGroups[i].Gamma_d * associatedTemporaryLoadGroups[i].SafetyFactor * associatedTemporaryLoadGroups[i].LoadCategory.Psi0);
                    else if (combinationType == ELoadCombinationType.Characteristic)
                        loadCombGammas.Add(associatedTemporaryLoadGroups[i].LoadCategory.Psi0);
                    else if (combinationType == ELoadCombinationType.Frequent)
                        loadCombGammas.Add(associatedTemporaryLoadGroups[i].LoadCategory.Psi2);
                    else if (combinationType == ELoadCombinationType.QuasiPermanent)
                        loadCombGammas.Add(associatedTemporaryLoadGroups[i].LoadCategory.Psi2);
                }

                // Remove the load case if gamma is zero
                if (loadCombGammas[loadCombGammas.Count - 1] == 0)
                {
                    loadCombGammas.RemoveAt(loadCombGammas.Count - 1);
                    loadCases.RemoveAt(loadCases.Count - 1);
                
                }
            }

            string loadCombinationType = "ultimate_ordinary";

            if (combinationType == ELoadCombinationType.SixTenA || combinationType == ELoadCombinationType.SixTenB)
                loadCombinationType = "ultimate_ordinary";
            else if (combinationType == ELoadCombinationType.Characteristic)
                loadCombinationType = "serviceability_characteristic";
            else if (combinationType == ELoadCombinationType.Frequent)
                loadCombinationType = "serviceability_frequent";
            else if (combinationType == ELoadCombinationType.QuasiPermanent)
                loadCombinationType = "serviceability_quasi_permanent";

            string loadCombName = "LC " + loadCombNumber.ToString() + " " + loadCombinationNameTag + " - " + leadingActionName + " as leading action";
            LoadCombination loadCombination = new LoadCombination(loadCombName, loadCombinationType, loadCases, loadCombGammas);

            return loadCombination;
        }

        /// <summary>
        /// Finds all combinations from the provided load groups by selecting one case from each group
        /// </summary>
        /// <param name="loadGroups"></param>
        /// <returns></returns>
        private (List<List<LoadCase>>, List<List<LoadGroup>>) PermuteLoadCases(List<LoadGroup> loadGroups)
        {
            //Save load group index associated to each element in arr
            List<LoadGroup>[] loadGroupRef = new List<LoadGroup>[loadGroups.Count];

            // Put all load cases in in an array, each array item is a list with load cases
            List<LoadCase>[] arr = new List<LoadCase>[loadGroups.Count];
            for (int i = 0; i < loadGroups.Count; i++)
            {
                if (loadGroups[i].LoadCaseRelation == ELoadGroupRelationship.Alternative)
                {
                    arr[i] = loadGroups[i].LoadCases;
                    loadGroupRef[i] = new List<LoadGroup>();
                    for (int j = 0; j < arr[i].Count; j++)
                        loadGroupRef[i].Add(loadGroups[i]);
                }
                else if (loadGroups[i].LoadCaseRelation == ELoadGroupRelationship.Entire)
                {
                    // Just use one of the load cases for now and add all load cases later
                    arr[i] = new List<LoadCase>() { loadGroups[i].LoadCases[0] };
                    loadGroupRef[i] = new List<LoadGroup>() { loadGroups[i] };
                }
            }

            // Number of arrays
            int n = arr.Length;

            List<List<LoadCase>> loadPermutations = new List<List<LoadCase>>();
            List<List<LoadGroup>> associatedloadGroups = new List<List<LoadGroup>>();

            // To keep track of next element in each of the n arrays
            int[] indices = new int[n];
            int combIter = 0;

            // Initialize with first element's index
            for (int i = 0; i < n; i++)
                indices[i] = 0;

            while (true)
            {
                loadPermutations.Add(new List<LoadCase>());
                associatedloadGroups.Add(new List<LoadGroup>());

                // Store current combination and associated load group
                for (int i = 0; i < n; i++)
                {
                    if (loadGroupRef[i][indices[i]].LoadCaseRelation == ELoadGroupRelationship.Entire)
                    {
                        loadPermutations[combIter].AddRange(loadGroupRef[i][0].LoadCases);
                        for (int j = 0; j < loadGroupRef[i][0].LoadCases.Count; j++)
                            associatedloadGroups[combIter].Add(loadGroupRef[i][0]);
                    }
                    else
                    {
                        loadPermutations[combIter].Add(arr[i][indices[i]]);
                        associatedloadGroups[combIter].Add(loadGroupRef[i][indices[i]]);
                    }
                }

                // Find the rightmost array that has more elements
                // left after the current element in that array
                int next = n - 1;
                while (next >= 0 &&
                      (indices[next] + 1 >=
                       arr[next].Count))
                    next--;

                // No such array is found so no more combinations left
                if (next < 0)
                    return (loadPermutations, associatedloadGroups);

                // If found move to next element in that array
                indices[next]++;

                // For all arrays to the right of this array current index again points to first element
                for (int i = next + 1; i < n; i++)
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
        /// <returns></returns>
        public (List<LoadCombination>, List<LoadCase>) GenerateLoadCombinations(List<LoadGroup> loadGroups, string loadCombinationNameTag, ELoadCombinationType combinationType)
        {
            // Separate out the permanent load groups and the temporary
            IEnumerable<LoadGroup> permanentLoadGroups = loadGroups.Where(lg => lg.Type == ELoadGroupType.Permanent);
            List<LoadGroup> temporaryLoadGroups = loadGroups.Where(lg => lg.Type == ELoadGroupType.Variable).ToList();

            // Initiate lists for storing load cases and groups for each combination
            int loadCombCounter = 1;
            List<LoadCase> loadCasesInComb = new List<LoadCase>();
            List<double> loadCombGammas = new List<double>();
            List<List<LoadCase>> loadCasePermutations = new List<List<LoadCase>>();
            List<List<LoadGroup>> associatedPermanentLoadGroups = new List<List<LoadGroup>>();
            List<List<LoadCase>> loadCasePermutationsTemp = new List<List<LoadCase>>();
            List<List<LoadGroup>> associatedLoadGroupsTemp = new List<List<LoadGroup>>();


            // Find all combinations of temporary load groups, such that all groups are leading action once (order of accompyaning actions not included)
            for (int i = 0; i < temporaryLoadGroups.Count(); i++)
            {
                if (!temporaryLoadGroups[i].PotentiallyLeadingAction)
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
                                                                            loadCombinationNameTag, permanentLoadGroups.ToList(),
                                                                            associatedPermanentLoadGroups[i], combinationType);
                AddLoadCombination(currentLoadCombination);
                
                // If 6.10a only one combination is needed
                if (combinationType == ELoadCombinationType.SixTenA)
                    break;

                loadCombCounter++;
            }

            // Find all unique load cases
            List<LoadCase> usedLoadCases = new List<LoadCase>();

            foreach (LoadGroup loadGroup in loadGroups)
                usedLoadCases.AddRange(loadGroup.LoadCases);
            usedLoadCases = usedLoadCases.Distinct().ToList();

            return (LoadCombinations, usedLoadCases);
        }
    }
}
