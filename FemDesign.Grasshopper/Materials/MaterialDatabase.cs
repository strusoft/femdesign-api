// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace FemDesign.Grasshopper
{
    public class MaterialDatabase : FEM_Design_API_Component
    {
        public MaterialDatabase() : base(" MaterialDatabase", "MaterialDatabase", "Load MaterialDatabase Default or FromStruxml.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "Connect 'ValueList' to get the options.\nNational annex of calculation code: B/COMMON/D/DK/E/EST/FIN/GB/H/LT/N/NL/PL/RO/S/TR\n\nNote: the TR (Turkish) national annex is no longer supported by FEM-Design. The default material database doesn't contain the plastic material properties for code 'TR'.", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml file.\nnote: `CountryCode` will not be use if `FilePath` is specified", GH_ParamAccess.item);
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
            string countryCode = null;
            DA.GetData(0, ref countryCode);

            // get input
            string filePath = null;
            DA.GetData(1, ref filePath);

            FemDesign.Materials.MaterialDatabase materialDatabase;

            if(filePath == null)
            {
                materialDatabase = FemDesign.Materials.MaterialDatabase.GetDefault(countryCode);
            }
            else
            {
                if (countryCode != null)
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Filepath has been provided. `CountryCode` will be omitted.");
                materialDatabase = FemDesign.Materials.MaterialDatabase.DeserializeStruxml(filePath);
            }


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
                return FemDesign.Properties.Resources.MaterialDatabaseFromStruxml;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{89ECE4C6-13E9-49BF-8A1E-CFD88B651A87}"); }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 0, Enum.GetNames(typeof(Country)).ToList(), null, GH_ValueListMode.DropDown);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}