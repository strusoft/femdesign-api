// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelReadStr: GH_Component
    {
        public ModelReadStr(): base("Model.ReadStr", "ReadStr", "Read model from .str file.", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("StrPath", "StrPath", "File path to FEM-Design model (.str) file.", GH_ParamAccess.item);
            pManager.AddTextParameter("ResultTypes", "ResultTypes", "Results to be extracted from model. This might require the model to have been analysed. Item or list.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Results", "Results", "Results.", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = null;
            List<string> resultTypes = new List<string>();

            DA.GetData("StrPath", ref filePath);
            DA.GetDataList("ResultTypes", resultTypes);
            if (filePath == null)
            {
                return;
            }

            var _resultTypes = resultTypes.Select(r => GenericClasses.EnumParser.Parse<Results.ResultType>(r));

            var (model, results) = Model.ReadStr(filePath, _resultTypes, false, true, true);

            DA.SetData("FdModel", model);
            DA.SetDataList("Results", results);
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
            get { return new Guid("e5d933c4-9217-4ffa-9f82-15a5a26c9967"); }
        }
    } 
}