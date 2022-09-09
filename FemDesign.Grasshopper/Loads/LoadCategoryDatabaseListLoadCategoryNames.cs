// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCategoryDatabaseListLoadCategoryNames : GH_Component
    {
        public LoadCategoryDatabaseListLoadCategoryNames()
          : base("LoadCategoryDatabase.ListLoadCategoryNames", "ListLoadCategoryNames",
              "Lists the names of all LoadCategories in LoadCategoryDatabase.",
              CategoryName.Name(), SubCategoryName.Cat3())
        { 

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCategoryDatabase", "LoadCategoryDatabase", "LoadCategoryDatabase.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("LoadCategoryNames", "LoadCategoryNames", "List of load categories names.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.LoadCategoryDatabase loadCategoryDatabase  = null;
            if (!DA.GetData(0, ref loadCategoryDatabase )) { return; }
            if (loadCategoryDatabase  == null) { return; }

            //
            List<string> loadCategoryNames = loadCategoryDatabase.LoadCategoryNames();

            // output
            DA.SetDataList(0, loadCategoryNames);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadCategoryDatabaseListCategoryNames;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("ce42c6c1-aa5b-4e04-b47e-1b0d31484603"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}