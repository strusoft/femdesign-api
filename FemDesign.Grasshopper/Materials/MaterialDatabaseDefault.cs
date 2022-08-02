// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

using System.Linq;

namespace FemDesign.Grasshopper
{
    public class MaterialDatabaseDefault: GH_Component
    {
        public MaterialDatabaseDefault(): base("MaterialDatabase.Default", "Default", "Load the default MaterialDatabase for each respective country.", "FEM-Design", "Materials")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code: D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Steel", "Steel", "", GH_ParamAccess.list);
            pManager.Register_GenericParam("Concrete", "Concrete", "", GH_ParamAccess.list);
            pManager.Register_GenericParam("Timber", "Timber", "", GH_ParamAccess.list);
            pManager.Register_GenericParam("Reinforcement", "Reinforcement", "", GH_ParamAccess.list);
            pManager.Register_GenericParam("Stratum", "Stratum", "", GH_ParamAccess.list);
            pManager.Register_GenericParam("Custom", "Custom", "", GH_ParamAccess.list);

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


            FemDesign.Materials.MaterialDatabase materialDatabase = FemDesign.Materials.MaterialDatabase.GetDefault(countryCode);
            (var steel, var concrete, var timber, var reinforcement, var stratum, var custom) = materialDatabase.ByType();

            // set output
            DA.SetDataList(0, steel);
            DA.SetDataList(1, concrete);
            DA.SetDataList(2, timber);
            DA.SetDataList(3, reinforcement);
            DA.SetDataList(4, stratum);
            DA.SetDataList(5, custom);
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
            get { return new Guid("{BC3E170C-C4BB-46C9-87BC-F5E23B54AF5D}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }   
}