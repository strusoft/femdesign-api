// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialDatabaseFromStruxml : GH_Component
    {
        public MaterialDatabaseFromStruxml() : base("MaterialDatabase.FromStruxml", "FromStruxml", "Load a custom MaterialDatabase from a .struxml file.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
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
            // get input
            string filePath = null;
            if (!DA.GetData(0, ref filePath)) { return; }
            if (filePath == null) { return; }

            //
            FemDesign.Materials.MaterialDatabase materialDatabase = FemDesign.Materials.MaterialDatabase.DeserializeStruxml(filePath);
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
            get { return new Guid("{201CD37E-D0EC-433D-9460-E9EF555D0AA8}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}