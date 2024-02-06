// https://strusoft.com/
using System;
using System.IO;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Text.RegularExpressions;
using FemDesign.Calculate;
using FemDesign.Results;
using Grasshopper.Kernel.Data;
using System.Threading.Tasks;

namespace FemDesign.Grasshopper
{
    public class ModelReadFromFile : FEM_Design_API_Component
    {
        public ModelReadFromFile() : base("Model.ReadFromFile", "ReadFromFile", "Read model from .struxml or .str. Note: Only supported elements will loaded from the .struxml model.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml or .str file.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "Model", "Model.", GH_ParamAccess.item);
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

            if (_FileName.IsASCII(filePath))
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "File path has special characters. This might cause problems.");


            Model model = null;

            // create Task
            var t = Task.Run((Action)(() =>
            {
                var connection = new FemDesign.FemDesignConnection(minimized: true);
                connection.Open(filePath);
                model = connection.GetModel();
                connection.Dispose();
            }));

            t.ConfigureAwait(false);

            try
            {
                t.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }


            // get output
            DA.SetData(0, model);
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
            get { return new Guid("{B5904420-07A9-4F1E-BB1E-0282A962A70F}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}