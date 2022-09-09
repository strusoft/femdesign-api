// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
   public class GetLoadCategoryByName: GH_Component
    {
        public GetLoadCategoryByName(): base("LoadCategory.GetLoadCategoryByName", "GetLoadCategoryByName", "Get LoadCategory from LoadCategoryDatabase by name.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadsCategoryDatabase", "LoadsCategoryDatabase", "LoadsCategoryDatabase.", GH_ParamAccess.item);
            pManager.AddTextParameter("LoadCategoryName", "LoadCategoryName", "Name of loda category to retreive.", GH_ParamAccess.item);
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Load category", "Load category", "Load category.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Loads.LoadCategoryDatabase loadCategoryDatabase = null;
            string loadCategoryName = null;
            if (!DA.GetData(0, ref loadCategoryDatabase)) return;
            if (!DA.GetData(1, ref loadCategoryName)) return;
            if (loadCategoryDatabase == null || loadCategoryName == null) return;

            FemDesign.Loads.LoadCategory loadCategory = loadCategoryDatabase.LoadCategoryByName(loadCategoryName);

            DA.SetData(0, loadCategory);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadCategoryDatabaseGetCategoryByName;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("4f28cdd5-2078-458a-b55b-98e9c489a26d"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}