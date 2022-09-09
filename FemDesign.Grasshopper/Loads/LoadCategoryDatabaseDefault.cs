// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCategoryDefault: GH_Component
    {
        public LoadCategoryDefault(): base("LoadCategoryDatabase.Default", "Default", "Load the default LoadCategoryDatabase for each respective country.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code: D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCategoryDefault", "LoadCategoryDefault", "Default LoadCategoryDatabase.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string countryCode = "S";
            if (!DA.GetData(0, ref countryCode))
            {
                // pass
            }
            if (countryCode == null)
            {
                return;
            }

            //
            FemDesign.Loads.LoadCategoryDatabase loadCategoryDatabase = FemDesign.Loads.LoadCategoryDatabase.GetDefault(countryCode);

            // set output
            DA.SetData(0, loadCategoryDatabase);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadCategoryDatabaseDefault;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("9d4b48dd-7b21-4df7-b02a-477a62eefbbf"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}