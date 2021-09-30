using System;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Loads
{
    /// <summary>
    /// Class for combining load cases
    /// </summary>
    public static class LoadCaseCombiner
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public static (List<List<LoadCase>>, List<List<LoadGroup>>) PermuteLoadCases(List<LoadGroup> loadGroups)
        {
            //Save load group index associated to each element in arr
            List<LoadGroup>[] loadGroupRef = new List<LoadGroup>[loadGroups.Count];

            // Put all load cases in in an array, each array item is a list with load cases
            List<LoadCase>[] arr = new List<LoadCase>[loadGroups.Count];
            for (int i = 0; i < loadGroups.Count; i++)
            {
                if(loadGroups[i].LoadCaseRelation == ELoadGroupRelation.Alternative)
                {
                    arr[i] = loadGroups[i].LoadCases;
                    loadGroupRef[i] = new List<LoadGroup>();
                    for (int j = 0; j < arr[i].Count; j++)
                        loadGroupRef[i].Add(loadGroups[i]);
                }
                else if(loadGroups[i].LoadCaseRelation == ELoadGroupRelation.Entire)
                {
                    // Just use one of the load cases for now and add all load cases later
                    arr[i] = new List<LoadCase>() { loadGroups[i].LoadCases[0]};
                    loadGroupRef[i] = new List<LoadGroup>() { loadGroups[i] };
                }
            }
                
            // Number of arrays
            int n = arr.Length;

            List<List<LoadCase>> loadPermutations = new List<List<LoadCase>>();
            List<List<LoadGroup>> associatedloadGroups = new List<List<LoadGroup>>();

            // To keep track of next
            // element in each of
            // the n arrays
            int[] indices = new int[n];
            int combIter = 0;

            // Initialize with first
            // element's index
            for (int i = 0; i < n; i++)
                indices[i] = 0;

            while (true)
            {
                loadPermutations.Add(new List<LoadCase>());
                associatedloadGroups.Add(new List<LoadGroup>());

                // Store current combination and associated load group
                for (int i = 0; i < n; i++)
                {
                    if(loadGroupRef[i][indices[i]].LoadCaseRelation == ELoadGroupRelation.Entire)
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
                    

                // Find the rightmost array
                // that has more elements
                // left after the current
                // element in that array
                int next = n - 1;
                while (next >= 0 &&
                      (indices[next] + 1 >=
                       arr[next].Count))
                    next--;

                // No such array is found
                // so no more combinations left
                if (next < 0)
                    return (loadPermutations, associatedloadGroups);

                // If found move to next
                // element in that array
                indices[next]++;

                // For all arrays to the right
                // of this array current index
                // again points to first element
                for (int i = next + 1; i < n; i++)
                    indices[i] = 0;

                combIter++;
            }

        }

        /// <summary>
        /// Creates an instance of <see cref="LoadCombination">LoadCombination</see>, includes all permanent loads in all combinations.
        /// </summary>
        /// <param name="temporaryLoadCases">List of permanent load cases to be included in the combination</param>
        /// <param name="loadCombNumber">The index of the combination</param>
        /// <param name="loadCombinationNameTag">The name tag used for naming the combination</param>
        /// <param name="permanentLoadGroups">List of permanent load groups</param>
        /// <param name="temporaryLoadGroups">List of temporary load groups</param>
        /// <param name="combinationType">Type of loda combination <see cref="ELoadCombinationType">ELoadCombinationType</see></param>
        /// <returns></returns>
        public static LoadCombination CreateLoadCombination(List<LoadCase> temporaryLoadCases, int loadCombNumber, string loadCombinationNameTag, 
                                                            List<LoadGroup> permanentLoadGroups, 
                                                            List<LoadGroup> temporaryLoadGroups, ELoadCombinationType combinationType)
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
                    else
                        loadCombGammas.Add(loadGroup.Gamma_d * loadGroup.Xi * loadGroup.SafetyFactor);
                }
            }

            string leadingActionName = "";

            // Add variable load cases
            for (int i = 0; i < temporaryLoadCases.Count; i++)
            {
                // First load case is the leading action
                if (i == 0)
                {
                    if (combinationType == ELoadCombinationType.SixTenB)
                    {
                        loadCases.Add(temporaryLoadCases[i]);
                        loadCombGammas.Add(temporaryLoadGroups[i].Gamma_d * temporaryLoadGroups[i].SafetyFactor);

                        if (temporaryLoadGroups[i].LoadCaseRelation == ELoadGroupRelation.Alternative)
                            leadingActionName = temporaryLoadCases[i].Name;
                        else if (temporaryLoadGroups[i].LoadCaseRelation == ELoadGroupRelation.Entire)
                            leadingActionName = temporaryLoadGroups[i].Name;
                    }
                }
                else
                { //Else accompanying action
                    if (combinationType == ELoadCombinationType.SixTenB)
                    {
                        loadCases.Add(temporaryLoadCases[i]);
                        loadCombGammas.Add(temporaryLoadGroups[i].Gamma_d * temporaryLoadGroups[i].SafetyFactor * temporaryLoadGroups[i].PsiValues[0]);
                    }
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
    }
}
