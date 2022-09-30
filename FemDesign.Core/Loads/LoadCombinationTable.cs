using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Loads
{
    /// <summary>
    /// Class for combining load cases and storing load combinations
    /// </summary>
    public class LoadCombinationTable
    {
        /// List of load combinations in collection
        public List<LoadCombination> LoadCombinations { get; set; }

        ///<summary>
        /// Constructor
        ///</summary>
        public LoadCombinationTable()
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
        /// <param name="combinationType">Type of loda combination <see cref="ELoadCombinationType">ELoadCombinationType</see></param>
        /// <returns></returns>
        private LoadCombination CreateLoadCombination(List<ModelLoadCaseInGroup> temporaryLoadCases, int loadCombNumber, string loadCombinationNameTag,
                                                            List<LoadGroupPermanent> permanentLoadGroups, ELoadCombinationType combinationType)
        {
            List<double> loadCombGammas = new List<double>();
            List<LoadCase> loadCases = new List<LoadCase>();

            // Add permanent load cases and coefficients
            AddPermanentLoadCases(permanentLoadGroups, loadCases, combinationType, loadCombGammas);

            // Add variable load cases
            string leadingActionName;
            leadingActionName = AddTemporaryLoadCases(temporaryLoadCases, combinationType, loadCases,
                                                      loadCombGammas);

            // Create load combination
            string loadCombName;
            if (combinationType == ELoadCombinationType.SixTenA)
                loadCombName = "LC " + loadCombNumber.ToString() + " " + loadCombinationNameTag;
            else
                loadCombName = "LC " + loadCombNumber.ToString() + " " + loadCombinationNameTag + " - " + leadingActionName + " as leading action";
            LoadCombination loadCombination = new LoadCombination(loadCombName, LoadCombinationType(combinationType), loadCases, loadCombGammas);

            return loadCombination;
        }

        /// <summary>
        /// Converts the Enum combination type to string
        /// </summary>
        /// <param name="combinationType"></param>
        /// <returns>The combination type as a string</returns>
        private LoadCombType LoadCombinationType(ELoadCombinationType combinationType)
        {
            LoadCombType loadCombinationType = LoadCombType.UltimateOrdinary;

            if (combinationType == ELoadCombinationType.SixTenA || combinationType == ELoadCombinationType.SixTenB)
                loadCombinationType = LoadCombType.UltimateOrdinary;
            else if (combinationType == ELoadCombinationType.Characteristic)
                loadCombinationType = LoadCombType.ServiceabilityCharacteristic;
            else if (combinationType == ELoadCombinationType.Frequent)
                loadCombinationType = LoadCombType.ServiceabilityFrequent;
            else if (combinationType == ELoadCombinationType.QuasiPermanent)
                loadCombinationType = LoadCombType.ServiceabilityQuasiPermanent;
            return loadCombinationType;
        }

        /// <summary>
        /// Finds all combinations, where load cases from the first group are leading actions, from the provided <paramref name="loadGroups"/>, 
        /// by selecting one case from each group
        /// </summary>
        /// <param name="loadGroups"></param>
        /// <returns>Two nested lists with load cases and their associated load groups, where each element in the out list is a list with load cases or groups</returns>
        private List<List<ModelLoadCaseInGroup>> PermuteLoadCases<T>(List<T> loadGroups) where T: LoadGroupBase
        {
            List<List<ModelLoadCaseInGroup>> loadPermutations = new List<List<ModelLoadCaseInGroup>>();

            // To keep track of next load case in each of the load groups
            int[] indices = new int[loadGroups.Count];
            int combIter = 0;

            // Start with first load case in each load group
            for (int i = 0; i < loadGroups.Count; i++)
                indices[i] = 0;

            while (true)
            {
                loadPermutations.Add(new List<ModelLoadCaseInGroup>());

                // Store current combination and associated load group
                for (int i = 0; i < loadGroups.Count; i++)
                {
                    if (loadGroups[i].Relationship == ELoadGroupRelationship.Entire)
                        loadPermutations[combIter].AddRange(loadGroups[i].ModelLoadCase);
                    else
                        loadPermutations[combIter].Add(loadGroups[i].ModelLoadCase[indices[i]]);
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
                    return loadPermutations;

                // If load group found, move to next load case in that group
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
        public void GenerateLoadCombinations(List<LoadGroupBase> loadGroups, string loadCombinationNameTag, ELoadCombinationType combinationType)
        {
            // Separate out the permanent load groups and the temporary
            List<LoadGroupPermanent> permanentLoadGroups = loadGroups.Where(lg => lg is LoadGroupPermanent).Cast<LoadGroupPermanent>().ToList();
            List<LoadGroupTemporary> temporaryLoadGroups = loadGroups.Where(lg => lg is LoadGroupTemporary).Cast<LoadGroupTemporary>().ToList();

            // Initiate lists for storing load cases for each combination
            int loadCombCounter = 1;
            List<LoadCase> loadCasesInComb = new List<LoadCase>();
            List<double> loadCombGammas = new List<double>();

            // Create a load combination for each permutation of temporary loads
            List<List<ModelLoadCaseInGroup>> loadCasePermutations = CreateLoadCasePermutations(temporaryLoadGroups);
            for (int i = 0; i < loadCasePermutations.Count; i++)
            {
                LoadCombination currentLoadCombination;
                currentLoadCombination = CreateLoadCombination(loadCasePermutations[i], loadCombCounter,
                                                               loadCombinationNameTag, permanentLoadGroups, combinationType);
                AddLoadCombination(currentLoadCombination);
                
                // If 6.10a only one combination is needed
                if (combinationType == ELoadCombinationType.SixTenA)
                    break;

                loadCombCounter++;
            }
        }

        /// <summary>
        /// Finds all permutations of load cases by letting all temporary load groups be the leading action once
        /// </summary>
        /// <param name="temporaryLoadGroups">The load groups that hold the load cases to permute</param>
        /// <returns>A list with lists of load cases, with the first load case being the leading action</returns>
        private List<List<ModelLoadCaseInGroup>> CreateLoadCasePermutations(List<LoadGroupTemporary> temporaryLoadGroups)
        {
            List<List<ModelLoadCaseInGroup>> loadCasePermutations = new List<List<ModelLoadCaseInGroup>>();

            // Find all combinations of temporary load groups, such that all groups are leading action once (order of accompyaning actions not included)
            for (int i = 0; i < temporaryLoadGroups.Count(); i++)
            {
                if (!temporaryLoadGroups[i].LeadingCases)
                    continue; // Dont generate combinations where current groups load cases are leading actions

                ExtensionMethods.Swap(temporaryLoadGroups, i, 0);
                loadCasePermutations.AddRange(PermuteLoadCases(temporaryLoadGroups));
            }
            return loadCasePermutations;
        }

        /// <summary>
        /// Adds the load cases in <paramref name="permanentLoadGroups"/> to <paramref name="loadCases"/> and computes associated load coefficients
        /// </summary>
        /// <param name="permanentLoadGroups"></param>
        /// <param name="loadCases"></param>
        /// <param name="combinationType"></param>
        /// <param name="loadCombGammas"></param>
        private void AddPermanentLoadCases<T>(List<T> permanentLoadGroups, List<LoadCase> loadCases, ELoadCombinationType combinationType, 
                                              List<double> loadCombGammas) where T : LoadGroupBase
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
        /// <param name="loadCombGammas">A list of combiation factors to add to</param>
        private string AddTemporaryLoadCases(List<ModelLoadCaseInGroup> temporaryLoadCases, ELoadCombinationType combinationType,
                                             List<LoadCase> loadCases, List<double> loadCombGammas)
        {
            string leadingActionName = "";

            // Get the indices of the leading action (or actions)
            List<int> indicesLeadingCases = GetLeadingLoadCaseIndices(temporaryLoadCases);

            for (int i = 0; i < temporaryLoadCases.Count; i++)
            {
                LoadGroupTemporary parentLoadGroup = (LoadGroupTemporary)temporaryLoadCases[i].LoadGroup;

                // If combination type is not 6.10a, include load case
                if (combinationType != ELoadCombinationType.SixTenA)
                    loadCases.Add(parentLoadGroup.GetCorrespondingCompleteLoadCase(temporaryLoadCases[i]));

                // If leading action
                if (indicesLeadingCases.Contains(i))
                {
                    leadingActionName = FindLeadingActionName(parentLoadGroup, temporaryLoadCases[i]);
                    AddCombinationFactorLeadingAction(parentLoadGroup, combinationType, loadCombGammas);
                }
                else //Else accompanying action
                    AddCombinationFactorsAccompanyingAction(parentLoadGroup, combinationType, loadCombGammas);

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
        /// Adds the load combination gamma factor based on the combination type
        /// </summary>
        /// <param name="parentLoadGroup">The load group to take the coefficients from</param>
        /// <param name="combinationType">The type of load combination</param>
        /// <param name="loadCombGammas">The list of combinations factors to append to</param>
        private void AddCombinationFactorLeadingAction(LoadGroupTemporary parentLoadGroup, ELoadCombinationType combinationType, List<double> loadCombGammas)
        {
            // Assign the combination factors
            if (combinationType == ELoadCombinationType.SixTenB)
                loadCombGammas.Add(parentLoadGroup.SafetyFactor);
            else if (combinationType == ELoadCombinationType.Characteristic)
                loadCombGammas.Add(1);
            else if (combinationType == ELoadCombinationType.Frequent)
                loadCombGammas.Add(parentLoadGroup.Psi1);
            else if (combinationType == ELoadCombinationType.QuasiPermanent)
                loadCombGammas.Add(parentLoadGroup.Psi2);
        }

        /// <summary>
        /// Adds the load combination gamma factor based on the combination type
        /// </summary>
        /// <param name="parentLoadGroup">The load group to take the coefficients from</param>
        /// <param name="combinationType">The type of load combination</param>
        /// <param name="loadCombGammas">The list of combinations factors to append to</param>
        private void AddCombinationFactorsAccompanyingAction(LoadGroupTemporary parentLoadGroup, ELoadCombinationType combinationType, List<double> loadCombGammas)
        {
            if (combinationType == ELoadCombinationType.SixTenB)
                loadCombGammas.Add(parentLoadGroup.SafetyFactor * parentLoadGroup.Psi0);
            else if (combinationType == ELoadCombinationType.Characteristic)
                loadCombGammas.Add(parentLoadGroup.Psi0);
            else if (combinationType == ELoadCombinationType.Frequent)
                loadCombGammas.Add(parentLoadGroup.Psi2);
            else if (combinationType == ELoadCombinationType.QuasiPermanent)
                loadCombGammas.Add(parentLoadGroup.Psi2);
        }

        /// <summary>
        /// Returns the leading action name or the leading load group name
        /// </summary>
        /// <param name="parentLoadGroup">The parent load group of the leading load case</param>
        /// <param name="leadingLoadCase">The leading load case</param>
        /// <returns>The name of the leading action or load group</returns>
        private string FindLeadingActionName(LoadGroupTemporary parentLoadGroup, ModelLoadCaseInGroup leadingLoadCase)
        {
            string leadingActionName = "";
            // Find a suitable name for the leading action
            if (parentLoadGroup.Relationship == ELoadGroupRelationship.Alternative)
                leadingActionName = parentLoadGroup.GetCorrespondingCompleteLoadCase(leadingLoadCase).Name;
            else if (parentLoadGroup.Relationship == ELoadGroupRelationship.Entire)
                // Use the groups name
                leadingActionName = parentLoadGroup.Name;
            return leadingActionName;
        }

        /// <summary>
        /// Gets the indices of the leading action (0) if load case relation is alternative or the whole groups indices if load case relation is entire
        /// </summary>
        ///<param name="loadCases">List of load cases</param>
        /// <returns>Returns a list of indices</returns>
        private List<int> GetLeadingLoadCaseIndices(List<ModelLoadCaseInGroup> loadCases)
        {
            List<int> indicesLeadingCases = new List<int>();

            // The first load cases is always one of the leading actions (entire) or the sole leading action (alternative)
            LoadGroupTemporary parentLoadGroupLeadingAction = (LoadGroupTemporary)loadCases[0].LoadGroup;

            // If relationship is entire the whole group is leading action
            if (parentLoadGroupLeadingAction.Relationship == ELoadGroupRelationship.Entire)
                indicesLeadingCases.AddRange(Enumerable.Range(0, parentLoadGroupLeadingAction.LoadCase.Count));
            else if (parentLoadGroupLeadingAction.Relationship == ELoadGroupRelationship.Alternative)
                indicesLeadingCases.Add(0);

            return indicesLeadingCases;
        }
    }
}
