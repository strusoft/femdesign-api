// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.LibraryItems;
using Grasshopper.Kernel;


using FemDesign.LibraryItems;

namespace FemDesign.Grasshopper
{
    public class VehicleDatabaseFromStruxml : GH_Component
    {
        public VehicleDatabaseFromStruxml() : base("VehicleDatabase", "VehicleDatabase Default or FromStruxml", "Load VehicleDatabase from a .struxml file or default.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Vehicle", "Vehicle", "", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string filePath = null;
            DA.GetData(0, ref filePath);

            List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> vehicleDatabase;

            if (filePath == null)
            {
                vehicleDatabase = VehicleDatabase.DeserializeFromResource();
            }
            else
            {
                vehicleDatabase = VehicleDatabase.DeserializeFromFilePath(filePath);
            }

            // set output
            DA.SetDataList(0, vehicleDatabase);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Vehicle;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{464CC295-BE45-4B9D-8269-3B3F21206470}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}