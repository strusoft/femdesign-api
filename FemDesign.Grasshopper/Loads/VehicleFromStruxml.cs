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
        public VehicleDatabaseFromStruxml() : base("VehicleDatabase.FromStruxml", "FromStruxml", "Load a custom VehicleDatabase from a .struxml file.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Vehicle", "Vehicle", "", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string filePath = null;
            if (!DA.GetData(0, ref filePath)) { return; }
            if (filePath == null) { return; }

            //
            var vehicleDatabase = VehicleDatabase.DeserializeFromFilePath(filePath);

            // set output
            DA.SetDataList(0, vehicleDatabase);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{464CC295-BE45-4B9D-8269-3B3F21206470}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}