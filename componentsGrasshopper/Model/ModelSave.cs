// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class ModelSave: GH_Component
    {
        public ModelSave(): base("Model.Save", "Save", "Save model to .struxml", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the model as .struxml", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 
            FemDesign.Model model = null;
            string filePath = null;

            // get data
            if(!DA.GetData(0, ref model))
            {
                return;
            }
            if (!DA.GetData(1, ref filePath))
            {
                return;
            }
            if (model == null || filePath == null)
            {
                return;
            }

            //
            model.SerializeModel(filePath);
            }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelSave;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ba85f198-112f-404e-a759-8417308849d7"); }
        }
    }  
}