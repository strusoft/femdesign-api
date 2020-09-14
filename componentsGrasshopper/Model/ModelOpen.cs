// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class ModelOpen: GH_Component
    {
        public ModelOpen(): base("Model.Open", "Open", "Open model in FEM-Design.", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the model as .struxml", GH_ParamAccess.item);
            pManager.AddBooleanParameter("CloseOpenWindows", "CloseOpenWindows", "If true all open windows will be closed without prior warning.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 
            FemDesign.Model model = null;
            string filePath = null;
            bool closeOpenWindows = false;

            // get data
            if(!DA.GetData(0, ref model))
            {
                return;
            }
            if (!DA.GetData(1, ref filePath))
            {
                return;
            }
            if (!DA.GetData(2, ref closeOpenWindows))
            {
                // pass
            }
            if (model == null || filePath == null)
            {
                return;
            }

            //
            model.SerializeModel(filePath);
            model.FdApp.OpenStruxml(filePath, closeOpenWindows);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelOpen;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("237b7d25-1a97-4604-9f07-68ef62abf016"); }
        }
    } 
}