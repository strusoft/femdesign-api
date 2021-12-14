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
            LoadCase deadLoad = new LoadCase("Deadload", "dead_load", "permanent");
            LoadCase liveLoad = new LoadCase("Liveload", "static", "permanent");

            List<LoadCase> loadCases = new List<LoadCase>() { deadLoad, liveLoad };

            var loadCategoryDatabase = LoadCategoryDatabase.GetDefault();
            LoadCategory loadCategory = loadCategoryDatabase.LoadCategoryByName("A");

            var LG1 = new LoadGroup("LG-1", ELoadGroupType.Permanent, loadCases, 0.5, 1.35, 0.9, ELoadGroupRelationship.Simultaneous, 0.89);
            var LG2 = new LoadGroup("LG-2", ELoadGroupType.Temporary, loadCases, loadCategory, 0.5, 1.35, 0.9, ELoadGroupRelationship.Alternative, true);

            var loadGroups = new List<LoadGroup> { LG1 };

            var model2 = new Model("S", null, null, loadCases, null, loadGroups);

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LoadGroups.struxml");
            model2.SerializeModel(path);
            var app = new Calculate.Application();
            app.OpenStruxml(path, true);
        }
    }
}
