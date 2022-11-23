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
    internal class Program
    {
        static void Main()
        {
            // PRACTICAL EXAMPLE: SETTING UP LOAD GROUPS
            // This example shows the core steps of creating load groups for a model.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


            // CREATING LOAD CASES
            LoadCase deadLoad1 = new LoadCase("Deadload1", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase deadLoad2 = new LoadCase("Deadload2", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            LoadCase liveLoad1 = new LoadCase("Liveload1", LoadCaseType.Static, LoadCaseDuration.Permanent);
            LoadCase liveLoad2 = new LoadCase("Liveload2", LoadCaseType.Static, LoadCaseDuration.Permanent);

            List<LoadCase> loadCasesDeadLoads = new List<LoadCase>() { deadLoad1, deadLoad2 };
            List<LoadCase> loadCaseLiveLoads = new List<LoadCase>() { liveLoad1, liveLoad2 };


            // FETCHING LOAD CATEGORY DATABASE
            var loadCategoryDatabase = LoadCategoryDatabase.GetDefault();
            LoadCategory loadCategory = loadCategoryDatabase.LoadCategoryByName("A");


            // CREATING LOAD GROUPS
            var LG1 = new ModelGeneralLoadGroup(new LoadGroupPermanent(1, 1.35, 1, 1, loadCasesDeadLoads, ELoadGroupRelationship.Simultaneous, 0.89, "LG1"));
            var LG2 = new ModelGeneralLoadGroup(new LoadGroupTemporary(1.5, loadCategory.Psi0, loadCategory.Psi1, loadCategory.Psi2, true, loadCaseLiveLoads, ELoadGroupRelationship.Alternative, "LG2"));

            var loadGroups = new List<ModelGeneralLoadGroup>() { LG1, LG2 };


            // CREATING AND OPENING NEW MODEL
            var model = new Model(Country.S, null, null, loadCasesDeadLoads.Concat(loadCaseLiveLoads).ToList(), null, loadGroups);

            using (var app = new FemDesignConnection())
            {
                app.Open(model);
                app.Disconnect();
            }
        }
    }
}
