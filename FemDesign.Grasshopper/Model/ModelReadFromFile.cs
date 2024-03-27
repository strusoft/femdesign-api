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
            DA.GetData(0, ref filePath);


            if (_FileName.IsASCII(filePath))
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "File path has special characters. This might cause problems.");


            bool fileExist = OnPingDocument().IsFilePathDefined;
            if (!fileExist)
            {
                // hops issue
                //var folderPath = System.IO.Directory.GetCurrentDirectory();
                string tempPath = System.IO.Path.GetTempPath();
                System.IO.Directory.SetCurrentDirectory(tempPath);
            }
            else
            {
                var ghFilePath = OnPingDocument().FilePath;
                var dirName = System.IO.Path.GetDirectoryName(ghFilePath);
                System.IO.Directory.SetCurrentDirectory(dirName);
            }




            Model model = null;

            // create Task
            if(filePath.EndsWith(".str"))
            {
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
            }
            else if(filePath.EndsWith(".struxml"))
            {
                model = Model.DeserializeFromFilePath(filePath);
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
            get { return new Guid("{A556C259-234A-43EB-A401-4D69C82098D9}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}