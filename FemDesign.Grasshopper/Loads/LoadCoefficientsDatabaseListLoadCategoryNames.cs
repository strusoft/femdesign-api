// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCoefficientsDatabaseListLoadCategoryNames : GH_Component
    {
        public LoadCoefficientsDatabaseListLoadCategoryNames()
          : base("LoadCoefficientsDatabase.ListLoadCategoryNames", "ListLoadCategoryNames",
              "Lists the names of all LoadCategories in LoadCoefficientsDatabase.",
              "FemDesign", "Loads")
        { 

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCoefficientsDatabase", "LoadCoefficientsDatabase", "LoadCoefficientsDatabase.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("LoadCategoryNames", "LoadCategoryNames", "List of load categories names.", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.LoadCoefficientsDatabase loadCoefficientsDatabase  = null;
            if (!DA.GetData(0, ref loadCoefficientsDatabase )) { return; }
            if (loadCoefficientsDatabase  == null) { return; }

            //
            List<string> loadCategoryNames = loadCoefficientsDatabase.LoadCategoryNames();

            // output
            DA.SetDataList(0, loadCategoryNames);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadCoefficientsDataBase;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("ce42c6c1-aa5b-4e04-b47e-1b0d31484603"); }
        }
    }   
}