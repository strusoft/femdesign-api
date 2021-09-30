// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCoefficientsDefault: GH_Component
    {
        public LoadCoefficientsDefault(): base("LoadCoefficientsDatabase.Default", "Default", "Load the default LoadCoefficientsDatabase for each respective country.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code: D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCoefficientsDefault", "LoadCoefficientsDefault", "Default LoadCoefficientsDefault.", GH_ParamAccess.item);
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
            FemDesign.Loads.LoadCoefficientsDatabase loadCoefficientsDatabase = FemDesign.Loads.LoadCoefficientsDatabase.GetDefault(countryCode);

            // set output
            DA.SetData(0, loadCoefficientsDatabase);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialDatabaseDefault;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("9d4b48dd-7b21-4df7-b02a-477a62eefbbf"); }
        }
    }   
}