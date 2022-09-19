// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialDatabaseListMaterialNamesOBSOLETE : GH_Component
    {
        public MaterialDatabaseListMaterialNamesOBSOLETE()
          : base("MaterialDatabase.ListMaterialNames", "ListMaterialNames",
              "Lists the names of all Materials in MaterialDatabase.",
              CategoryName.Name(), SubCategoryName.Cat4a())
        { 

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("MaterialDatabase", "MaterialDatabase", "MaterialDatabase.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("MaterialNames", "MaterialNames", "List of material names.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Materials.MaterialDatabase materialDatabase = null;
            if (!DA.GetData(0, ref materialDatabase)) { return; }
            if (materialDatabase == null) { return; }

            //
            List<string> materialNames = materialDatabase.MaterialNames();

            // output
            DA.SetDataList(0, materialNames);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialDatabaseListMaterialNames;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("ce42c6c1-aa5b-4e04-b47e-1b0d31444603"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }   
}