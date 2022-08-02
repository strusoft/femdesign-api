// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialDatabaseFromStruxml : GH_Component
    {
        public MaterialDatabaseFromStruxml() : base("MaterialDatabase.FromStruxml", "FromStruxml", "Load a custom MaterialDatabase from a .struxml file.", "FEM-Design", "Materials")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Steel", "Steel", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Concrete", "Concrete", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Timber", "Timber", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Reinforcement", "Reinforcement", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Stratum", "Stratum", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("Custom", "Custom", "", GH_ParamAccess.list);
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
            get { return new Guid("831686d3-0300-4c76-9da6-28548fc9f36d"); }
        }

    }
}