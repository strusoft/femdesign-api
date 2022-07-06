// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelReadStrOBSOLETE: GH_Component
    {
        public ModelReadStrOBSOLETE(): base("Model.ReadStr", "ReadStr", "Read model from .str file.", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePathStr", "FilePathStr", "File path to .str file.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathBsc", "FilePathBsc", "File path to .bsc batch-file. Item or list.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("HasExited", "HasExited", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 
            string filePath = null;
            List<string> bscPath = new List<string>();

            // get data
            if (!DA.GetData(0, ref filePath))
            {
                return;
            }
            if (!DA.GetDataList(1, bscPath))
            {
                // pass
            }
            else
            {
                if (bscPath.Count == 0)
                {
                    bscPath = null;
                }
            }
            if (filePath == null)
            {
                return;
            }

            //
            FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.ReadStr(filePath, bscPath);
            FemDesign.Calculate.Application app = new FemDesign.Calculate.Application();
            bool hasExited = app.RunFdScript(fdScript, false, true, false);

            //
            if (hasExited)
            {
                DA.SetData(0, FemDesign.Model.DeserializeFromFilePath(fdScript.StruxmlPath));
                DA.SetData(1, hasExited);
            }
            else
            {
                return;
            }
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelReadStr;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("237b7d25-1a97-4604-9f07-62ef62abf016"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}