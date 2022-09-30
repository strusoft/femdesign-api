// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    
    public class ModelRunAnalysisOBSOLETE: GH_Component
    {
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public ModelRunAnalysisOBSOLETE(): base("Application.RunAnalysis", "RunAnalysis", "Run analysis of model. .csv list files and .docx documentation files are saved in the same work directory as StruxmlPath.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the model as .struxml", GH_ParamAccess.item);
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
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
            FemDesign.Model model = null;
            string filePath = null;
            FemDesign.Calculate.Analysis analysis = null;
            List<string> bscPath = new List<string>();
            string docxTemplatePath = "";
            bool endSession = false;
            bool closeOpenWindows = false;
            bool runNode = false;

            // get data
            if(!DA.GetData(0, ref model))
            {
                return;
            }
            if (!DA.GetData(1, ref filePath))
            {
                return;
            }
            if (!DA.GetData(2, ref analysis))
            {
                return;
            }
            if (!DA.GetDataList(3, bscPath))
            {
                // pass
            }
            if (!DA.GetData(4, ref docxTemplatePath))
            {
                // pass
            }
            if (!DA.GetData(5, ref endSession))
            {
                // pass
            }         
            if (!DA.GetData(6, ref closeOpenWindows))
            {
                // pass
            }
            if (!DA.GetData(7, ref runNode))
            {
                // pass
            }
            if (model == null || filePath == null || analysis == null)
            {
                return;
            }

            //
            if (runNode)
            {
                model.SerializeModel(filePath);
                analysis.SetLoadCombinationCalculationParameters(model);
                bool rtn = model.FdApp.RunAnalysis(filePath, analysis, bscPath, docxTemplatePath, endSession, closeOpenWindows);
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
                return FemDesign.Properties.Resources.ModelRunAnalysis;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c2391cbf-3f0c-4dc9-ada1-84ead8d68ea1"); }
        }
    }    
}