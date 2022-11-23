using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Loads;

namespace FemDesign.Examples
{
    public class Program
    {
        static void Main()
        {
            // PRACTICAL EXAMPLE: COMBINING LOAD GROUPS
            // This example shows the steps for creating a complete set of load combination,
            // combining existing load groups. In addition to the main program, there are two
            // public functions at the bottom - CombineULS and CombineSLS.

            // This example was last using the ver. 21.1.0 FEM-Design API.


            // Create load cases
            LoadCase deadLoad1 = new LoadCase("Deadload1", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase deadLoad2 = new LoadCase("Deadload2", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase liveLoad1 = new LoadCase("Liveload1", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase liveLoad2 = new LoadCase("Liveload2", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase windLoad1 = new LoadCase("Windload1", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase windLoad2 = new LoadCase("Windload2", LoadCaseType.Static, LoadCaseDuration.Permanent);
            List<LoadCase> loadCasesDeadLoads = new List<LoadCase>() { deadLoad1, deadLoad2 };
            List<LoadCase> loadCaseCategoryA = new List<LoadCase>() { liveLoad1, liveLoad2 };
            List<LoadCase> loadCaseCategoryWind = new List<LoadCase>() { windLoad1, windLoad2 };
            List<LoadCase> loadCases = loadCasesDeadLoads.Concat(loadCaseCategoryA).Concat(loadCaseCategoryWind).ToList();


            // Get the load categories that hold the coefficients
            var loadCategoryDatabase = LoadCategoryDatabase.GetDefault();
            LoadCategory loadCategoryA = loadCategoryDatabase.LoadCategoryByName("A");
            LoadCategory loadCategoryWind = loadCategoryDatabase.LoadCategoryByName("Wind");


            // Create load groups
            var LGPermanent = new LoadGroupPermanent(1, 1.35, 1, 1, loadCasesDeadLoads, ELoadGroupRelationship.Entire, 0.89, "LGPermanent");
            var LGA = new LoadGroupTemporary(1.5, loadCategoryA.Psi0, loadCategoryA.Psi1, loadCategoryA.Psi2, true, loadCaseCategoryA, ELoadGroupRelationship.Alternative, "LGCategoryA");
            var LGWind = new LoadGroupTemporary(1.5, loadCategoryWind.Psi0, loadCategoryWind.Psi1, loadCategoryWind.Psi2, true, loadCaseCategoryWind, ELoadGroupRelationship.Alternative, "LGCategoryWind");

            var loadGroups = new List<LoadGroupBase>() { LGPermanent, LGA, LGWind };


            // Wrap the load groups so that they can be added to the load group table 
            var generalLoadGroups = new List<ModelGeneralLoadGroup>() { new ModelGeneralLoadGroup(LGPermanent), new ModelGeneralLoadGroup(LGA),
                                                                        new ModelGeneralLoadGroup(LGWind)};

            // Generate ULS and SLS Combinations
            List<LoadCombination> loadCombinations;
            LoadCombinationTable loadCombinationTable = new LoadCombinationTable();
            CombineULS(loadGroups, loadCombinationTable);
            CombineSLS(loadGroups, loadCombinationTable);
            loadCombinations = loadCombinationTable.LoadCombinations;


            // Create model and open file in FEM design
            var model = new Model(Country.S, null, null, loadCases, loadCombinations, generalLoadGroups);

            using (var app = new FemDesign.FemDesignConnection())
            {
                app.Open(model);
                app.Disconnect();
            }
        }


        public static void CombineULS(List<LoadGroupBase> generalLoadGroups, LoadCombinationTable loadCombinationTable)
        {
            //Generate load combinations from the load groups
            List<string> loadCombTypeNames = new List<string>() { "6.10a", "6.10b" };
            List<ELoadCombinationType> loadCombTypes = new List<ELoadCombinationType>() { ELoadCombinationType.SixTenA, ELoadCombinationType.SixTenB };

            for (int i = 0; i < loadCombTypes.Count; i++)
                loadCombinationTable.GenerateLoadCombinations(generalLoadGroups, loadCombTypeNames[i], loadCombTypes[i]);
        }


        public static void CombineSLS(List<LoadGroupBase> generalloadGroups, LoadCombinationTable loadCombinationTable)
        {
            //Generate load combinations from the load groups
            List<string> loadCombTypeNames = new List<string>() { "Characteristic", "Frequent", "Quasi-permanent" };
            List<ELoadCombinationType> loadCombTypes = new List<ELoadCombinationType>() { ELoadCombinationType.Characteristic, ELoadCombinationType.Frequent, ELoadCombinationType.QuasiPermanent };
            for (int i = 0; i < loadCombTypes.Count; i++)
                loadCombinationTable.GenerateLoadCombinations(generalloadGroups, loadCombTypeNames[i], loadCombTypes[i]);
        }
    }
}
