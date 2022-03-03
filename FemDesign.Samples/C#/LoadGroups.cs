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
        public static void CreateLoadGroups()
        {
            LoadCase deadLoad1 = new LoadCase("Deadload1", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase deadLoad2 = new LoadCase("Deadload2", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase liveLoad1 = new LoadCase("Liveload1", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase liveLoad2 = new LoadCase("Liveload2", LoadCaseType.Static, LoadCaseDuration.Permanent);

            List<LoadCase> loadCasesDeadLoads = new List<LoadCase>() { deadLoad1, deadLoad2 };
            List<LoadCase> loadCaseLiveLoads = new List<LoadCase>() { liveLoad1, liveLoad2 };

            var loadCategoryDatabase = LoadCategoryDatabase.GetDefault();
            LoadCategory loadCategory = loadCategoryDatabase.LoadCategoryByName("A");

            var LG1 = new ModelGeneralLoadGroup(new LoadGroupPermanent(1, 1.35, 1, 1, loadCasesDeadLoads, ELoadGroupRelationship.Simultaneous, 0.89, "LG1"));
            var LG2 = new ModelGeneralLoadGroup(new LoadGroupTemporary(1.5, loadCategory.Psi0, loadCategory.Psi1, loadCategory.Psi2, true, loadCaseLiveLoads, ELoadGroupRelationship.Alternative, "LG2"));

            var loadGroups = new List<ModelGeneralLoadGroup>() { LG1, LG2 };

            var model2 = new Model(Country.S, null, null, loadCasesDeadLoads.Concat(loadCaseLiveLoads).ToList(), null, loadGroups);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LoadGroups.struxml");
            model2.SerializeModel(path);
            var app = new Calculate.Application();
            app.OpenStruxml(path, true);
        }
    }
}
