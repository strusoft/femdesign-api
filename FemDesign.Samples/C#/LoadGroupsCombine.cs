using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FemDesign;
using FemDesign.Loads;


namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        public static void LoadGroupsCombine()
        {
            LoadCase deadLoad1 = new LoadCase("Deadload1", "dead_load", "permanent");
            LoadCase deadLoad2 = new LoadCase("Deadload2", "dead_load", "permanent");
            LoadCase liveLoad1 = new LoadCase("Liveload1", "static", "permanent");
            LoadCase liveLoad2 = new LoadCase("Liveload2", "static", "permanent");

            List<LoadCase> loadCasesDeadLoads = new List<LoadCase>() { deadLoad1, deadLoad2 };
            List<LoadCase> loadCaseLiveLoads = new List<LoadCase>() { liveLoad1, liveLoad2 };

            var loadCategoryDatabase = LoadCategoryDatabase.GetDefault();
            LoadCategory loadCategory = loadCategoryDatabase.LoadCategoryByName("A");

            var LG1 = new LoadGroupPermanent(1, 1.35, 1, 1, loadCasesDeadLoads, ELoadGroupRelationship.Simultaneous, 0.89);
            var LG2 = new LoadGroupTemporary(1.5, loadCategory.Psi0, loadCategory.Psi1, loadCategory.Psi2, true, loadCaseLiveLoads, ELoadGroupRelationship.Alternative);

            var loadGroups = new List<LoadGroupBase>() { LG1, LG2 };

            //Generate load combinations from the load groups
            LoadCombinationCollection loadCombinationCollection = new LoadCombinationCollection();
            string loadCombTypeName = "6.10b";

            List<LoadCombination> loadCombinations;
            List<LoadCase> usedLoadCases;
            (loadCombinations, usedLoadCases) = loadCombinationCollection.GenerateLoadCombinations(loadGroups, loadCombTypeName, ELoadCombinationType.SixTenB);

            // Create model and open file in FEM design
            var model2 = new Model("S", null, null, usedLoadCases, loadCombinations, null);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LoadGroups.struxml");
            model2.SerializeModel(path);
            var app = new Calculate.Application();
            app.OpenStruxml(path, true);
        }
    }
}
