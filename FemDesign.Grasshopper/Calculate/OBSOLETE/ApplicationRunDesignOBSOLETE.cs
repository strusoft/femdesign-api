// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelRunDesignOBSOLETE: GH_Component
    {
        public override GH_Exposure Exposure => GH_Exposure.hidden;  
        public ModelRunDesignOBSOLETE(): base("Application.RunDesign", "RunDesign", "Run analysis and design of model. .csv list files and .docx documentation files are saved in the same work directory as StruxmlPath.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Mode", "Mode", "Design mode: rc, steel or timber.", GH_ParamAccess.item);
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the model as .struxml", GH_ParamAccess.item);
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
            pManager.AddTextParameter("BscPath", "BscPath", "File path(s) to batch-file (.bsc) to run. Optional parameter.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("DocxTemplatePath", "DocxTemplatePath", "File path to documenation template file (.dsc) to run. Optional parameter.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("EndSession", "EndSession", "If true FEM-Design will close after execution.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("CloseOpenWindows", "CloseOpenWindows", "If true all open windows will be closed without prior warning.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("HasExited", "HasExited", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 
            string mode = null;
            FemDesign.Model model = null;
            string filePath = null;
            FemDesign.Calculate.Analysis analysis = null;
            FemDesign.Calculate.Design design = null;
            List<string> bscPath = new List<string>();
            string docxTemplatePath = "";
            bool endSession = false;
            bool closeOpenWindows = false;
            bool runNode = false;


            // get data
            if (!DA.GetData(0, ref mode))
            {
                return;
            }
            if (!DA.GetData(1, ref model))
            {
                return;
            }
            if (!DA.GetData(2, ref filePath))
            {
                return;
            }
            if (!DA.GetData(3, ref analysis))
            {
                return;
            }
            if (!DA.GetData(4, ref design))
            {
                return;
            }
            if (!DA.GetDataList(5, bscPath))
            {
                // pass
            }
            if (!DA.GetData(6, ref docxTemplatePath))
            {
                // pass
            }
            if (!DA.GetData(7, ref endSession))
            {
                // pass
            }         
            if (!DA.GetData(8, ref closeOpenWindows))
            {
                // pass
            }
            if (!DA.GetData(9, ref runNode))
            {
                // pass
            }
            if (mode == null || model == null || filePath == null || analysis == null)
            {
                return;
            }

            //
            if (runNode)
            {
                model.SerializeModel(filePath);
                analysis.SetLoadCombinationCalculationParameters(model);
                bool rtn = model.FdApp.RunDesign(mode, filePath, analysis, design, bscPath, docxTemplatePath, endSession, closeOpenWindows);
                DA.SetData(0, rtn);
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
                return FemDesign.Properties.Resources.ModelRunDesign;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("09404618-b65d-4baa-89db-77cd07a5673b"); }
        }
    }  
}