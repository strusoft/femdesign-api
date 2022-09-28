// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class MaterialDatabaseDefault: GH_Component
    {
        public MaterialDatabaseDefault(): base("MaterialDatabase.Default", "Default", "Load the default MaterialDatabase for each respective country.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "Connect 'ValueList' to get the options.\nNational annex of calculation code: D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Steel Material", "Steel Material", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Concrete Material", "Concrete Material", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Timber Material", "Timber Material", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Reinforcement Material", "Reinforcement Material", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Stratum Material", "Stratum Material", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Custom Material", "Custom Material", "", GH_ParamAccess.list);

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

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 0, new List<string>
            { "D","DK","EST","FIN","GB","H","N","PL","RO","S","TR"
            }, null, 0);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }   
}