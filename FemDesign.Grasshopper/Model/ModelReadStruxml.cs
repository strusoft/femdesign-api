// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelReadStruxml: GH_Component
    {
        public ModelReadStruxml(): base("Model.ReadStruxml", "ReadStruxml", "Read model from .struxml. Add entities to model. Note: Only supported elements will loaded from the .struxml model.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            string filePath = null;
            if (!DA.GetData(0, ref filePath))
            {
                return;
            }
            
            if (filePath == null) 
            {
                return;
            }

            //
            FemDesign.Model obj = FemDesign.Model.DeserializeFromFilePath(filePath);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelFromStruxml;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("4e60944b-bad8-4211-9c24-0b62012bbacf"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}