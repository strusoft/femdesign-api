// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelOpen: GH_Component
    {
        public ModelOpen(): base("Model.Open", "Open", "Open model in FEM-Design.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the model as .struxml.\nIf not specified, the file will be saved using the name and location folder of your .gh script.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("CloseOpenWindows", "CloseOpenWindows", "If true all open windows will be closed without prior warning.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 
            FemDesign.Model model = null;
            DA.GetData(0, ref model);

            // get data
            string filePath = null;
            if(!DA.GetData(1, ref filePath))
            {
                bool fileExist = OnPingDocument().IsFilePathDefined;
                if (!fileExist)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Save your .gh script or specfy a FilePath.");
                    return;
                }
                filePath = OnPingDocument().FilePath;
                filePath = System.IO.Path.ChangeExtension(filePath, "struxml");
            }

            bool closeOpenWindows = false;
            DA.GetData(2, ref closeOpenWindows);

            bool runNode = true;
            DA.GetData(3, ref runNode);

            //
            if (runNode)
            {
                model.Open(filePath, closeOpenWindows);
            }
            else
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "RunNode is set to false!");
            }
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

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}